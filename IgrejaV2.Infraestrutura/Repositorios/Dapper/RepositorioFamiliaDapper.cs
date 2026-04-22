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

        public async Task<Familia?> ObterComMembrosAsync(int id, CancellationToken ct = default)
        {
            var sql = @"
                SELECT
                    f.id, f.nome, f.responsavel_id, f.ativo, f.observacoes,
                    p.id, p.nome, p.email, p.telefone, p.ativo, p.familia_id
                FROM familias f
                LEFT JOIN pessoas p ON p.familia_id = f.id AND p.deletado = false
                WHERE f.id = @Id AND f.deletado = false";

            using var conn = CriarConexao();
            var familiaDict = new Dictionary<int, Familia>();

            await conn.QueryAsync<Familia, Pessoa, Familia>(
                new CommandDefinition(sql, new { Id = id }, cancellationToken: ct),
                (familia, membro) =>
                {
                    if (!familiaDict.TryGetValue(familia.Id, out var familiaExistente))
                    {
                        familiaExistente = familia;
                        familiaDict[familia.Id] = familiaExistente;
                    }
                    if (membro != null)
                        familiaExistente.Membros.Add(membro);
                    return familiaExistente;
                },
                splitOn: "id"
            );

            return familiaDict.Values.FirstOrDefault();
        }
    }
}
