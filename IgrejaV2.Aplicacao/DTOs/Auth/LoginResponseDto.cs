using IgrejaV2.Aplicacao.DTOs.Usuarios;

namespace IgrejaV2.Aplicacao.DTOs.Auth;

public class LoginResponseDto
{
    public string Token { get; set; } = string.Empty;
    public DateTime Expiracao { get; set; }
    public UsuarioResponseDto Usuario { get; set; } = null!;
}
