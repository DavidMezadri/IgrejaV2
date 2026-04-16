using IgrejaV2.Dominio.Interfaces;
using IgrejaV2.Dominio.Entidades;

namespace IgrejaV2.Infraestrutura.Repositorios.Dapper
{
    // Cuidado: No Dapper o RepositorioBaseDapper precisaria ser criado se houver.
    // Para simplificar a compilação, farei implementar as interfaces cruas se necessário.
    // Usualmente RepositorioEventoDapper apenas recebe string.
    public class RepositorioEventoDapper : IRepositorioEvento
    {
        private readonly string _connectionString;
        public RepositorioEventoDapper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Task<Evento?> ObterComPresencasAsync(int id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Evento>> ObterEventosAtivosAsync(CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<Evento?> ObterUltimoEventoAsync(CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Evento>> ObterTodosAsync(CancellationToken ct = default) => throw new NotImplementedException();
        public Task<Evento?> ObterPorIdAsync(int id, CancellationToken ct = default) => throw new NotImplementedException();
        public Task AdicionarAsync(Evento entidade, CancellationToken ct = default) => throw new NotImplementedException();
        public Task AtualizarAsync(Evento entidade, CancellationToken ct = default) => throw new NotImplementedException();
        public Task DeletarAsync(int id, CancellationToken ct = default) => throw new NotImplementedException();
    }
}
