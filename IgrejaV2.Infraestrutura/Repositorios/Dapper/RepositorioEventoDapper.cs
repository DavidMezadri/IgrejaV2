using Dapper;
using IgrejaV2.Dominio.Entidades;
using IgrejaV2.Dominio.Interfaces;
using Npgsql;

namespace IgrejaV2.Infraestrutura.Repositorios.Dapper
{
    public class RepositorioEventoDapper : IgrejaV2.Infraestrutura.Repositorios.Base.RepositorioBaseDapper<Evento>, IRepositorioEvento
    {
        protected override string NomeTabela => "eventos";

        public RepositorioEventoDapper(string connectionString) : base(connectionString) { }

        public async Task<IEnumerable<Evento>> ObterEventosAtivosAsync(CancellationToken ct = default)
        {
            var sql = @"
                SELECT * FROM eventos
                WHERE ativo = true AND deletado = false
                ORDER BY data_inicio DESC";

            using var conn = CriarConexao();
            return await conn.QueryAsync<Evento>(new CommandDefinition(sql, cancellationToken: ct));
        }

        public async Task<Evento?> ObterComPresencasAsync(int id, CancellationToken ct = default)
        {
            var sql = @"
                SELECT 
                    e.id, e.nome, e.descricao, e.local, e.data_inicio, e.data_fim,
                    e.ativo, e.requer_inscricao, e.tipo_evento_id, e.capacidade_maxima,
                    p.id, p.presente, p.pessoa_id, p.evento_id, p.observacao, p.registrado_por_id
                FROM eventos e
                LEFT JOIN presencas p ON p.evento_id = e.id AND p.deletado = false
                WHERE e.id = @Id AND e.deletado = false";

            using var conn = CriarConexao();

            var eventoDict = new Dictionary<int, Evento>();

            await conn.QueryAsync<Evento, Presenca, Evento>(
                new CommandDefinition(sql, new { Id = id }, cancellationToken: ct),
                (evento, presenca) =>
                {
                    if (!eventoDict.TryGetValue(evento.Id, out var eventoExistente))
                    {
                        eventoExistente = evento;
                        eventoDict[evento.Id] = eventoExistente;
                    }
                    if (presenca != null)
                        eventoExistente.Presencas.Add(presenca);
                    return eventoExistente;
                },
                splitOn: "id"
            );

            return eventoDict.Values.FirstOrDefault();
        }
    }
}
