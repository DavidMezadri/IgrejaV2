using System.Linq.Expressions;
using IgrejaV2.Dominio.Entidades;
using IgrejaV2.Dominio.Interfaces;

namespace IgrejaV2.Testes.Fakes;

public class RepositorioEmMemoria<T> : IRepositorio<T> where T : EntidadeBase
{
    protected readonly List<T> Dados = [];
    private int _proximoId = 1;

    public Task<T?> ObterPorIdAsync(int id, CancellationToken ct = default)
        => Task.FromResult(Dados.FirstOrDefault(e => e.Id == id));

    public Task<T?> ObterPrimeiroAsync(Expression<Func<T, bool>> predicado, CancellationToken ct = default)
        => Task.FromResult(Dados.AsQueryable().FirstOrDefault(predicado));

    public Task<IEnumerable<T>> ListarTodosAsync(CancellationToken ct = default)
        => Task.FromResult<IEnumerable<T>>([.. Dados]);

    public Task<IEnumerable<T>> ListarAsync(Expression<Func<T, bool>> predicado, CancellationToken ct = default)
        => Task.FromResult(Dados.AsQueryable().Where(predicado).AsEnumerable());

    public Task<bool> ExisteAsync(Expression<Func<T, bool>> predicado, CancellationToken ct = default)
        => Task.FromResult(Dados.AsQueryable().Any(predicado));

    public Task<int> ContarAsync(Expression<Func<T, bool>>? predicado = null, CancellationToken ct = default)
        => Task.FromResult(predicado is null ? Dados.Count : Dados.AsQueryable().Count(predicado));

    public Task AdicionarAsync(T entidade, CancellationToken ct = default)
    {
        entidade.Id = _proximoId++;
        Dados.Add(entidade);
        return Task.CompletedTask;
    }

    public Task AdicionarVariosAsync(IEnumerable<T> entidades, CancellationToken ct = default)
    {
        foreach (var e in entidades)
        {
            e.Id = _proximoId++;
            Dados.Add(e);
        }
        return Task.CompletedTask;
    }

    public Task AtualizarAsync(T entidade, CancellationToken ct = default)
        => Task.CompletedTask;

    public Task RemoverAsync(T entidade, CancellationToken ct = default)
    {
        Dados.Remove(entidade);
        return Task.CompletedTask;
    }

    public Task RemoverPorIdAsync(int id, CancellationToken ct = default)
    {
        Dados.RemoveAll(e => e.Id == id);
        return Task.CompletedTask;
    }

    public Task<int> SalvarAlteracoesAsync(CancellationToken ct = default)
        => Task.FromResult(1);
}
