using IgrejaV2.Aplicacao.DTOs.Eventos;
using IgrejaV2.Dominio.Entidades;
using IgrejaV2.Dominio.Interfaces;

namespace IgrejaV2.Aplicacao.Servico;

public class EventoServico(IRepositorioEvento repositorio)
{
    public async Task<EventoResponseDto> CriarAsync(CriarEventoDto dto, CancellationToken ct = default)
    {
        var evento = new Evento
        {
            Nome = dto.Nome,
            Descricao = dto.Descricao,
            TipoEventoId = dto.TipoEventoId,
            DataInicio = dto.DataInicio,
            DataFim = dto.DataFim,
            Local = dto.Local,
            CapacidadeMaxima = dto.CapacidadeMaxima,
            RequerInscricao = dto.RequerInscricao,
            Ativo = true
        };

        await repositorio.AdicionarAsync(evento, ct);
        await repositorio.SalvarAlteracoesAsync(ct);

        return ToDto(evento);
    }

    public async Task<EventoResponseDto?> ObterPorIdAsync(int id, CancellationToken ct = default)
    {
        var evento = await repositorio.ObterPorIdAsync(id, ct);
        return evento is null ? null : ToDto(evento);
    }

    public async Task<IEnumerable<EventoResponseDto>> ListarTodosAsync(CancellationToken ct = default)
    {
        var eventos = await repositorio.ListarTodosAsync(ct);
        return eventos.Select(ToDto);
    }

    public async Task<IEnumerable<EventoResponseDto>> ListarAtivosAsync(CancellationToken ct = default)
    {
        var eventos = await repositorio.ObterEventosAtivosAsync(ct);
        return eventos.Select(ToDto);
    }

    public async Task<EventoResponseDto?> AtualizarAsync(int id, AtualizarEventoDto dto, CancellationToken ct = default)
    {
        var evento = await repositorio.ObterPorIdAsync(id, ct);
        if (evento is null) return null;

        evento.Nome = dto.Nome;
        evento.Descricao = dto.Descricao;
        evento.TipoEventoId = dto.TipoEventoId;
        evento.DataInicio = dto.DataInicio;
        evento.DataFim = dto.DataFim;
        evento.Local = dto.Local;
        evento.CapacidadeMaxima = dto.CapacidadeMaxima;
        evento.RequerInscricao = dto.RequerInscricao;
        evento.Ativo = dto.Ativo;
        evento.DataAtualizacao = DateTime.UtcNow;

        await repositorio.AtualizarAsync(evento, ct);
        await repositorio.SalvarAlteracoesAsync(ct);

        return ToDto(evento);
    }

    public async Task<bool> RemoverAsync(int id, CancellationToken ct = default)
    {
        var existe = await repositorio.ExisteAsync(e => e.Id == id, ct);
        if (!existe) return false;

        await repositorio.RemoverPorIdAsync(id, ct);
        await repositorio.SalvarAlteracoesAsync(ct);
        return true;
    }

    private static EventoResponseDto ToDto(Evento e) => new()
    {
        Id = e.Id,
        Nome = e.Nome,
        Descricao = e.Descricao,
        TipoEventoId = e.TipoEventoId,
        TipoEventoNome = e.TipoEvento?.Nome,
        DataInicio = e.DataInicio,
        DataFim = e.DataFim,
        Local = e.Local,
        CapacidadeMaxima = e.CapacidadeMaxima,
        RequerInscricao = e.RequerInscricao,
        Ativo = e.Ativo,
        DataCriacao = e.DataCriacao
    };
}
