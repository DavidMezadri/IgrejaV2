using IgrejaV2.Aplicacao.DTOs.Auth;
using IgrejaV2.Aplicacao.DTOs.Usuarios;
using IgrejaV2.Aplicacao.Servico;
using IgrejaV2.API.Servicos;
using Microsoft.AspNetCore.Mvc;

namespace IgrejaV2.API.Controllers;

/// <summary>
/// Endpoints de autenticação: login, recuperação e redefinição de senha.
/// </summary>
[ApiController]
[Route("api/auth")]
[Produces("application/json")]
[Tags("Autenticação")]
public class AutenticacaoController(AuthServico authServico, TokenServico tokenServico) : ControllerBase
{
    /// <summary>Autentica um usuário e retorna um token JWT.</summary>
    /// <remarks>
    /// Use as credenciais cadastradas. O token retornado deve ser enviado no header
    /// <c>Authorization: Bearer {token}</c> em todos os endpoints protegidos.
    ///
    /// **Exemplo de request:**
    /// ```json
    /// { "nomeUsuario": "admin", "senha": "MinhaSenh@123" }
    /// ```
    /// </remarks>
    /// <response code="200">Token JWT gerado com sucesso.</response>
    /// <response code="401">Credenciais inválidas.</response>
    [HttpPost("login")] 
    [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginDto dto, CancellationToken ct)
    {
        var usuario = await authServico.ValidarCredenciaisAsync(dto, ct);
        if (usuario is null)
            return Unauthorized(new { mensagem = "Usuário ou senha inválidos." });

        var (tokenStr, expiracao) = tokenServico.Gerar(usuario);

        return Ok(new LoginResponseDto
        {
            Token = tokenStr,
            Expiracao = expiracao,
            Usuario = new UsuarioResponseDto
            {
                Id = usuario.Id,
                NomeUsuario = usuario.NomeUsuario,
                Email = usuario.Email,
                TipoUsuario = usuario.TipoUsuario,
                PrimeiroAcesso = usuario.PrimeiroAcesso,
                UltimoLogin = usuario.UltimoLogin,
                DataCriacao = usuario.DataCriacao
            }
        });
    }

    /// <summary>Solicita a recuperação de senha por e-mail.</summary>
    /// <remarks>
    /// Gera um token de reset com validade de **2 horas** e o envia para o e-mail informado.
    ///
    /// A resposta é sempre `200` independente de o e-mail existir ou não
    /// (evita enumeração de usuários).
    ///
    /// ⚠️ **Atenção (dev):** o campo `token` é retornado na resposta apenas em ambiente de desenvolvimento.
    /// Em produção, ele deve ser enviado exclusivamente por e-mail.
    /// </remarks>
    /// <response code="200">Solicitação processada (e-mail enviado se cadastrado).</response>
    [HttpPost("recuperar-senha")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> RecuperarSenha([FromBody] RecuperarSenhaDto dto, CancellationToken ct)
    {
        var token = await authServico.GerarTokenRecuperacaoAsync(dto, ct);

        return Ok(new
        {
            mensagem = "Se o e-mail estiver cadastrado, você receberá as instruções para redefinir a senha.",
            token // remover em produção
        });
    }

    /// <summary>Redefine a senha usando o token de recuperação.</summary>
    /// <remarks>
    /// Use o token recebido por e-mail (ou retornado em dev) para definir uma nova senha.
    /// O token é invalidado após o uso ou ao expirar.
    ///
    /// **Exemplo de request:**
    /// ```json
    /// {
    ///   "token": "abc123...",
    ///   "novaSenha": "NovaSenh@456",
    ///   "confirmarNovaSenha": "NovaSenh@456"
    /// }
    /// ```
    /// </remarks>
    /// <response code="204">Senha redefinida com sucesso.</response>
    /// <response code="400">Token inválido ou expirado.</response>
    [HttpPost("resetar-senha")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ResetarSenha([FromBody] ResetarSenhaDto dto, CancellationToken ct)
    {
        var sucesso = await authServico.ResetarSenhaAsync(dto, ct);
        if (!sucesso)
            return BadRequest(new { mensagem = "Token inválido ou expirado." });

        return NoContent();
    }
}
