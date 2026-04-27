using System.ComponentModel.DataAnnotations;

namespace IgrejaV2.Aplicacao.DTOs.Auth;

public class LoginDto
{
    [Required]
    public string NomeUsuario { get; set; } = string.Empty;

    [Required]
    public string Senha { get; set; } = string.Empty;
}
