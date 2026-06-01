using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IgrejaV2.API.Controllers;

/// <summary>
/// Gerenciamento de upload de imagens para o CMS.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Tags("Upload")]
public class UploadController(IWebHostEnvironment webHost) : ControllerBase
{
    private readonly string UPLOAD_PATH = "wwwroot/uploads/images";

    /// <summary>Faz upload de imagem e retorna URL.</summary>
    /// <param name="arquivo">Arquivo de imagem a enviar.</param>
    /// <response code="200">Upload realizado com sucesso.</response>
    /// <response code="400">Arquivo inválido ou muito grande.</response>
    /// <response code="401">Não autenticado.</response>
    /// <response code="403">Acesso proibido.</response>
    /// <response code="500">Erro ao fazer upload.</response>
    [HttpPost("imagem")]
    [Authorize(Roles = "Administrador")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UploadImagem(IFormFile? arquivo)
    {
        if (arquivo is null || arquivo.Length == 0)
            return BadRequest(new { mensagem = "Nenhum arquivo enviado" });

        var extensoesPermitidas = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
        var extensao = Path.GetExtension(arquivo.FileName).ToLower();

        if (!extensoesPermitidas.Contains(extensao))
            return BadRequest(new { mensagem = "Formato de imagem inválido. Aceito: jpg, png, gif, webp" });

        if (arquivo.Length > 5 * 1024 * 1024)
            return BadRequest(new { mensagem = "Arquivo muito grande. Máximo 5MB" });

        try
        {
            var uploadDir = Path.Combine(webHost.ContentRootPath, UPLOAD_PATH);
            if (!Directory.Exists(uploadDir))
                Directory.CreateDirectory(uploadDir);

            var nomeArquivo = $"{Guid.NewGuid()}{extensao}";
            var caminhoCompleto = Path.Combine(uploadDir, nomeArquivo);

            using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
            {
                await arquivo.CopyToAsync(stream);
            }

            var urlRelativa = $"/uploads/images/{nomeArquivo}";
            return Ok(new
            {
                url = urlRelativa,
                nomeArquivo = nomeArquivo,
                mensagem = "Upload realizado com sucesso"
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                mensagem = "Erro ao fazer upload",
                erro = ex.Message
            });
        }
    }

    /// <summary>Lista todas as imagens disponíveis no servidor.</summary>
    /// <response code="200">Lista de imagens retornada com sucesso.</response>
    [HttpGet("imagens")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult ListarImagens()
    {
        var uploadDir = Path.Combine(webHost.ContentRootPath, UPLOAD_PATH);
        if (!Directory.Exists(uploadDir))
            return Ok(new { imagens = Array.Empty<object>() });

        var extensoesPermitidas = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

        var imagens = Directory.GetFiles(uploadDir)
            .Where(f => extensoesPermitidas.Contains(Path.GetExtension(f).ToLower()))
            .Select(f =>
            {
                var nome = Path.GetFileName(f);
                return new { nomeArquivo = nome, url = $"/uploads/images/{nome}" };
            })
            .ToArray();

        return Ok(new { imagens });
    }

    /// <summary>Remove imagem do servidor.</summary>
    /// <param name="nomeArquivo">Nome do arquivo a deletar.</param>
    /// <response code="200">Imagem deletada com sucesso.</response>
    /// <response code="400">Nome do arquivo inválido.</response>
    /// <response code="401">Não autenticado.</response>
    /// <response code="403">Acesso proibido.</response>
    /// <response code="404">Arquivo não encontrado.</response>
    /// <response code="500">Erro ao deletar imagem.</response>
    [HttpDelete("imagem/{nomeArquivo}")]
    [Authorize(Roles = "Administrador")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult DeleteImagem(string nomeArquivo)
    {
        if (string.IsNullOrWhiteSpace(nomeArquivo))
            return BadRequest(new { mensagem = "Nome do arquivo não fornecido" });

        try
        {
            var caminhoCompleto = Path.Combine(webHost.ContentRootPath, UPLOAD_PATH, nomeArquivo);

            // Segurança: verificar se está dentro do diretório permitido
            var uploadDirFull = Path.Combine(webHost.ContentRootPath, UPLOAD_PATH);
            if (!Path.GetFullPath(caminhoCompleto).StartsWith(Path.GetFullPath(uploadDirFull)))
                return BadRequest(new { mensagem = "Caminho inválido" });

            if (System.IO.File.Exists(caminhoCompleto))
            {
                System.IO.File.Delete(caminhoCompleto);
                return Ok(new { mensagem = "Imagem deletada com sucesso" });
            }

            return NotFound(new { mensagem = "Arquivo não encontrado" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                mensagem = "Erro ao deletar imagem",
                erro = ex.Message
            });
        }
    }
}
