using IgrejaV2.Aplicacao.DTOs.Usuarios;
using IgrejaV2.Aplicacao.Servico;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IgrejaV2.API.Controllers;

/// <summary>
/// CRUD de usuários do sistema.
/// </summary>
/// <remarks>
/// Os endpoints de listagem, busca, atualização e remoção exigem autenticação JWT.
/// Somente a criação de usuário é pública.
/// </remarks>
[ApiController]
[Route("api/usuarios")]
[Produces("application/json")]
[Tags("Usuários")]
public class UsuariosController(UsuarioServico servico) : ControllerBase
{
    /// <summary>Cria um novo usuário.</summary>
    /// <remarks>
    /// Endpoint público — não requer autenticação.
    ///
    /// A senha é armazenada com hash BCrypt. Nome de usuário e e-mail devem ser únicos.
    ///
    /// **Exemplo de request:**
    /// ```json
    /// {
    ///   "nomeUsuario": "joao.silva",
    ///   "email": "joao@exemplo.com",
    ///   "senha": "MinhaSenh@123",
    ///   "confirmarSenha": "MinhaSenh@123",
    ///   "tipoUsuario": 4
    /// }
    /// ```
    ///
    /// **Tipos de usuário:** `0` Administrador · `1` Pastor · `2` Presidente · `3` Comissão · `4` Membro · `5` Visitante
    /// </remarks>
    /// <response code="201">Usuário criado. O header `Location` aponta para o recurso.</response>
    /// <response code="400">Dados inválidos (validação falhou).</response>
    /// <response code="409">Nome de usuário ou e-mail já cadastrado.</response>
    [HttpPost]
    [ProducesResponseType(typeof(UsuarioResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Criar([FromBody] CriarUsuarioDto dto, CancellationToken ct)
    {
        try
        {
            var usuario = await servico.CriarAsync(dto, ct);
            return CreatedAtAction(nameof(ObterPorId), new { id = usuario.Id }, usuario);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { mensagem = ex.Message });
        }
    }

    /// <summary>Lista todos os usuários.</summary>
    /// <remarks>Requer autenticação JWT. Retorna apenas usuários não deletados.</remarks>
    /// <response code="200">Lista de usuários.</response>
    /// <response code="401">Token ausente ou inválido.</response>
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(IEnumerable<UsuarioResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Listar(CancellationToken ct)
    {
        var usuarios = await servico.ListarTodosAsync(ct);
        return Ok(usuarios);
    }

    /// <summary>Obtém um usuário pelo ID.</summary>
    /// <param name="id">Identificador único do usuário.</param>
    /// <response code="200">Dados do usuário.</response>
    /// <response code="401">Token ausente ou inválido.</response>
    /// <response code="404">Usuário não encontrado.</response>
    [HttpGet("{id:int}")]
    [Authorize]
    [ProducesResponseType(typeof(UsuarioResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorId(int id, CancellationToken ct)
    {
        var usuario = await servico.ObterPorIdAsync(id, ct);
        return usuario is null ? NotFound() : Ok(usuario);
    }

    /// <summary>Atualiza nome de usuário e tipo de um usuário existente.</summary>
    /// <param name="id">Identificador único do usuário.</param>
    /// <response code="200">Usuário atualizado.</response>
    /// <response code="401">Token ausente ou inválido.</response>
    /// <response code="404">Usuário não encontrado.</response>
    [HttpPut("{id:int}")]
    [Authorize]
    [ProducesResponseType(typeof(UsuarioResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Atualizar(int id, [FromBody] AtualizarUsuarioDto dto, CancellationToken ct)
    {
        var usuario = await servico.AtualizarAsync(id, dto, ct);
        return usuario is null ? NotFound() : Ok(usuario);
    }

    /// <summary>Remove um usuário (soft delete).</summary>
    /// <param name="id">Identificador único do usuário.</param>
    /// <response code="204">Usuário removido com sucesso.</response>
    /// <response code="401">Token ausente ou inválido.</response>
    /// <response code="404">Usuário não encontrado.</response>
    [HttpDelete("{id:int}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Remover(int id, CancellationToken ct)
    {
        var removido = await servico.RemoverAsync(id, ct);
        return removido ? NoContent() : NotFound();
    }
}
