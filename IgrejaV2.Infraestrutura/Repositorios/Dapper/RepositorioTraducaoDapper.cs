using Dapper;
using IgrejaV2.Dominio.Entidades;
using IgrejaV2.Dominio.Interfaces;

namespace IgrejaV2.Infraestrutura.Repositorios.Dapper
{
    public class RepositorioTraducaoDapper(string connectionString) : Base.RepositorioBaseDapper<Traducao>(connectionString), IRepositorioTraducao
    {
        protected override string NomeTabela => "traducoes";

        public async Task<Traducao?> ObterPorAbreviacaoAsync(string abreviacao)
        {
            var sql = @"
                SELECT * FROM traducoes
                WHERE abreviacao = @Abreviacao AND deletado = false
                LIMIT 1";

            using var conn = CriarConexao();
            return await conn.QueryFirstOrDefaultAsync<Traducao>(
                new CommandDefinition(sql, new { Abreviacao = abreviacao }));
        }
    }
}
