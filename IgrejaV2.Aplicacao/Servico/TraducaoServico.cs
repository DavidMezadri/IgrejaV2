using IgrejaV2.Aplicacao.DTOs.Traducoes;
using IgrejaV2.Dominio.Entidades;
using IgrejaV2.Dominio.Enums;
using IgrejaV2.Dominio.Interfaces;

namespace IgrejaV2.Aplicacao.Servico;

public class TraducaoServico(IRepositorioTraducao repositorio, LogServico logServico)
{
    public async Task<TraducaoResponseDto> CriarAsync(CriarTraducaoDto dto, CancellationToken ct = default)
    {
        var traducao = new Traducao
        {
            Nome = dto.Nome,
            Abreviacao = dto.Abreviacao,
            Descricao = dto.Descricao
        };

        await repositorio.AdicionarAsync(traducao, ct);
        await repositorio.SalvarAlteracoesAsync(ct);

        var traducaoDto = ToDto(traducao);

        await logServico.RegistrarAsync(
            AcaoLogEnum.Criacao,
            nameof(Traducao),
            traducao.Id,
            descricao: $"Tradução criada: {traducao.Nome}",
            dadosNovos: traducaoDto,
            ct: ct);

        return traducaoDto;
    }

    public async Task<TraducaoResponseDto?> ObterPorIdAsync(int id, CancellationToken ct = default)
    {
        var traducao = await repositorio.ObterPorIdAsync(id, ct);
        return traducao is null ? null : ToDto(traducao);
    }

    public async Task<IEnumerable<TraducaoResponseDto>> ListarTodosAsync(CancellationToken ct = default)
    {
        var traducoes = await repositorio.ListarTodosAsync(ct);
        return traducoes.Select(ToDto);
    }

    public async Task<TraducaoResponseDto?> AtualizarAsync(int id, AtualizarTraducaoDto dto, CancellationToken ct = default)
    {
        var traducao = await repositorio.ObterPorIdAsync(id, ct);
        if (traducao is null) return null;

        var traducaoAntes = ToDto(traducao);

        traducao.Nome = dto.Nome;
        traducao.Abreviacao = dto.Abreviacao;
        traducao.Descricao = dto.Descricao;
        traducao.DataAtualizacao = DateTime.UtcNow;

        await repositorio.AtualizarAsync(traducao, ct);
        await repositorio.SalvarAlteracoesAsync(ct);

        var traducaoDepois = ToDto(traducao);

        await logServico.RegistrarAsync(
            AcaoLogEnum.Edicao,
            nameof(Traducao),
            traducao.Id,
            descricao: $"Tradução atualizada: {traducao.Nome}",
            dadosAnteriores: traducaoAntes,
            dadosNovos: traducaoDepois,
            ct: ct);

        return traducaoDepois;
    }

    public async Task<bool> RemoverAsync(int id, CancellationToken ct = default)
    {
        var traducao = await repositorio.ObterPorIdAsync(id, ct);
        if (traducao is null) return false;

        var traducaoDados = ToDto(traducao);

        await repositorio.RemoverPorIdAsync(id, ct);
        await repositorio.SalvarAlteracoesAsync(ct);

        await logServico.RegistrarAsync(
            AcaoLogEnum.Delecao,
            nameof(Traducao),
            id,
            descricao: $"Tradução removida: {traducao.Nome}",
            dadosAnteriores: traducaoDados,
            ct: ct);

        return true;
    }

    private static TraducaoResponseDto ToDto(Traducao t) => new()
    {
        Id = t.Id,
        Nome = t.Nome,
        Abreviacao = t.Abreviacao,
        Descricao = t.Descricao,
        DataCriacao = t.DataCriacao
    };
}
