using Dapper;
using IgrejaV2.Dominio.Entidades;
using IgrejaV2.Dominio.Interfaces;
using Npgsql;

namespace IgrejaV2.Infraestrutura.Repositorios.Dapper
{
    public class RepositorioPessoaDapper : IgrejaV2.Infraestrutura.Repositorios.Base.RepositorioBaseDapper<Pessoa>, IRepositorioPessoa
    {
        protected override string NomeTabela => "pessoas";

        public RepositorioPessoaDapper(string connectionString) : base(connectionString) { }

        public async Task<IEnumerable<Pessoa>> ObterAtivosAsync(CancellationToken ct = default)
        {
            var sql = @"
                SELECT * FROM pessoas
                WHERE ativo = true AND deletado = false
                ORDER BY nome";

            using var conn = CriarConexao();
            return await conn.QueryAsync<Pessoa>(new CommandDefinition(sql, cancellationToken: ct));
        }

        public async Task<IEnumerable<Pessoa>> ObterPorFamiliaAsync(int familiaId, CancellationToken ct = default)
        {
            var sql = @"
                SELECT * FROM pessoas
                WHERE familia_id = @FamiliaId AND deletado = false
                ORDER BY nome";

            using var conn = CriarConexao();
            return await conn.QueryAsync<Pessoa>(
                new CommandDefinition(sql, new { FamiliaId = familiaId }, cancellationToken: ct));
        }

        public async Task<IEnumerable<Pessoa>> BuscarPorNomeAsync(string nome, CancellationToken ct = default)
        {
            var sql = @"
                SELECT * FROM pessoas
                WHERE nome ILIKE @Nome AND deletado = false
                ORDER BY nome";

            using var conn = CriarConexao();
            return await conn.QueryAsync<Pessoa>(
                new CommandDefinition(sql, new { Nome = $"%{nome}%" }, cancellationToken: ct));
        }
    }
}
