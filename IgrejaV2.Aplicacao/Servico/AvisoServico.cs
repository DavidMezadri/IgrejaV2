using IgrejaV2.Aplicacao.DTOs.Avisos;
using IgrejaV2.Dominio.Entidades;
using IgrejaV2.Dominio.Enums;
using IgrejaV2.Dominio.Interfaces;

namespace IgrejaV2.Aplicacao.Servico;

public class AvisoServico(IRepositorioAviso repositorio, LogServico logServico)
{
    public async Task<AvisoResponseDto> CriarAsync(CriarAvisoDto dto, CancellationToken ct = default)
    {
        var aviso = new Aviso
        {
            Titulo = dto.Titulo,
            Resumo = dto.Resumo,
            Conteudo = dto.Conteudo,
            Data = dto.Data,
            Categoria = dto.Categoria,
            Ativo = dto.Ativo
        };

        await repositorio.AdicionarAsync(aviso, ct);
        await repositorio.SalvarAlteracoesAsync(ct);

        var avisoDto = ToDto(aviso);

        await logServico.RegistrarAsync(
            AcaoLogEnum.Criacao,
            nameof(Aviso),
            aviso.Id,
            descricao: $"Aviso criado: {aviso.Titulo} em {aviso.Data:dd/MM/yyyy}",
            dadosNovos: avisoDto,
            ct: ct);

        return avisoDto;
    }

    public async Task<AvisoResponseDto?> ObterPorIdAsync(int id, CancellationToken ct = default)
    {
        var aviso = await repositorio.ObterPorIdAsync(id, ct);
        return aviso is null ? null : ToDto(aviso);
    }

    public async Task<IEnumerable<AvisoResponseDto>> ListarTodosAsync(CancellationToken ct = default)
    {
        var avisos = await repositorio.ListarTodosAsync(ct);
        return avisos.Select(ToDto);
    }

    public async Task<IEnumerable<AvisoResponseDto>> ListarAtivosAsync(CancellationToken ct = default)
    {
        var avisos = await repositorio.ObterAvisosAtivosAsync(ct);
        return avisos.Select(ToDto);
    }

    public async Task<AvisoResponseDto?> AtualizarAsync(int id, AtualizarAvisoDto dto, CancellationToken ct = default)
    {
        var aviso = await repositorio.ObterPorIdAsync(id, ct);
        if (aviso is null) return null;

        var avisoAntes = ToDto(aviso);

        aviso.Titulo = dto.Titulo;
        aviso.Resumo = dto.Resumo;
        aviso.Conteudo = dto.Conteudo;
        aviso.Data = dto.Data;
        aviso.Categoria = dto.Categoria;
        aviso.Ativo = dto.Ativo;
        aviso.DataAtualizacao = DateTime.UtcNow;

        await repositorio.AtualizarAsync(aviso, ct);
        await repositorio.SalvarAlteracoesAsync(ct);

        var avisoDepois = ToDto(aviso);

        await logServico.RegistrarAsync(
            AcaoLogEnum.Edicao,
            nameof(Aviso),
            aviso.Id,
            descricao: $"Aviso atualizado: {aviso.Titulo}",
            dadosAnteriores: avisoAntes,
            dadosNovos: avisoDepois,
            ct: ct);

        return avisoDepois;
    }

    public async Task<bool> RemoverAsync(int id, CancellationToken ct = default)
    {
        var aviso = await repositorio.ObterPorIdAsync(id, ct);
        if (aviso is null) return false;

        var avisoDados = ToDto(aviso);

        await repositorio.RemoverPorIdAsync(id, ct);
        await repositorio.SalvarAlteracoesAsync(ct);

        await logServico.RegistrarAsync(
            AcaoLogEnum.Delecao,
            nameof(Aviso),
            id,
            descricao: $"Aviso removido: {aviso.Titulo}",
            dadosAnteriores: avisoDados,
            ct: ct);

        return true;
    }

    private static AvisoResponseDto ToDto(Aviso a) => new()
    {
        Id = a.Id,
        Titulo = a.Titulo,
        Resumo = a.Resumo,
        Conteudo = a.Conteudo,
        Data = a.Data,
        Categoria = a.Categoria,
        Ativo = a.Ativo,
        DataCriacao = a.DataCriacao,
        DataAtualizacao = a.DataAtualizacao
    };
}
