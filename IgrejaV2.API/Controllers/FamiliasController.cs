using IgrejaV2.Aplicacao.DTOs.Familias;
using IgrejaV2.Aplicacao.Servico;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IgrejaV2.API.Controllers;

/// <summary>
/// CRUD de famílias da igreja.
/// </summary>
[ApiController]
[Route("api/familias")]
[Authorize]
[Produces("application/json")]
[Tags("Famílias")]
public class FamiliasController(FamiliaServico servico) : ControllerBase
{
    /// <summary>Cadastra uma nova família.</summary>
    /// <response code="201">Família criada.</response>
    /// <response code="400">Dados inválidos.</response>
    [HttpPost]
    [ProducesResponseType(typeof(FamiliaResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Criar([FromBody] CriarFamiliaDto dto, CancellationToken ct)
    {
        var familia = await servico.CriarAsync(dto, ct);
        return CreatedAtAction(nameof(ObterPorId), new { id = familia.Id }, familia);
    }

    /// <summary>Lista todas as famílias.</summary>
    /// <response code="200">Lista de famílias.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<FamiliaResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Listar(CancellationToken ct)
    {
        var familias = await servico.ListarTodosAsync(ct);
        return Ok(familias);
    }

    /// <summary>Obtém uma família pelo ID, incluindo membros.</summary>
    /// <param name="id">Identificador único da família.</param>
    /// <response code="200">Dados da família.</response>
    /// <response code="404">Família não encontrada.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(FamiliaResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorId(int id, CancellationToken ct)
    {
        var familia = await servico.ObterPorIdAsync(id, ct);
        return familia is null ? NotFound() : Ok(familia);
    }

    /// <summary>Atualiza os dados de uma família.</summary>
    /// <param name="id">Identificador único da família.</param>
    /// <response code="200">Família atualizada.</response>
    /// <response code="404">Família não encontrada.</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(FamiliaResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Atualizar(int id, [FromBody] AtualizarFamiliaDto dto, CancellationToken ct)
    {
        var familia = await servico.AtualizarAsync(id, dto, ct);
        return familia is null ? NotFound() : Ok(familia);
    }

    /// <summary>Remove uma família.</summary>
    /// <param name="id">Identificador único da família.</param>
    /// <response code="204">Família removida com sucesso.</response>
    /// <response code="404">Família não encontrada.</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Remover(int id, CancellationToken ct)
    {
        var removido = await servico.RemoverAsync(id, ct);
        return removido ? NoContent() : NotFound();
    }
}
