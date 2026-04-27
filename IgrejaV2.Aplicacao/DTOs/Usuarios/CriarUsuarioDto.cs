using System.ComponentModel.DataAnnotations;
using IgrejaV2.Dominio.Enums;

namespace IgrejaV2.Aplicacao.DTOs.Usuarios;

public class CriarUsuarioDto
{
    [Required]
    [MinLength(3), MaxLength(150)]
    public string NomeUsuario { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(200)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(8)]
    public string Senha { get; set; } = string.Empty;

    [Required]
    [Compare(nameof(Senha), ErrorMessage = "As senhas não conferem.")]
    public string ConfirmarSenha { get; set; } = string.Empty;

    [Required]
    public TipoUsuarioEnum TipoUsuario { get; set; }
}
