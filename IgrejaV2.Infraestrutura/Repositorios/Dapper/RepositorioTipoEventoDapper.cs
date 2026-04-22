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
            var sql = @"
                SELECT * FROM tipos_evento
                WHERE ativo = true AND deletado = false
                ORDER BY nome";

            using var conn = CriarConexao();
            return await conn.QueryAsync<TipoEvento>(new CommandDefinition(sql, cancellationToken: ct));
        }
    }
}
