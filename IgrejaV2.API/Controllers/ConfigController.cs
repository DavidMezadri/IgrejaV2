using IgrejaV2.Aplicacao.DTOs.Config;
using IgrejaV2.Aplicacao.Servico;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IgrejaV2.API.Controllers;

/// <summary>
/// Gerenciamento de configurações dinâmicas do CMS para páginas públicas.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Tags("Configurações")]
public class ConfigController(IConfigServico servico) : ControllerBase
{
    /// <summary>Obtém todas as configurações.</summary>
    /// <response code="200">Dicionário com todas as configurações.</response>
    /// <response code="500">Erro ao carregar configurações.</response>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObterTodas(CancellationToken ct)
    {
        try
        {
            var configs = await servico.ObterTodasAsync(ct);

            var resultado = new Dictionary<string, string>();
            foreach (var config in configs)
            {
                resultado[config.Chave] = config.Valor;
            }

            return Ok(resultado);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = "Erro ao carregar configurações", erro = ex.Message });
        }
    }

    /// <summary>Atualiza múltiplas configurações.</summary>
    /// <response code="200">Configurações atualizadas com sucesso.</response>
    /// <response code="400">Dados inválidos.</response>
    /// <response code="401">Não autorizado.</response>
    /// <response code="403">Acesso proibido.</response>
    /// <response code="500">Erro ao atualizar configurações.</response>
    [HttpPut]
    [Authorize(Roles = "Administrador")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Atualizar([FromBody] List<ConfigDto> configs, CancellationToken ct)
    {
        if (configs is null || configs.Count == 0)
            return BadRequest(new { mensagem = "Nenhuma configuração para atualizar" });

        try
        {
            await servico.AtualizarAsync(configs, ct);
            return Ok(new { mensagem = "Configurações atualizadas com sucesso" });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensagem = "Erro ao atualizar configurações", erro = ex.Message });
        }
    }
}
