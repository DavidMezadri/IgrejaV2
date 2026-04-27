using System.ComponentModel.DataAnnotations;

namespace IgrejaV2.Aplicacao.DTOs.Auth;

public class RecuperarSenhaDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}
