using IgrejaV2.Aplicacao.DTOs.Familias;
using IgrejaV2.Dominio.Entidades;
using IgrejaV2.Dominio.Interfaces;

namespace IgrejaV2.Aplicacao.Servico;

public class FamiliaServico(IRepositorioFamilia repositorio)
{
    public async Task<FamiliaResponseDto> CriarAsync(CriarFamiliaDto dto, CancellationToken ct = default)
    {
        var familia = new Familia
        {
            Nome = dto.Nome,
            ResponsavelId = dto.ResponsavelId,
            Observacoes = dto.Observacoes,
            Ativo = true
        };

        await repositorio.AdicionarAsync(familia, ct);
        await repositorio.SalvarAlteracoesAsync(ct);

        return ToDto(familia);
    }

    public async Task<FamiliaResponseDto?> ObterPorIdAsync(int id, CancellationToken ct = default)
    {
        var familia = await repositorio.ObterComMembrosAsync(id, ct);
        return familia is null ? null : ToDtoComMembros(familia);
    }

    public async Task<IEnumerable<FamiliaResponseDto>> ListarTodosAsync(CancellationToken ct = default)
    {
        var familias = await repositorio.ListarTodosAsync(ct);
        return familias.Select(ToDto);
    }

    public async Task<FamiliaResponseDto?> AtualizarAsync(int id, AtualizarFamiliaDto dto, CancellationToken ct = default)
    {
        var familia = await repositorio.ObterPorIdAsync(id, ct);
        if (familia is null) return null;

        familia.Nome = dto.Nome;
        familia.ResponsavelId = dto.ResponsavelId;
        familia.Observacoes = dto.Observacoes;
        familia.Ativo = dto.Ativo;
        familia.DataAtualizacao = DateTime.UtcNow;

        await repositorio.AtualizarAsync(familia, ct);
        await repositorio.SalvarAlteracoesAsync(ct);

        return ToDto(familia);
    }

    public async Task<bool> RemoverAsync(int id, CancellationToken ct = default)
    {
        var existe = await repositorio.ExisteAsync(f => f.Id == id, ct);
        if (!existe) return false;

        await repositorio.RemoverPorIdAsync(id, ct);
        await repositorio.SalvarAlteracoesAsync(ct);
        return true;
    }

    private static FamiliaResponseDto ToDto(Familia f) => new()
    {
        Id = f.Id,
        Nome = f.Nome,
        ResponsavelId = f.ResponsavelId,
        ResponsavelNome = f.Responsavel?.Nome,
        Observacoes = f.Observacoes,
        Ativo = f.Ativo,
        TotalMembros = f.Membros.Count,
        DataCriacao = f.DataCriacao
    };

    private static FamiliaResponseDto ToDtoComMembros(Familia f) => new()
    {
        Id = f.Id,
        Nome = f.Nome,
        ResponsavelId = f.ResponsavelId,
        ResponsavelNome = f.Responsavel?.Nome,
        Observacoes = f.Observacoes,
        Ativo = f.Ativo,
        TotalMembros = f.Membros.Count,
        DataCriacao = f.DataCriacao
    };
}
