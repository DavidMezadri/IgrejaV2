using Dapper;
using IgrejaV2.Dominio.Entidades;
using IgrejaV2.Dominio.Interfaces;
using Npgsql;

namespace IgrejaV2.Infraestrutura.Repositorios.Dapper
{
    public class RepositorioPresencaDapper : IgrejaV2.Infraestrutura.Repositorios.Base.RepositorioBaseDapper<Presenca>, IRepositorioPresenca
    {
        protected override string NomeTabela => "presencas";

        public RepositorioPresencaDapper(string connectionString) : base(connectionString) { }

        public async Task<IEnumerable<Presenca>> ObterPorEventoAsync(int eventoId, CancellationToken ct = default)
        {
            var sql = @"
                SELECT * FROM presencas
                WHERE evento_id = @EventoId AND deletado = false
                ORDER BY id";

            using var conn = CriarConexao();
            return await conn.QueryAsync<Presenca>(new CommandDefinition(sql, new { EventoId = eventoId }, cancellationToken: ct));
        }

        public async Task<IEnumerable<Presenca>> ObterPorPessoaAsync(int pessoaId, CancellationToken ct = default)
        {
            var sql = @"
                SELECT * FROM presencas
                WHERE pessoa_id = @PessoaId AND deletado = false
                ORDER BY data_criacao DESC";

            using var conn = CriarConexao();
            return await conn.QueryAsync<Presenca>(new CommandDefinition(sql, new { PessoaId = pessoaId }, cancellationToken: ct));
        }
    }
}
