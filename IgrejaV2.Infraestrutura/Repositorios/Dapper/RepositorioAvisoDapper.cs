using Dapper;
using IgrejaV2.Dominio.Entidades;
using IgrejaV2.Dominio.Interfaces;
using Npgsql;

namespace IgrejaV2.Infraestrutura.Repositorios.Dapper
{
    public class RepositorioAvisoDapper : IgrejaV2.Infraestrutura.Repositorios.Base.RepositorioBaseDapper<Aviso>, IRepositorioAviso
    {
        protected override string NomeTabela => "avisos";

        public RepositorioAvisoDapper(string connectionString) : base(connectionString) { }

        public async Task<IEnumerable<Aviso>> ObterAvisosAtivosAsync(CancellationToken ct = default)
        {
            var sql = @"
                SELECT * FROM avisos
                WHERE ativo = true AND deletado = false
                ORDER BY data DESC";

            using var conn = CriarConexao();
            return await conn.QueryAsync<Aviso>(new CommandDefinition(sql, cancellationToken: ct));
        }
    }
}
