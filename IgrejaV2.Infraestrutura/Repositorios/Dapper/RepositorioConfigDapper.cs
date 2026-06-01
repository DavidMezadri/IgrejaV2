using Dapper;
using IgrejaV2.Dominio.Entidades;
using IgrejaV2.Dominio.Interfaces;
using IgrejaV2.Infraestrutura.Repositorios.Base;
using Npgsql;

namespace IgrejaV2.Infraestrutura.Repositorios.Dapper;

public class RepositorioConfigDapper : RepositorioBaseDapper<Configuracao>, IRepositorioConfig
{
    protected override string NomeTabela => "configuracoes";

    public RepositorioConfigDapper(string connectionString) : base(connectionString) { }

    public async Task<Configuracao?> ObterPorChaveAsync(string chave, CancellationToken ct = default)
    {
        var sql = @"SELECT * FROM configuracoes WHERE chave = @Chave";
        using var conn = CriarConexao();
        return await conn.QueryFirstOrDefaultAsync<Configuracao>(
            new CommandDefinition(sql, new { Chave = chave }, cancellationToken: ct));
    }

    public async Task<IEnumerable<Configuracao>> ObterTodasAsync(CancellationToken ct = default)
    {
        var sql = @"SELECT * FROM configuracoes ORDER BY chave";
        using var conn = CriarConexao();
        return await conn.QueryAsync<Configuracao>(
            new CommandDefinition(sql, cancellationToken: ct));
    }

    public async Task AtualizarOuCriarAsync(string chave, string valor, CancellationToken ct = default)
    {
        var config = await ObterPorChaveAsync(chave, ct);

        if (config is null)
        {
            var sql = @"
                INSERT INTO configuracoes (chave, valor, criado_em, atualizado_em)
                VALUES (@Chave, @Valor, NOW(), NOW())
                RETURNING id, chave, valor, criado_em, atualizado_em";

            using var conn = CriarConexao();
            await conn.QuerySingleAsync<Configuracao>(
                new CommandDefinition(sql, new { Chave = chave, Valor = valor }, cancellationToken: ct));
        }
        else
        {
            var sql = @"
                UPDATE configuracoes
                SET valor = @Valor, atualizado_em = NOW()
                WHERE chave = @Chave";

            using var conn = CriarConexao();
            await conn.ExecuteAsync(
                new CommandDefinition(sql, new { Chave = chave, Valor = valor }, cancellationToken: ct));
        }
    }
}
