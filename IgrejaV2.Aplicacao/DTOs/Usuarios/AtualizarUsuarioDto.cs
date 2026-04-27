using IgrejaV2.Dominio.Enums;

namespace IgrejaV2.Aplicacao.DTOs.Usuarios;

public class AtualizarUsuarioDto
{
    public string NomeUsuario { get; set; } = string.Empty;
    public TipoUsuarioEnum TipoUsuario { get; set; }
}
