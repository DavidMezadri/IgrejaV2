using System.Linq.Expressions;
using IgrejaV2.Dominio.Entidades;
using IgrejaV2.Dominio.Interfaces;

namespace IgrejaV2.Testes.Fakes;

public class RepositorioPessoaEmMemoria : IRepositorioPessoa
{
    private readonly List<Pessoa> _dados = [];
    private int _proximoId = 1;

    public Task<Pessoa?> ObterPorIdAsync(int id, CancellationToken ct = default)
        => Task.FromResult(_dados.FirstOrDefault(p => p.Id == id));

    public Task<Pessoa?> ObterPrimeiroAsync(Expression<Func<Pessoa, bool>> predicado, CancellationToken ct = default)
        => Task.FromResult(_dados.AsQueryable().FirstOrDefault(predicado));

    public Task<IEnumerable<Pessoa>> ListarTodosAsync(CancellationToken ct = default)
        => Task.FromResult<IEnumerable<Pessoa>>([.._dados]);

    public Task<IEnumerable<Pessoa>> ListarAsync(Expression<Func<Pessoa, bool>> predicado, CancellationToken ct = default)
        => Task.FromResult(_dados.AsQueryable().Where(predicado).AsEnumerable());

    public Task<bool> ExisteAsync(Expression<Func<Pessoa, bool>> predicado, CancellationToken ct = default)
        => Task.FromResult(_dados.AsQueryable().Any(predicado));

    public Task<int> ContarAsync(Expression<Func<Pessoa, bool>>? predicado = null, CancellationToken ct = default)
        => Task.FromResult(predicado is null ? _dados.Count : _dados.AsQueryable().Count(predicado));

    public Task AdicionarAsync(Pessoa entidade, CancellationToken ct = default)
    {
        entidade.Id = _proximoId++;
        _dados.Add(entidade);
        return Task.CompletedTask;
    }

    public Task AdicionarVariosAsync(IEnumerable<Pessoa> entidades, CancellationToken ct = default)
    {
        foreach (var e in entidades)
        {
            e.Id = _proximoId++;
            _dados.Add(e);
        }
        return Task.CompletedTask;
    }

    public Task AtualizarAsync(Pessoa entidade, CancellationToken ct = default)
        => Task.CompletedTask;

    public Task RemoverAsync(Pessoa entidade, CancellationToken ct = default)
    {
        _dados.Remove(entidade);
        return Task.CompletedTask;
    }

    public Task RemoverPorIdAsync(int id, CancellationToken ct = default)
    {
        var pessoa = _dados.FirstOrDefault(p => p.Id == id);
        if (pessoa != null)
            _dados.Remove(pessoa);
        return Task.CompletedTask;
    }

    public Task<int> SalvarAlteracoesAsync(CancellationToken ct = default)
        => Task.FromResult(0);

    public Task<IEnumerable<Pessoa>> ObterAtivosAsync(CancellationToken ct = default)
        => Task.FromResult(_dados.Where(p => p.Ativo).OrderBy(p => p.Nome).AsEnumerable());

    public Task<IEnumerable<Pessoa>> ObterPorFamiliaAsync(int familiaId, CancellationToken ct = default)
        => Task.FromResult(_dados.Where(p => p.FamiliaId == familiaId).OrderBy(p => p.Nome).AsEnumerable());

    public Task<IEnumerable<Pessoa>> BuscarPorNomeAsync(string nome, CancellationToken ct = default)
        => Task.FromResult(
            _dados.Where(p => p.Nome.Contains(nome, StringComparison.OrdinalIgnoreCase))
                .OrderBy(p => p.Nome)
                .AsEnumerable());
}
