using IgrejaV2.Dominio.Entidades;
using IgrejaV2.Dominio.Interfaces;

namespace IgrejaV2.Infraestrutura.Repositorios.Dapper
{
    public class RepositorioUsuarioDapper : IRepositorioUsuario
    {
        private readonly string _connectionString;
        public RepositorioUsuarioDapper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Task AdicionarAsync(Usuario entidade, CancellationToken ct = default) => throw new NotImplementedException();
        public Task AtualizarAsync(Usuario entidade, CancellationToken ct = default) => throw new NotImplementedException();
        public Task DeletarAsync(int id, CancellationToken ct = default) => throw new NotImplementedException();
        public Task<Usuario?> ObterPorIdAsync(int id, CancellationToken ct = default) => throw new NotImplementedException();
        public Task<IEnumerable<Usuario>> ObterTodosAsync(CancellationToken ct = default) => throw new NotImplementedException();
    }
}
