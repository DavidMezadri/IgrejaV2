using IgrejaV2.Aplicacao.DTOs.Enderecos;
using IgrejaV2.Aplicacao.DTOs.PessoasEnderecos;
using IgrejaV2.Aplicacao.Servico;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IgrejaV2.API.Controllers;

/// <summary>
/// CRUD de endereços e vinculação de pessoas com endereços.
/// </summary>
[ApiController]
[Route("api/enderecos")]
[Authorize]
[Produces("application/json")]
[Tags("Endereços")]
public class EnderecosController(EnderecoServico servicoEndereco, PessoaEnderecoServico servicoPessoaEndereco) : ControllerBase
{
    /// <summary>Cadastra um novo endereço.</summary>
    /// <response code="201">Endereço criado. O header `Location` aponta para o recurso.</response>
    /// <response code="400">Dados inválidos.</response>
    [HttpPost]
    [ProducesResponseType(typeof(EnderecoResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Criar([FromBody] CriarEnderecoDto dto, CancellationToken ct)
    {
        var endereco = await servicoEndereco.CriarAsync(dto, ct);
        return CreatedAtAction(nameof(ObterPorId), new { id = endereco.Id }, endereco);
    }

    /// <summary>Lista todos os endereços.</summary>
    /// <response code="200">Lista de endereços.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<EnderecoResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Listar(CancellationToken ct)
    {
        var enderecos = await servicoEndereco.ListarTodosAsync(ct);
        return Ok(enderecos);
    }

    /// <summary>Obtém um endereço pelo ID.</summary>
    /// <param name="id">Identificador único do endereço.</param>
    /// <response code="200">Dados do endereço.</response>
    /// <response code="404">Endereço não encontrado.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(EnderecoResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorId(int id, CancellationToken ct)
    {
        var endereco = await servicoEndereco.ObterPorIdAsync(id, ct);
        return endereco is null ? NotFound() : Ok(endereco);
    }

    /// <summary>Atualiza os dados de um endereço.</summary>
    /// <param name="id">Identificador único do endereço.</param>
    /// <response code="200">Endereço atualizado.</response>
    /// <response code="404">Endereço não encontrado.</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(EnderecoResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Atualizar(int id, [FromBody] AtualizarEnderecoDto dto, CancellationToken ct)
    {
        var endereco = await servicoEndereco.AtualizarAsync(id, dto, ct);
        return endereco is null ? NotFound() : Ok(endereco);
    }

    /// <summary>Remove um endereço (hard delete).</summary>
    /// <param name="id">Identificador único do endereço.</param>
    /// <response code="204">Endereço removido com sucesso.</response>
    /// <response code="404">Endereço não encontrado.</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Remover(int id, CancellationToken ct)
    {
        var removido = await servicoEndereco.RemoverAsync(id, ct);
        return removido ? NoContent() : NotFound();
    }

    /// <summary>Vincula um endereço a uma pessoa.</summary>
    /// <response code="201">Vinculação criada.</response>
    /// <response code="400">Dados inválidos ou recurso não encontrado.</response>
    [HttpPost("pessoas")]
    [ProducesResponseType(typeof(PessoaEnderecoResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> VincularPessoa([FromBody] CriarPessoaEnderecoDto dto, CancellationToken ct)
    {
        try
        {
            var pessoaEndereco = await servicoPessoaEndereco.VincularAsync(dto, ct);
            return CreatedAtAction(nameof(ObterEnderecosPorPessoa), new { pessoaId = dto.PessoaId }, pessoaEndereco);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    /// <summary>Lista todos os endereços de uma pessoa.</summary>
    /// <param name="pessoaId">Identificador da pessoa.</param>
    /// <response code="200">Lista de endereços da pessoa.</response>
    /// <response code="400">Pessoa não encontrada.</response>
    [HttpGet("pessoas/{pessoaId:int}")]
    [ProducesResponseType(typeof(IEnumerable<PessoaEnderecoResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ObterEnderecosPorPessoa(int pessoaId, CancellationToken ct)
    {
        try
        {
            var enderecos = await servicoPessoaEndereco.ObterEnderecosPorPessoaAsync(pessoaId, ct);
            return Ok(enderecos);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    /// <summary>Remove a vinculação de um endereço com uma pessoa.</summary>
    /// <param name="pessoaEnderecoId">Identificador da vinculação pessoa-endereço.</param>
    /// <response code="204">Vinculação removida com sucesso.</response>
    /// <response code="404">Vinculação não encontrada.</response>
    [HttpDelete("pessoas/{pessoaEnderecoId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DesvincularPessoa(int pessoaEnderecoId, CancellationToken ct)
    {
        var removido = await servicoPessoaEndereco.DesvincularAsync(pessoaEnderecoId, ct);
        return removido ? NoContent() : NotFound();
    }
}
