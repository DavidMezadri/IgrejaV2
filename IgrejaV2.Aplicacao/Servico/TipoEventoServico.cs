using IgrejaV2.Aplicacao.DTOs.TiposEvento;
using IgrejaV2.Dominio.Entidades;
using IgrejaV2.Dominio.Enums;
using IgrejaV2.Dominio.Interfaces;

namespace IgrejaV2.Aplicacao.Servico;

public class TipoEventoServico(IRepositorioTipoEvento repositorio, LogServico logServico)
{
    public async Task<TipoEventoResponseDto> CriarAsync(CriarTipoEventoDto dto, CancellationToken ct = default)
    {
        var tipo = new TipoEvento
        {
            Nome = dto.Nome,
            Descricao = dto.Descricao,
            PublicoAlvo = dto.PublicoAlvo,
            RequerPresenca = dto.RequerPresenca,
            Ativo = true
        };

        await repositorio.AdicionarAsync(tipo, ct);
        await repositorio.SalvarAlteracoesAsync(ct);

        var tipoDto = ToDto(tipo);

        await logServico.RegistrarAsync(
            AcaoLogEnum.Criacao,
            nameof(TipoEvento),
            tipo.Id,
            descricao: $"Tipo de evento criado: {tipo.Nome}",
            dadosNovos: tipoDto,
            ct: ct);

        return tipoDto;
    }

    public async Task<TipoEventoResponseDto?> ObterPorIdAsync(int id, CancellationToken ct = default)
    {
        var tipo = await repositorio.ObterPorIdAsync(id, ct);
        return tipo is null ? null : ToDto(tipo);
    }

    public async Task<IEnumerable<TipoEventoResponseDto>> ListarTodosAsync(CancellationToken ct = default)
    {
        var tipos = await repositorio.ListarTodosAsync(ct);
        return tipos.Select(ToDto);
    }

    public async Task<TipoEventoResponseDto?> AtualizarAsync(int id, AtualizarTipoEventoDto dto, CancellationToken ct = default)
    {
        var tipo = await repositorio.ObterPorIdAsync(id, ct);
        if (tipo is null) return null;

        var tipoAntes = ToDto(tipo);

        tipo.Nome = dto.Nome;
        tipo.Descricao = dto.Descricao;
        tipo.PublicoAlvo = dto.PublicoAlvo;
        tipo.RequerPresenca = dto.RequerPresenca;
        tipo.Ativo = dto.Ativo;
        tipo.DataAtualizacao = DateTime.UtcNow;

        await repositorio.AtualizarAsync(tipo, ct);
        await repositorio.SalvarAlteracoesAsync(ct);

        var tipoDepois = ToDto(tipo);

        await logServico.RegistrarAsync(
            AcaoLogEnum.Edicao,
            nameof(TipoEvento),
            tipo.Id,
            descricao: $"Tipo de evento atualizado: {tipo.Nome}",
            dadosAnteriores: tipoAntes,
            dadosNovos: tipoDepois,
            ct: ct);

        return tipoDepois;
    }

    public async Task<bool> RemoverAsync(int id, CancellationToken ct = default)
    {
        var tipo = await repositorio.ObterPorIdAsync(id, ct);
        if (tipo is null) return false;

        var tipoDados = ToDto(tipo);

        await repositorio.RemoverPorIdAsync(id, ct);
        await repositorio.SalvarAlteracoesAsync(ct);

        await logServico.RegistrarAsync(
            AcaoLogEnum.Delecao,
            nameof(TipoEvento),
            id,
            descricao: $"Tipo de evento removido: {tipo.Nome}",
            dadosAnteriores: tipoDados,
            ct: ct);

        return true;
    }

    private static TipoEventoResponseDto ToDto(TipoEvento t) => new()
    {
        Id = t.Id,
        Nome = t.Nome,
        Descricao = t.Descricao,
        PublicoAlvo = t.PublicoAlvo,
        RequerPresenca = t.RequerPresenca,
        Ativo = t.Ativo,
        DataCriacao = t.DataCriacao
    };
}
