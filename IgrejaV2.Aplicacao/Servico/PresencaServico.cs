using IgrejaV2.Aplicacao.DTOs.Presencas;
using IgrejaV2.Dominio.Entidades;
using IgrejaV2.Dominio.Enums;
using IgrejaV2.Dominio.Interfaces;

namespace IgrejaV2.Aplicacao.Servico;

public class PresencaServico(IRepositorioPresenca repositorio, LogServico logServico)
{
    public async Task<PresencaResponseDto> CriarAsync(CriarPresencaDto dto, CancellationToken ct = default)
    {
        var jaRegistrada = await repositorio.ExisteAsync(
            p => p.EventoId == dto.EventoId && p.PessoaId == dto.PessoaId, ct);
        if (jaRegistrada)
            throw new InvalidOperationException("Presença já registrada para esta pessoa neste evento.");

        var presenca = new Presenca
        {
            EventoId = dto.EventoId,
            PessoaId = dto.PessoaId,
            Presente = dto.Presente,
            RegistradoPorId = dto.RegistradoPorId,
            Observacao = dto.Observacao
        };

        await repositorio.AdicionarAsync(presenca, ct);
        await repositorio.SalvarAlteracoesAsync(ct);

        var presencaDto = ToDto(presenca);

        await logServico.RegistrarAsync(
            AcaoLogEnum.Checkin,
            nameof(Presenca),
            presenca.Id,
            descricao: $"Presença registrada - Evento {presenca.EventoId}, Pessoa {presenca.PessoaId}, Presente: {presenca.Presente}",
            dadosNovos: presencaDto,
            ct: ct);

        return presencaDto;
    }

    public async Task<PresencaResponseDto?> ObterPorIdAsync(int id, CancellationToken ct = default)
    {
        var presenca = await repositorio.ObterPorIdAsync(id, ct);
        return presenca is null ? null : ToDto(presenca);
    }

    public async Task<IEnumerable<PresencaResponseDto>> ListarPorEventoAsync(int eventoId, CancellationToken ct = default)
    {
        var presencas = await repositorio.ObterPorEventoAsync(eventoId, ct);
        return presencas.Select(ToDto);
    }

    public async Task<IEnumerable<PresencaResponseDto>> ListarPorPessoaAsync(int pessoaId, CancellationToken ct = default)
    {
        var presencas = await repositorio.ObterPorPessoaAsync(pessoaId, ct);
        return presencas.Select(ToDto);
    }

    public async Task<PresencaResponseDto?> AtualizarAsync(int id, AtualizarPresencaDto dto, CancellationToken ct = default)
    {
        var presenca = await repositorio.ObterPorIdAsync(id, ct);
        if (presenca is null) return null;

        var presencaAntes = ToDto(presenca);

        presenca.Presente = dto.Presente;
        presenca.Observacao = dto.Observacao;
        presenca.DataAtualizacao = DateTime.UtcNow;

        await repositorio.AtualizarAsync(presenca, ct);
        await repositorio.SalvarAlteracoesAsync(ct);

        var presencaDepois = ToDto(presenca);

        await logServico.RegistrarAsync(
            AcaoLogEnum.Edicao,
            nameof(Presenca),
            presenca.Id,
            descricao: $"Presença atualizada - Evento {presenca.EventoId}, Pessoa {presenca.PessoaId}, Presente: {presenca.Presente}",
            dadosAnteriores: presencaAntes,
            dadosNovos: presencaDepois,
            ct: ct);

        return presencaDepois;
    }

    public async Task<bool> RemoverAsync(int id, CancellationToken ct = default)
    {
        var presenca = await repositorio.ObterPorIdAsync(id, ct);
        if (presenca is null) return false;

        var presencaDados = ToDto(presenca);

        await repositorio.RemoverPorIdAsync(id, ct);
        await repositorio.SalvarAlteracoesAsync(ct);

        await logServico.RegistrarAsync(
            AcaoLogEnum.Delecao,
            nameof(Presenca),
            id,
            descricao: $"Presença removida - Evento {presenca.EventoId}, Pessoa {presenca.PessoaId}",
            dadosAnteriores: presencaDados,
            ct: ct);

        return true;
    }

    private static PresencaResponseDto ToDto(Presenca p) => new()
    {
        Id = p.Id,
        EventoId = p.EventoId,
        EventoNome = p.Evento?.Nome,
        PessoaId = p.PessoaId,
        PessoaNome = p.Pessoa?.Nome,
        Presente = p.Presente,
        RegistradoPorId = p.RegistradoPorId,
        Observacao = p.Observacao,
        DataCriacao = p.DataCriacao
    };
}
