using IgrejaV2.Aplicacao.DTOs.TiposEvento;
using IgrejaV2.Dominio.Entidades;
using IgrejaV2.Dominio.Interfaces;

namespace IgrejaV2.Aplicacao.Servico;

public class TipoEventoServico(IRepositorioTipoEvento repositorio)
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

        return ToDto(tipo);
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

        tipo.Nome = dto.Nome;
        tipo.Descricao = dto.Descricao;
        tipo.PublicoAlvo = dto.PublicoAlvo;
        tipo.RequerPresenca = dto.RequerPresenca;
        tipo.Ativo = dto.Ativo;
        tipo.DataAtualizacao = DateTime.UtcNow;

        await repositorio.AtualizarAsync(tipo, ct);
        await repositorio.SalvarAlteracoesAsync(ct);

        return ToDto(tipo);
    }

    public async Task<bool> RemoverAsync(int id, CancellationToken ct = default)
    {
        var existe = await repositorio.ExisteAsync(t => t.Id == id, ct);
        if (!existe) return false;

        await repositorio.RemoverPorIdAsync(id, ct);
        await repositorio.SalvarAlteracoesAsync(ct);
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
