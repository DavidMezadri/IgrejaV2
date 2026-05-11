using IgrejaV2.Aplicacao.DTOs.Traducoes;
using IgrejaV2.Aplicacao.Servico;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IgrejaV2.API.Controllers;

/// <summary>
/// CRUD de traduções da bíblia.
/// </summary>
[ApiController]
[Route("api/traducoes")]
[Authorize]
[Produces("application/json")]
[Tags("Traduções")]
public class TraducoesController(TraducaoServico servico) : ControllerBase
{
    /// <summary>Cria uma nova tradução.</summary>
    /// <response code="201">Tradução criada.</response>
    /// <response code="400">Dados inválidos.</response>
    [HttpPost]
    [ProducesResponseType(typeof(TraducaoResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Criar([FromBody] CriarTraducaoDto dto, CancellationToken ct)
    {
        var traducao = await servico.CriarAsync(dto, ct);
        return CreatedAtAction(nameof(ObterPorId), new { id = traducao.Id }, traducao);
    }

    /// <summary>Lista todas as traduções.</summary>
    /// <response code="200">Lista de traduções.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TraducaoResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Listar(CancellationToken ct)
    {
        var traducoes = await servico.ListarTodosAsync(ct);
        return Ok(traducoes);
    }

    /// <summary>Obtém uma tradução pelo ID.</summary>
    /// <param name="id">Identificador único da tradução.</param>
    /// <response code="200">Dados da tradução.</response>
    /// <response code="404">Tradução não encontrada.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(TraducaoResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorId(int id, CancellationToken ct)
    {
        var traducao = await servico.ObterPorIdAsync(id, ct);
        return traducao is null ? NotFound() : Ok(traducao);
    }

    /// <summary>Atualiza uma tradução.</summary>
    /// <param name="id">Identificador único da tradução.</param>
    /// <response code="200">Tradução atualizada.</response>
    /// <response code="404">Tradução não encontrada.</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(TraducaoResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Atualizar(int id, [FromBody] AtualizarTraducaoDto dto, CancellationToken ct)
    {
        var traducao = await servico.AtualizarAsync(id, dto, ct);
        return traducao is null ? NotFound() : Ok(traducao);
    }

    /// <summary>Remove uma tradução.</summary>
    /// <param name="id">Identificador único da tradução.</param>
    /// <response code="204">Tradução removida com sucesso.</response>
    /// <response code="404">Tradução não encontrada.</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Remover(int id, CancellationToken ct)
    {
        var removido = await servico.RemoverAsync(id, ct);
        return removido ? NoContent() : NotFound();
    }
}
