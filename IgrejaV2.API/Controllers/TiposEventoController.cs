using IgrejaV2.Aplicacao.DTOs.TiposEvento;
using IgrejaV2.Aplicacao.Servico;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IgrejaV2.API.Controllers;

/// <summary>
/// CRUD de tipos de evento.
/// </summary>
[ApiController]
[Route("api/tipos-evento")]
[Authorize]
[Produces("application/json")]
[Tags("Tipos de Evento")]
public class TiposEventoController(TipoEventoServico servico) : ControllerBase
{
    /// <summary>Cria um novo tipo de evento.</summary>
    /// <response code="201">Tipo de evento criado.</response>
    /// <response code="400">Dados inválidos.</response>
    [HttpPost]
    [ProducesResponseType(typeof(TipoEventoResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Criar([FromBody] CriarTipoEventoDto dto, CancellationToken ct)
    {
        var tipo = await servico.CriarAsync(dto, ct);
        return CreatedAtAction(nameof(ObterPorId), new { id = tipo.Id }, tipo);
    }

    /// <summary>Lista todos os tipos de evento.</summary>
    /// <response code="200">Lista de tipos de evento.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TipoEventoResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Listar(CancellationToken ct)
    {
        var tipos = await servico.ListarTodosAsync(ct);
        return Ok(tipos);
    }

    /// <summary>Obtém um tipo de evento pelo ID.</summary>
    /// <param name="id">Identificador único do tipo de evento.</param>
    /// <response code="200">Dados do tipo de evento.</response>
    /// <response code="404">Tipo de evento não encontrado.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(TipoEventoResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorId(int id, CancellationToken ct)
    {
        var tipo = await servico.ObterPorIdAsync(id, ct);
        return tipo is null ? NotFound() : Ok(tipo);
    }

    /// <summary>Atualiza um tipo de evento.</summary>
    /// <param name="id">Identificador único do tipo de evento.</param>
    /// <response code="200">Tipo de evento atualizado.</response>
    /// <response code="404">Tipo de evento não encontrado.</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(TipoEventoResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Atualizar(int id, [FromBody] AtualizarTipoEventoDto dto, CancellationToken ct)
    {
        var tipo = await servico.AtualizarAsync(id, dto, ct);
        return tipo is null ? NotFound() : Ok(tipo);
    }

    /// <summary>Remove um tipo de evento.</summary>
    /// <param name="id">Identificador único do tipo de evento.</param>
    /// <response code="204">Tipo de evento removido com sucesso.</response>
    /// <response code="404">Tipo de evento não encontrado.</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Remover(int id, CancellationToken ct)
    {
        var removido = await servico.RemoverAsync(id, ct);
        return removido ? NoContent() : NotFound();
    }
}
