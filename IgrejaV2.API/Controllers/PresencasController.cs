using IgrejaV2.Aplicacao.DTOs.Presencas;
using IgrejaV2.Aplicacao.Servico;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IgrejaV2.API.Controllers;

/// <summary>
/// Registro e consulta de presenças em eventos.
/// </summary>
[ApiController]
[Route("api/presencas")]
[Authorize]
[Produces("application/json")]
[Tags("Presenças")]
public class PresencasController(PresencaServico servico) : ControllerBase
{
    /// <summary>Registra a presença de uma pessoa em um evento.</summary>
    /// <response code="201">Presença registrada.</response>
    /// <response code="400">Dados inválidos.</response>
    /// <response code="409">Presença já registrada para esta pessoa neste evento.</response>
    [HttpPost]
    [ProducesResponseType(typeof(PresencaResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Criar([FromBody] CriarPresencaDto dto, CancellationToken ct)
    {
        try
        {
            var presenca = await servico.CriarAsync(dto, ct);
            return CreatedAtAction(nameof(ObterPorId), new { id = presenca.Id }, presenca);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { mensagem = ex.Message });
        }
    }

    /// <summary>Obtém uma presença pelo ID.</summary>
    /// <param name="id">Identificador único da presença.</param>
    /// <response code="200">Dados da presença.</response>
    /// <response code="404">Presença não encontrada.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(PresencaResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorId(int id, CancellationToken ct)
    {
        var presenca = await servico.ObterPorIdAsync(id, ct);
        return presenca is null ? NotFound() : Ok(presenca);
    }

    /// <summary>Lista presenças de um evento.</summary>
    /// <param name="eventoId">Identificador do evento.</param>
    /// <response code="200">Lista de presenças do evento.</response>
    [HttpGet("evento/{eventoId:int}")]
    [ProducesResponseType(typeof(IEnumerable<PresencaResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ListarPorEvento(int eventoId, CancellationToken ct)
    {
        var presencas = await servico.ListarPorEventoAsync(eventoId, ct);
        return Ok(presencas);
    }

    /// <summary>Lista presenças de uma pessoa.</summary>
    /// <param name="pessoaId">Identificador da pessoa.</param>
    /// <response code="200">Lista de presenças da pessoa.</response>
    [HttpGet("pessoa/{pessoaId:int}")]
    [ProducesResponseType(typeof(IEnumerable<PresencaResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ListarPorPessoa(int pessoaId, CancellationToken ct)
    {
        var presencas = await servico.ListarPorPessoaAsync(pessoaId, ct);
        return Ok(presencas);
    }

    /// <summary>Atualiza o status de presença (presente/ausente) e observação.</summary>
    /// <param name="id">Identificador único da presença.</param>
    /// <response code="200">Presença atualizada.</response>
    /// <response code="404">Presença não encontrada.</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(PresencaResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Atualizar(int id, [FromBody] AtualizarPresencaDto dto, CancellationToken ct)
    {
        var presenca = await servico.AtualizarAsync(id, dto, ct);
        return presenca is null ? NotFound() : Ok(presenca);
    }

    /// <summary>Remove um registro de presença.</summary>
    /// <param name="id">Identificador único da presença.</param>
    /// <response code="204">Presença removida com sucesso.</response>
    /// <response code="404">Presença não encontrada.</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Remover(int id, CancellationToken ct)
    {
        var removido = await servico.RemoverAsync(id, ct);
        return removido ? NoContent() : NotFound();
    }
}
