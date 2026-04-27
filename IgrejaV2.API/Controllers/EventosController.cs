using IgrejaV2.Aplicacao.DTOs.Eventos;
using IgrejaV2.Aplicacao.Servico;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IgrejaV2.API.Controllers;

/// <summary>
/// CRUD de eventos da igreja.
/// </summary>
[ApiController]
[Route("api/eventos")]
[Authorize]
[Produces("application/json")]
[Tags("Eventos")]
public class EventosController(EventoServico servico) : ControllerBase
{
    /// <summary>Cria um novo evento.</summary>
    /// <response code="201">Evento criado.</response>
    /// <response code="400">Dados inválidos.</response>
    [HttpPost]
    [ProducesResponseType(typeof(EventoResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Criar([FromBody] CriarEventoDto dto, CancellationToken ct)
    {
        var evento = await servico.CriarAsync(dto, ct);
        return CreatedAtAction(nameof(ObterPorId), new { id = evento.Id }, evento);
    }

    /// <summary>Lista todos os eventos.</summary>
    /// <response code="200">Lista de eventos.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<EventoResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Listar(CancellationToken ct)
    {
        var eventos = await servico.ListarTodosAsync(ct);
        return Ok(eventos);
    }

    /// <summary>Lista apenas os eventos ativos, ordenados pela data de início.</summary>
    /// <response code="200">Lista de eventos ativos.</response>
    [HttpGet("ativos")]
    [ProducesResponseType(typeof(IEnumerable<EventoResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ListarAtivos(CancellationToken ct)
    {
        var eventos = await servico.ListarAtivosAsync(ct);
        return Ok(eventos);
    }

    /// <summary>Obtém um evento pelo ID.</summary>
    /// <param name="id">Identificador único do evento.</param>
    /// <response code="200">Dados do evento.</response>
    /// <response code="404">Evento não encontrado.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(EventoResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorId(int id, CancellationToken ct)
    {
        var evento = await servico.ObterPorIdAsync(id, ct);
        return evento is null ? NotFound() : Ok(evento);
    }

    /// <summary>Atualiza os dados de um evento.</summary>
    /// <param name="id">Identificador único do evento.</param>
    /// <response code="200">Evento atualizado.</response>
    /// <response code="404">Evento não encontrado.</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(EventoResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Atualizar(int id, [FromBody] AtualizarEventoDto dto, CancellationToken ct)
    {
        var evento = await servico.AtualizarAsync(id, dto, ct);
        return evento is null ? NotFound() : Ok(evento);
    }

    /// <summary>Remove um evento.</summary>
    /// <param name="id">Identificador único do evento.</param>
    /// <response code="204">Evento removido com sucesso.</response>
    /// <response code="404">Evento não encontrado.</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Remover(int id, CancellationToken ct)
    {
        var removido = await servico.RemoverAsync(id, ct);
        return removido ? NoContent() : NotFound();
    }
}
