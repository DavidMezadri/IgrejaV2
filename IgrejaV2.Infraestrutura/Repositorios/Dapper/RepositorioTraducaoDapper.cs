using IgrejaV2.Dominio.Entidades;
using IgrejaV2.Dominio.Interfaces;

namespace IgrejaV2.Infraestrutura.Repositorios.Dapper
{
    public class RepositorioTraducaoDapper(string connectionString) : IRepositorioTraducao
    {
        public Task AdicionarAsync(Traducao entidade, CancellationToken ct = default) => Task.CompletedTask;
        public Task AdicionarVariosAsync(IEnumerable<Traducao> entidades, CancellationToken ct = default) => Task.CompletedTask;
        public Task AtualizarAsync(Traducao entidade, CancellationToken ct = default) => Task.CompletedTask;
        public Task<int> ContarAsync(System.Linq.Expressions.Expression<Func<Traducao, bool>>? predicado = null, CancellationToken ct = default) => Task.FromResult(0);
        public Task<bool> ExisteAsync(System.Linq.Expressions.Expression<Func<Traducao, bool>> predicado, CancellationToken ct = default) => Task.FromResult(false);
        public Task<IEnumerable<Traducao>> ListarAsync(System.Linq.Expressions.Expression<Func<Traducao, bool>> predicado, CancellationToken ct = default) => Task.FromResult(Enumerable.Empty<Traducao>());
        public Task<IEnumerable<Traducao>> ListarTodosAsync(CancellationToken ct = default) => Task.FromResult(Enumerable.Empty<Traducao>());
        public Task<Traducao?> ObterPorAbreviacaoAsync(string abreviacao) => Task.FromResult<Traducao?>(null);
        public Task<Traducao?> ObterPorIdAsync(int id, CancellationToken ct = default) => Task.FromResult<Traducao?>(null);
        public Task<Traducao?> ObterPrimeiroAsync(System.Linq.Expressions.Expression<Func<Traducao, bool>> predicado, CancellationToken ct = default) => Task.FromResult<Traducao?>(null);
        public Task RemoverAsync(Traducao entidade, CancellationToken ct = default) => Task.CompletedTask;
        public Task RemoverPorIdAsync(int id, CancellationToken ct = default) => Task.CompletedTask;
        public Task<int> SalvarAlteracoesAsync(CancellationToken ct = default) => Task.FromResult(0);
    }
}
