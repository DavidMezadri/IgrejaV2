using IgrejaV2.Dominio.Enums;

namespace IgrejaV2.Dominio.Entidades;

public class Usuario : EntidadeBase
{
    public string NomeUsuario { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
    public TipoUsuarioEnum TipoUsuario { get; set; }
    public bool PrimeiroAcesso { get; set; }
    public DateTime? UltimoLogin { get; set; }
    public string? IpUltimoLogin { get; set; }
    
    public int? PessoaId { get; set; }
    public Pessoa? Pessoa { get; set; }
}
