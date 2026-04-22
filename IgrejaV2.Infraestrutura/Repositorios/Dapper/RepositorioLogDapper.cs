using Dapper;
using IgrejaV2.Dominio.Entidades;
using IgrejaV2.Dominio.Interfaces;
using Npgsql;

namespace IgrejaV2.Infraestrutura.Repositorios.Dapper
{
    public class RepositorioLogDapper : IgrejaV2.Infraestrutura.Repositorios.Base.RepositorioBaseDapper<Log>, IRepositorioLog
    {
        protected override string NomeTabela => "logs";

        public RepositorioLogDapper(string connectionString) : base(connectionString) { }

        public async Task<IEnumerable<Log>> ObterPorUsuarioAsync(int usuarioId, CancellationToken ct = default)
        {
            var sql = @"
                SELECT * FROM logs
                WHERE usuario_id = @UsuarioId AND deletado = false
                ORDER BY data_criacao DESC";

            using var conn = CriarConexao();
            return await conn.QueryAsync<Log>(new CommandDefinition(sql, new { UsuarioId = usuarioId }, cancellationToken: ct));
        }

        public async Task<IEnumerable<Log>> ObterPorEntidadeAsync(string entidade, int entidadeId, CancellationToken ct = default)
        {
            var sql = @"
                SELECT * FROM logs
                WHERE entidade = @Entidade AND entidade_id = @EntidadeId AND deletado = false
                ORDER BY data_criacao DESC";

            using var conn = CriarConexao();
            return await conn.QueryAsync<Log>(new CommandDefinition(sql,
                new { Entidade = entidade, EntidadeId = entidadeId }, cancellationToken: ct));
        }
    }
}
