using IgrejaV2.Dominio.Entidades;
using IgrejaV2.Dominio.Interfaces;

namespace IgrejaV2.Infraestrutura.Repositorios.Dapper
{
    public class RepositorioLogDapper : IRepositorioLog
    {
        private readonly string _connectionString;
        public RepositorioLogDapper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Task AdicionarAsync(Log entidade, CancellationToken ct = default) => throw new NotImplementedException();
        public Task AtualizarAsync(Log entidade, CancellationToken ct = default) => throw new NotImplementedException();
        public Task DeletarAsync(int id, CancellationToken ct = default) => throw new NotImplementedException();
        public Task<Log?> ObterPorIdAsync(int id, CancellationToken ct = default) => throw new NotImplementedException();
        public Task<IEnumerable<Log>> ObterTodosAsync(CancellationToken ct = default) => throw new NotImplementedException();
    }
}
