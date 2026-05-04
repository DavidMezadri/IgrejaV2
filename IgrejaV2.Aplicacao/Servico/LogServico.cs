using System.Text.Json;
using IgrejaV2.Dominio.Entidades;
using IgrejaV2.Dominio.Enums;
using IgrejaV2.Dominio.Interfaces;

namespace IgrejaV2.Aplicacao.Servico;

public class LogServico(IRepositorioLog repositorio)
{
    public async Task RegistrarAsync(
        AcaoLogEnum acao,
        string entidade,
        int entidadeId,
        int? usuarioId = null,
        int? igrejaId = null,
        string? descricao = null,
        object? dadosAnteriores = null,
        object? dadosNovos = null,
        string? ip = null,
        string? userAgent = null,
        CancellationToken ct = default)
    {
        var log = new Log
        {
            Acao = acao,
            Entidade = entidade,
            EntidadeId = entidadeId,
            UsuarioId = usuarioId,
            IgrejaId = igrejaId,
            Descricao = descricao,
            DadosAnteriores = dadosAnteriores != null ? JsonSerializer.Serialize(dadosAnteriores) : null,
            DadosNovos = dadosNovos != null ? JsonSerializer.Serialize(dadosNovos) : null,
            Ip = ip,
            UserAgent = userAgent
        };

        await repositorio.AdicionarAsync(log, ct);
        await repositorio.SalvarAlteracoesAsync(ct);
    }

    public async Task<IEnumerable<Log>> ListarPorEntidadeAsync(string entidade, int entidadeId, CancellationToken ct = default)
    {
        var logs = await repositorio.ListarTodosAsync(ct);
        return logs.Where(l => l.Entidade == entidade && l.EntidadeId == entidadeId);
    }
}
