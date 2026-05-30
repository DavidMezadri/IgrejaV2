using System.Linq.Expressions;
using IgrejaV2.Dominio.Entidades;
using IgrejaV2.Dominio.Interfaces;

namespace IgrejaV2.Testes.Fakes;

public class RepositorioFamiliaEmMemoria : IRepositorioFamilia
{
    private readonly List<Familia> _dados = [];
    private int _proximoId = 1;

    public Task<Familia?> ObterPorIdAsync(int id, CancellationToken ct = default)
        => Task.FromResult(_dados.FirstOrDefault(f => f.Id == id));

    public Task<Familia?> ObterPrimeiroAsync(Expression<Func<Familia, bool>> predicado, CancellationToken ct = default)
        => Task.FromResult(_dados.AsQueryable().FirstOrDefault(predicado));

    public Task<IEnumerable<Familia>> ListarTodosAsync(CancellationToken ct = default)
        => Task.FromResult<IEnumerable<Familia>>([.._dados]);

    public Task<IEnumerable<Familia>> ListarAsync(Expression<Func<Familia, bool>> predicado, CancellationToken ct = default)
        => Task.FromResult(_dados.AsQueryable().Where(predicado).AsEnumerable());

    public Task<bool> ExisteAsync(Expression<Func<Familia, bool>> predicado, CancellationToken ct = default)
        => Task.FromResult(_dados.AsQueryable().Any(predicado));

    public Task<int> ContarAsync(Expression<Func<Familia, bool>>? predicado = null, CancellationToken ct = default)
        => Task.FromResult(predicado is null ? _dados.Count : _dados.AsQueryable().Count(predicado));

    public Task AdicionarAsync(Familia entidade, CancellationToken ct = default)
    {
        entidade.Id = _proximoId++;
        _dados.Add(entidade);
        return Task.CompletedTask;
    }

    public Task AdicionarVariosAsync(IEnumerable<Familia> entidades, CancellationToken ct = default)
    {
        foreach (var e in entidades)
        {
            e.Id = _proximoId++;
            _dados.Add(e);
        }
        return Task.CompletedTask;
    }

    public Task AtualizarAsync(Familia entidade, CancellationToken ct = default)
        => Task.CompletedTask;

    public Task RemoverAsync(Familia entidade, CancellationToken ct = default)
    {
        _dados.Remove(entidade);
        return Task.CompletedTask;
    }

    public Task RemoverPorIdAsync(int id, CancellationToken ct = default)
    {
        var familia = _dados.FirstOrDefault(f => f.Id == id);
        if (familia != null)
            _dados.Remove(familia);
        return Task.CompletedTask;
    }

    public Task<int> SalvarAlteracoesAsync(CancellationToken ct = default)
        => Task.FromResult(0);

    public Task<Familia?> ObterComMembrosAsync(int id, CancellationToken ct = default)
        => Task.FromResult(_dados.FirstOrDefault(f => f.Id == id));

    public Task<IEnumerable<Familia>> BuscarPorNomeAsync(string nome, CancellationToken ct = default)
        => Task.FromResult(
            _dados.Where(f => f.Nome.Contains(nome, StringComparison.OrdinalIgnoreCase))
                .OrderBy(f => f.Nome)
                .AsEnumerable());
}
