using IgrejaV2.Aplicacao.DTOs.Avisos;
using IgrejaV2.Aplicacao.Servico;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IgrejaV2.API.Controllers;

/// <summary>
/// CRUD de avisos da congregação.
/// </summary>
[ApiController]
[Route("api/avisos")]
[Produces("application/json")]
[Tags("Avisos")]
public class AvisosController(AvisoServico servico) : ControllerBase
{
    /// <summary>Cria um novo aviso.</summary>
    /// <response code="201">Aviso criado.</response>
    /// <response code="400">Dados inválidos.</response>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(AvisoResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Criar([FromBody] CriarAvisoDto dto, CancellationToken ct)
    {
        var aviso = await servico.CriarAsync(dto, ct);
        return CreatedAtAction(nameof(ObterPorId), new { id = aviso.Id }, aviso);
    }

    /// <summary>Lista todos os avisos.</summary>
    /// <response code="200">Lista de avisos.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<AvisoResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Listar(CancellationToken ct)
    {
        var avisos = await servico.ListarTodosAsync(ct);
        return Ok(avisos);
    }

    /// <summary>Lista apenas os avisos ativos, ordenados pela data.</summary>
    /// <response code="200">Lista de avisos ativos.</response>
    [HttpGet("ativos")]
    [ProducesResponseType(typeof(IEnumerable<AvisoResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ListarAtivos(CancellationToken ct)
    {
        var avisos = await servico.ListarAtivosAsync(ct);
        return Ok(avisos);
    }

    /// <summary>Obtém um aviso pelo ID.</summary>
    /// <param name="id">Identificador único do aviso.</param>
    /// <response code="200">Dados do aviso.</response>
    /// <response code="404">Aviso não encontrado.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(AvisoResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorId(int id, CancellationToken ct)
    {
        var aviso = await servico.ObterPorIdAsync(id, ct);
        return aviso is null ? NotFound() : Ok(aviso);
    }

    /// <summary>Atualiza os dados de um aviso.</summary>
    /// <param name="id">Identificador único do aviso.</param>
    /// <response code="200">Aviso atualizado.</response>
    /// <response code="404">Aviso não encontrado.</response>
    [HttpPut("{id:int}")]
    [Authorize]
    [ProducesResponseType(typeof(AvisoResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Atualizar(int id, [FromBody] AtualizarAvisoDto dto, CancellationToken ct)
    {
        var aviso = await servico.AtualizarAsync(id, dto, ct);
        return aviso is null ? NotFound() : Ok(aviso);
    }

    /// <summary>Remove um aviso.</summary>
    /// <param name="id">Identificador único do aviso.</param>
    /// <response code="204">Aviso removido com sucesso.</response>
    /// <response code="404">Aviso não encontrado.</response>
    [HttpDelete("{id:int}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Remover(int id, CancellationToken ct)
    {
        var removido = await servico.RemoverAsync(id, ct);
        return removido ? NoContent() : NotFound();
    }
}
