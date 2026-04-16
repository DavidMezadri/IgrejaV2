using IgrejaV2.Dominio.Entidades;
using IgrejaV2.Dominio.Interfaces;

namespace IgrejaV2.Infraestrutura.Repositorios.Dapper
{
    public class RepositorioPresencaDapper : IRepositorioPresenca
    {
        private readonly string _connectionString;
        public RepositorioPresencaDapper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Task AdicionarAsync(Presenca entidade, CancellationToken ct = default) => throw new NotImplementedException();
        public Task AtualizarAsync(Presenca entidade, CancellationToken ct = default) => throw new NotImplementedException();
        public Task DeletarAsync(int id, CancellationToken ct = default) => throw new NotImplementedException();
        public Task<Presenca?> ObterPorIdAsync(int id, CancellationToken ct = default) => throw new NotImplementedException();
        public Task<IEnumerable<Presenca>> ObterTodosAsync(CancellationToken ct = default) => throw new NotImplementedException();
    }
}
