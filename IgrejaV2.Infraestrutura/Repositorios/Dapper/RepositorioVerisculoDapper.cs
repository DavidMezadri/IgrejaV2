using Dapper;
using IgrejaV2.Dominio.Entidades;
using IgrejaV2.Dominio.Interfaces;

namespace IgrejaV2.Infraestrutura.Repositorios.Dapper
{
    public class RepositorioVerisculoDapper(string connectionString) : Base.RepositorioBaseDapper<Versiculo>(connectionString), IRepositorioVersiculo
    {
        protected override string NomeTabela => "versiculos";

        public async Task<Versiculo?> ObterPorLivroCaptituloNumeroAsync(int livro, int capitulo, int numero, int traducaoId)
        {
            var sql = @"
                SELECT * FROM versiculos
                WHERE livro = @Livro AND capitulo = @Capitulo AND numero = @Numero AND traducao_id = @TraducaoId AND deletado = false
                LIMIT 1";

            using var conn = CriarConexao();
            return await conn.QueryFirstOrDefaultAsync<Versiculo>(
                new CommandDefinition(sql, new { Livro = livro, Capitulo = capitulo, Numero = numero, TraducaoId = traducaoId }));
        }

        public async Task<IEnumerable<Versiculo>> ObterPorLivroAsync(int livro, int traducaoId)
        {
            var sql = @"
                SELECT * FROM versiculos
                WHERE livro = @Livro AND traducao_id = @TraducaoId AND deletado = false
                ORDER BY capitulo, numero";

            using var conn = CriarConexao();
            return await conn.QueryAsync<Versiculo>(
                new CommandDefinition(sql, new { Livro = livro, TraducaoId = traducaoId }));
        }

        public async Task<IEnumerable<Versiculo>> ObterPorLivroCaptituloAsync(int livro, int capitulo, int traducaoId)
        {
            var sql = @"
                SELECT * FROM versiculos
                WHERE livro = @Livro AND capitulo = @Capitulo AND traducao_id = @TraducaoId AND deletado = false
                ORDER BY numero";

            using var conn = CriarConexao();
            return await conn.QueryAsync<Versiculo>(
                new CommandDefinition(sql, new { Livro = livro, Capitulo = capitulo, TraducaoId = traducaoId }));
        }
    }
}
