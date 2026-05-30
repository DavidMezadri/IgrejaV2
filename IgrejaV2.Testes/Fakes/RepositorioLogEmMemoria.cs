using System.Linq.Expressions;
using IgrejaV2.Dominio.Entidades;
using IgrejaV2.Dominio.Interfaces;

namespace IgrejaV2.Testes.Fakes;

public class RepositorioLogEmMemoria : IRepositorioLog
{
    private readonly List<Log> _dados = [];
    private int _proximoId = 1;

    public Task<Log?> ObterPorIdAsync(int id, CancellationToken ct = default)
        => Task.FromResult(_dados.FirstOrDefault(l => l.Id == id));

    public Task<Log?> ObterPrimeiroAsync(Expression<Func<Log, bool>> predicado, CancellationToken ct = default)
        => Task.FromResult(_dados.AsQueryable().FirstOrDefault(predicado));

    public Task<IEnumerable<Log>> ListarTodosAsync(CancellationToken ct = default)
        => Task.FromResult<IEnumerable<Log>>([.._dados]);

    public Task<IEnumerable<Log>> ListarAsync(Expression<Func<Log, bool>> predicado, CancellationToken ct = default)
        => Task.FromResult(_dados.AsQueryable().Where(predicado).AsEnumerable());

    public Task<bool> ExisteAsync(Expression<Func<Log, bool>> predicado, CancellationToken ct = default)
        => Task.FromResult(_dados.AsQueryable().Any(predicado));

    public Task<int> ContarAsync(Expression<Func<Log, bool>>? predicado = null, CancellationToken ct = default)
        => Task.FromResult(predicado is null ? _dados.Count : _dados.AsQueryable().Count(predicado));

    public Task AdicionarAsync(Log entidade, CancellationToken ct = default)
    {
        entidade.Id = _proximoId++;
        _dados.Add(entidade);
        return Task.CompletedTask;
    }

    public Task AdicionarVariosAsync(IEnumerable<Log> entidades, CancellationToken ct = default)
    {
        foreach (var e in entidades)
        {
            e.Id = _proximoId++;
            _dados.Add(e);
        }
        return Task.CompletedTask;
    }

    public Task AtualizarAsync(Log entidade, CancellationToken ct = default)
        => Task.CompletedTask;

    public Task RemoverAsync(Log entidade, CancellationToken ct = default)
    {
        _dados.Remove(entidade);
        return Task.CompletedTask;
    }

    public Task RemoverPorIdAsync(int id, CancellationToken ct = default)
    {
        var log = _dados.FirstOrDefault(l => l.Id == id);
        if (log != null)
            _dados.Remove(log);
        return Task.CompletedTask;
    }

    public Task<int> SalvarAlteracoesAsync(CancellationToken ct = default)
        => Task.FromResult(0);

    public Task<IEnumerable<Log>> ObterPorUsuarioAsync(int usuarioId, CancellationToken ct = default)
        => Task.FromResult(_dados.Where(l => l.UsuarioId == usuarioId).AsEnumerable());

    public Task<IEnumerable<Log>> ObterPorEntidadeAsync(string entidade, int entidadeId, CancellationToken ct = default)
        => Task.FromResult(
            _dados.Where(l => l.Entidade == entidade && l.EntidadeId == entidadeId)
                .AsEnumerable());
}
