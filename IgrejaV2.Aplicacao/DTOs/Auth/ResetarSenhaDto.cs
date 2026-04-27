using System.ComponentModel.DataAnnotations;

namespace IgrejaV2.Aplicacao.DTOs.Auth;

public class ResetarSenhaDto
{
    [Required]
    public string Token { get; set; } = string.Empty;

    [Required]
    [MinLength(8)]
    public string NovaSenha { get; set; } = string.Empty;

    [Required]
    [Compare(nameof(NovaSenha), ErrorMessage = "As senhas não conferem.")]
    public string ConfirmarNovaSenha { get; set; } = string.Empty;
}
