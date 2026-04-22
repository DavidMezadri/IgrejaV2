using Dapper;
using IgrejaV2.Dominio.Entidades;
using IgrejaV2.Dominio.Interfaces;
using Npgsql;

namespace IgrejaV2.Infraestrutura.Repositorios.Dapper
{
    public class RepositorioIgrejaDapper : IgrejaV2.Infraestrutura.Repositorios.Base.RepositorioBaseDapper<Igreja>, IRepositorioIgreja
    {
        protected override string NomeTabela => "igrejas";

        public RepositorioIgrejaDapper(string connectionString) : base(connectionString) { }

        public async Task<Igreja?> ObterComPatrimoniosAsync(int id, CancellationToken ct = default)
        {
            var sql = @"
                SELECT
                    i.id, i.nome, i.cnpj, i.telefone, i.email, i.data_fundacao, i.ativa, i.observacoes,
                    p.id, p.nome, p.descricao, p.numero_patrimonio, p.ativo, p.valor_aquisicao, p.data_aquisicao
                FROM igrejas i
                LEFT JOIN patrimonios p ON p.igreja_id = i.id AND p.deletado = false
                WHERE i.id = @Id AND i.deletado = false";

            using var conn = CriarConexao();
            var igrejaDict = new Dictionary<int, Igreja>();

            await conn.QueryAsync<Igreja, Patrimonio, Igreja>(
                new CommandDefinition(sql, new { Id = id }, cancellationToken: ct),
                (igreja, patrimonio) =>
                {
                    if (!igrejaDict.TryGetValue(igreja.Id, out var igrejaExistente))
                    {
                        igrejaExistente = igreja;
                        igrejaDict[igreja.Id] = igrejaExistente;
                    }
                    if (patrimonio != null)
                        igrejaExistente.Patrimonios.Add(patrimonio);
                    return igrejaExistente;
                },
                splitOn: "id"
            );

            return igrejaDict.Values.FirstOrDefault();
        }
    }
}
