using IgrejaV2.Aplicacao.DTOs.Igreja;
using IgrejaV2.Aplicacao.Servico;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IgrejaV2.API.Controllers;

/// <summary>
/// CRUD de igrejas.
/// </summary>
[ApiController]
[Route("api/igrejas")]
[Produces("application/json")]
[Tags("Igrejas")]
public class IgrejasController(IgrejaServico servico) : ControllerBase
{
    /// <summary>Cria uma nova igreja.</summary>
    /// <response code="201">Igreja criada.</response>
    /// <response code="400">Dados inválidos.</response>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(IgrejaResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Criar([FromBody] CriarIgrejaDto dto, CancellationToken ct)
    {
        var igreja = await servico.CriarAsync(dto, ct);
        return CreatedAtAction(nameof(ObterPorId), new { id = igreja.Id }, igreja);
    }

    /// <summary>Lista todas as igrejas.</summary>
    /// <response code="200">Lista de igrejas.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<IgrejaResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Listar(CancellationToken ct)
    {
        var igrejas = await servico.ListarTodosAsync(ct);
        return Ok(igrejas);
    }

    /// <summary>Obtém uma igreja pelo ID.</summary>
    /// <param name="id">Identificador único da igreja.</param>
    /// <response code="200">Dados da igreja.</response>
    /// <response code="404">Igreja não encontrada.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(IgrejaResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorId(int id, CancellationToken ct)
    {
        var igreja = await servico.ObterPorIdAsync(id, ct);
        return igreja is null ? NotFound() : Ok(igreja);
    }

    /// <summary>Atualiza uma igreja.</summary>
    /// <param name="id">Identificador único da igreja.</param>
    /// <response code="200">Igreja atualizada.</response>
    /// <response code="404">Igreja não encontrada.</response>
    [HttpPut("{id:int}")]
    [Authorize]
    [ProducesResponseType(typeof(IgrejaResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Atualizar(int id, [FromBody] AtualizarIgrejaDto dto, CancellationToken ct)
    {
        var igreja = await servico.AtualizarAsync(id, dto, ct);
        return igreja is null ? NotFound() : Ok(igreja);
    }

    /// <summary>Remove uma igreja.</summary>
    /// <param name="id">Identificador único da igreja.</param>
    /// <response code="204">Igreja removida com sucesso.</response>
    /// <response code="404">Igreja não encontrada.</response>
    [HttpDelete("{id:int}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Remover(int id, CancellationToken ct)
    {
        var removida = await servico.RemoverAsync(id, ct);
        return removida ? NoContent() : NotFound();
    }
}
