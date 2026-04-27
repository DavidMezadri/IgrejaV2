using IgrejaV2.Dominio.Enums;

namespace IgrejaV2.Aplicacao.DTOs.Usuarios;

public class UsuarioResponseDto
{
    public int Id { get; set; }
    public string NomeUsuario { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public TipoUsuarioEnum TipoUsuario { get; set; }
    public bool PrimeiroAcesso { get; set; }
    public DateTime? UltimoLogin { get; set; }
    public DateTime DataCriacao { get; set; }
}
