using Dapper;
using IgrejaV2.Dominio.Entidades;
using IgrejaV2.Dominio.Interfaces;
using Npgsql;

namespace IgrejaV2.Infraestrutura.Repositorios.Dapper
{
    public class RepositorioPatrimonioDapper : IgrejaV2.Infraestrutura.Repositorios.Base.RepositorioBaseDapper<Patrimonio>, IRepositorioPatrimonio
    {
        protected override string NomeTabela => "patrimonios";

        public RepositorioPatrimonioDapper(string connectionString) : base(connectionString) { }

        public async Task<IEnumerable<Patrimonio>> ObterPorIgrejaAsync(int igrejaId, CancellationToken ct = default)
        {
            var sql = @"
                SELECT * FROM patrimonios
                WHERE igreja_id = @IgrejaId AND deletado = false
                ORDER BY nome";

            using var conn = CriarConexao();
            return await conn.QueryAsync<Patrimonio>(
                new CommandDefinition(sql, new { IgrejaId = igrejaId }, cancellationToken: ct));
        }
    }
}
