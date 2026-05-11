using IgrejaV2.Dominio.Entidades;
using IgrejaV2.Dominio.Interfaces;

namespace IgrejaV2.Infraestrutura.Repositorios.Dapper
{
    public class RepositorioVerisculoDapper(string connectionString) : IRepositorioVersiculo
    {
        public Task AdicionarAsync(Versiculo entidade, CancellationToken ct = default) => Task.CompletedTask;
        public Task AdicionarVariosAsync(IEnumerable<Versiculo> entidades, CancellationToken ct = default) => Task.CompletedTask;
        public Task AtualizarAsync(Versiculo entidade, CancellationToken ct = default) => Task.CompletedTask;
        public Task<int> ContarAsync(System.Linq.Expressions.Expression<Func<Versiculo, bool>>? predicado = null, CancellationToken ct = default) => Task.FromResult(0);
        public Task<bool> ExisteAsync(System.Linq.Expressions.Expression<Func<Versiculo, bool>> predicado, CancellationToken ct = default) => Task.FromResult(false);
        public Task<IEnumerable<Versiculo>> ListarAsync(System.Linq.Expressions.Expression<Func<Versiculo, bool>> predicado, CancellationToken ct = default) => Task.FromResult(Enumerable.Empty<Versiculo>());
        public Task<IEnumerable<Versiculo>> ListarTodosAsync(CancellationToken ct = default) => Task.FromResult(Enumerable.Empty<Versiculo>());
        public Task<Versiculo?> ObterPorIdAsync(int id, CancellationToken ct = default) => Task.FromResult<Versiculo?>(null);
        public Task<Versiculo?> ObterPrimeiroAsync(System.Linq.Expressions.Expression<Func<Versiculo, bool>> predicado, CancellationToken ct = default) => Task.FromResult<Versiculo?>(null);
        public Task<Versiculo?> ObterPorLivroCaptituloNumeroAsync(int livro, int capitulo, int numero, int traducaoId) => Task.FromResult<Versiculo?>(null);
        public Task<IEnumerable<Versiculo>> ObterPorLivroAsync(int livro, int traducaoId) => Task.FromResult(Enumerable.Empty<Versiculo>());
        public Task<IEnumerable<Versiculo>> ObterPorLivroCaptituloAsync(int livro, int capitulo, int traducaoId) => Task.FromResult(Enumerable.Empty<Versiculo>());
        public Task RemoverAsync(Versiculo entidade, CancellationToken ct = default) => Task.CompletedTask;
        public Task RemoverPorIdAsync(int id, CancellationToken ct = default) => Task.CompletedTask;
        public Task<int> SalvarAlteracoesAsync(CancellationToken ct = default) => Task.FromResult(0);
    }
}
