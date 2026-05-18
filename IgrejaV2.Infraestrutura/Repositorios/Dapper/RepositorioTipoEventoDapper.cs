using Dapper;
using IgrejaV2.Dominio.Entidades;
using IgrejaV2.Dominio.Interfaces;
using Npgsql;

namespace IgrejaV2.Infraestrutura.Repositorios.Dapper
{
    public class RepositorioTipoEventoDapper : IgrejaV2.Infraestrutura.Repositorios.Base.RepositorioBaseDapper<TipoEvento>, IRepositorioTipoEvento
    {
        protected override string NomeTabela => "tipos_evento";

        public RepositorioTipoEventoDapper(string connectionString) : base(connectionString) { }

        public async Task<IEnumerable<TipoEvento>> ObterAtivosAsync(CancellationToken ct = default)
        {
            var sql = $@"
                SELECT id, nome, descricao, publico_alvo, requer_presenca
                FROM {NomeTabela}
                WHERE ativo = true AND deletado = false
                ORDER BY nome";

            using var conn = CriarConexao();
            return await conn.QueryAsync<TipoEvento>(new CommandDefinition(sql, cancellationToken: ct));
        }

        public override async Task<TipoEvento?> ObterPorIdAsync(int id, CancellationToken ct = default)
        {
            var sql = @$"
                        SELECT id, nome, descricao, publico_alvo, requer_presenca, ativo
                        FROM {NomeTabela}
                        WHERE  id = @Id";

            using var conn = CriarConexao();
            return await conn.QueryFirstOrDefaultAsync<TipoEvento?>(
                new CommandDefinition(sql, new { Id = id }, cancellationToken: ct));
        }

        public override async Task<IEnumerable<TipoEvento>> ListarTodosAsync(CancellationToken ct = default)
        {
            var sql = @$"
                        SELECT id, nome, descricao, publico_alvo, requer_presenca, ativo
                        FROM {NomeTabela}
                        WHERE deletado = false
                        ORDER BY nome asc";

            using var conn = CriarConexao();
            return await conn.QueryAsync<TipoEvento>(new CommandDefinition(sql, cancellationToken: ct));
        }
    }
}
