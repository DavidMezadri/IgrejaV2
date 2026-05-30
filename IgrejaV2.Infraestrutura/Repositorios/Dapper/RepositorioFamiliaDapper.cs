using Dapper;
using IgrejaV2.Dominio.Entidades;
using IgrejaV2.Dominio.Interfaces;
using Npgsql;

namespace IgrejaV2.Infraestrutura.Repositorios.Dapper
{
    public class RepositorioFamiliaDapper : IgrejaV2.Infraestrutura.Repositorios.Base.RepositorioBaseDapper<Familia>, IRepositorioFamilia
    {
        protected override string NomeTabela => "familias";

        public RepositorioFamiliaDapper(string connectionString) : base(connectionString) { }

        public override async Task<IEnumerable<Familia>> ListarTodosAsync(CancellationToken ct = default)
        {
            var sql = $@"
                        SELECT
                            f.id, f.nome, f.responsavel_id, f.ativo, f.observacoes,
                            r.id, r.nome,
                            p.id, p.nome, p.email, p.telefone, p.ativo, p.familia_id
                        FROM {NomeTabela} f
                        LEFT JOIN pessoas r ON r.id = f.responsavel_id AND r.deletado = false
                        LEFT JOIN pessoas p ON p.familia_id = f.id AND p.deletado = false
                        WHERE f.deletado = false";

            using var conn = CriarConexao();
            var familiaDict = new Dictionary<int, Familia>();

            await conn.QueryAsync<Familia, Pessoa, Pessoa, Familia>(
                new CommandDefinition(sql, cancellationToken: ct),
                (familia, responsavel, membro) =>
                {
                    if (!familiaDict.TryGetValue(familia.Id, out var familiaExistente))
                    {
                        familiaExistente = familia;
                        familiaExistente.Responsavel = responsavel;
                        familiaDict[familia.Id] = familiaExistente;
                    }
                    if (membro != null)
                        familiaExistente.Membros.Add(membro);
                    return familiaExistente;
                },
                splitOn: "id, id"
            );

            return familiaDict.Values;
        }

        public override async Task<Familia?> ObterPorIdAsync(int id, CancellationToken ct = default)
        {
            var sql = @$"
                SELECT
                    f.id, f.nome, f.responsavel_id, f.ativo, f.observacoes,
                    p.id, p.nome
                FROM {NomeTabela} f
                LEFT JOIN pessoas p ON p.familia_id = f.id AND p.deletado = false
                WHERE f.id = @Id AND f.deletado = false";

            using var conn = CriarConexao();

            var resultado = await conn.QueryAsync<Familia, Pessoa, Familia>(
                sql,
                (familia, responsavel) =>
                {
                    familia.Responsavel = responsavel;
                    return familia;
                },
                new { Id = id },
                splitOn: "id"
            );

            return resultado.FirstOrDefault();
        }

        public async Task<Familia?> ObterComMembrosAsync(int id, CancellationToken ct = default)
        {
            var sql = $@"
                    SELECT
                        f.id, f.nome, f.responsavel_id, f.ativo, f.observacoes,
                        r.id, r.nome,
                        p.id, p.nome, p.email, p.telefone, p.ativo, p.familia_id
                    FROM {NomeTabela} f
                    LEFT JOIN pessoas r ON r.id = f.responsavel_id AND r.deletado = false
                    LEFT JOIN pessoas p ON p.familia_id = f.id AND p.deletado = false
                    WHERE f.id = @Id AND f.deletado = false";

            using var conn = CriarConexao();
            var familiaDict = new Dictionary<int, Familia>();

            await conn.QueryAsync<Familia, Pessoa, Pessoa, Familia>(
                new CommandDefinition(sql, new { Id = id }, cancellationToken: ct),
                (familia, responsavel, membro) =>
                {
                    if (!familiaDict.TryGetValue(familia.Id, out var familiaExistente))
                    {
                        familiaExistente = familia;
                        familiaExistente.Responsavel = responsavel;
                        familiaDict[familia.Id] = familiaExistente;
                    }
                    if (membro != null)
                        familiaExistente.Membros.Add(membro);
                    return familiaExistente;
                },
                splitOn: "id, id"
            );

            return familiaDict.Values.FirstOrDefault();
        }

        public async Task<IEnumerable<Familia>> BuscarPorNomeAsync(string nome, CancellationToken ct = default)
        {
            var sql = @"
                SELECT * FROM familias
                WHERE nome ILIKE @Nome AND deletado = false
                ORDER BY nome";

            using var conn = CriarConexao();
            return await conn.QueryAsync<Familia>(
                new CommandDefinition(sql, new { Nome = $"%{nome}%" }, cancellationToken: ct));
        }
    }
}
