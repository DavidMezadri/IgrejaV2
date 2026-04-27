using IgrejaV2.Aplicacao.DTOs.Pessoas;
using IgrejaV2.Aplicacao.Servico;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IgrejaV2.API.Controllers;

/// <summary>
/// CRUD de pessoas (membros e visitantes) da igreja.
/// </summary>
[ApiController]
[Route("api/pessoas")]
[Authorize]
[Produces("application/json")]
[Tags("Pessoas")]
public class PessoasController(PessoaServico servico) : ControllerBase
{
    /// <summary>Cadastra uma nova pessoa.</summary>
    /// <response code="201">Pessoa criada. O header `Location` aponta para o recurso.</response>
    /// <response code="400">Dados inválidos.</response>
    [HttpPost]
    [ProducesResponseType(typeof(PessoaResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Criar([FromBody] CriarPessoaDto dto, CancellationToken ct)
    {
        var pessoa = await servico.CriarAsync(dto, ct);
        return CreatedAtAction(nameof(ObterPorId), new { id = pessoa.Id }, pessoa);
    }

    /// <summary>Lista todas as pessoas.</summary>
    /// <response code="200">Lista de pessoas.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PessoaResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Listar(CancellationToken ct)
    {
        var pessoas = await servico.ListarTodosAsync(ct);
        return Ok(pessoas);
    }

    /// <summary>Lista apenas as pessoas ativas.</summary>
    /// <response code="200">Lista de pessoas ativas.</response>
    [HttpGet("ativos")]
    [ProducesResponseType(typeof(IEnumerable<PessoaResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ListarAtivos(CancellationToken ct)
    {
        var pessoas = await servico.ListarAtivosAsync(ct);
        return Ok(pessoas);
    }

    /// <summary>Lista pessoas de uma família.</summary>
    /// <param name="familiaId">Identificador da família.</param>
    /// <response code="200">Lista de pessoas da família.</response>
    [HttpGet("familia/{familiaId:int}")]
    [ProducesResponseType(typeof(IEnumerable<PessoaResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ListarPorFamilia(int familiaId, CancellationToken ct)
    {
        var pessoas = await servico.ListarPorFamiliaAsync(familiaId, ct);
        return Ok(pessoas);
    }

    /// <summary>Obtém uma pessoa pelo ID.</summary>
    /// <param name="id">Identificador único da pessoa.</param>
    /// <response code="200">Dados da pessoa.</response>
    /// <response code="404">Pessoa não encontrada.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(PessoaResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorId(int id, CancellationToken ct)
    {
        var pessoa = await servico.ObterPorIdAsync(id, ct);
        return pessoa is null ? NotFound() : Ok(pessoa);
    }

    /// <summary>Atualiza os dados de uma pessoa.</summary>
    /// <param name="id">Identificador único da pessoa.</param>
    /// <response code="200">Pessoa atualizada.</response>
    /// <response code="404">Pessoa não encontrada.</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(PessoaResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Atualizar(int id, [FromBody] AtualizarPessoaDto dto, CancellationToken ct)
    {
        var pessoa = await servico.AtualizarAsync(id, dto, ct);
        return pessoa is null ? NotFound() : Ok(pessoa);
    }

    /// <summary>Remove uma pessoa (hard delete).</summary>
    /// <param name="id">Identificador único da pessoa.</param>
    /// <response code="204">Pessoa removida com sucesso.</response>
    /// <response code="404">Pessoa não encontrada.</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Remover(int id, CancellationToken ct)
    {
        var removido = await servico.RemoverAsync(id, ct);
        return removido ? NoContent() : NotFound();
    }
}
