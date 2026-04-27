using Dapper;
using IgrejaV2.Dominio.Entidades;
using IgrejaV2.Dominio.Interfaces;
using Npgsql;

namespace IgrejaV2.Infraestrutura.Repositorios.Dapper
{
    public class RepositorioUsuarioDapper : IgrejaV2.Infraestrutura.Repositorios.Base.RepositorioBaseDapper<Usuario>, IRepositorioUsuario
    {
        protected override string NomeTabela => "usuarios";

        public RepositorioUsuarioDapper(string connectionString) : base(connectionString) { }

        public async Task<Usuario?> ObterPorNomeUsuarioAsync(string nomeUsuario, CancellationToken ct = default)
        {
            const string sql = "SELECT * FROM usuarios WHERE nome_usuario = @NomeUsuario AND deletado = false";
            using var conn = CriarConexao();
            return await conn.QueryFirstOrDefaultAsync<Usuario>(
                new CommandDefinition(sql, new { NomeUsuario = nomeUsuario }, cancellationToken: ct));
        }

        public async Task<Usuario?> ObterPorEmailAsync(string email, CancellationToken ct = default)
        {
            const string sql = "SELECT * FROM usuarios WHERE email = @Email AND deletado = false";
            using var conn = CriarConexao();
            return await conn.QueryFirstOrDefaultAsync<Usuario>(
                new CommandDefinition(sql, new { Email = email }, cancellationToken: ct));
        }

        public async Task<Usuario?> ObterPorTokenRecuperacaoAsync(string token, CancellationToken ct = default)
        {
            const string sql = "SELECT * FROM usuarios WHERE token_recuperacao_senha = @Token AND deletado = false";
            using var conn = CriarConexao();
            return await conn.QueryFirstOrDefaultAsync<Usuario>(
                new CommandDefinition(sql, new { Token = token }, cancellationToken: ct));
        }
    }
}
