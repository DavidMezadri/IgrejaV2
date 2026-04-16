using IgrejaV2.Dominio.Enums;

namespace IgrejaV2.Dominio.Entidades;

public class Log : EntidadeBase
{
    public int? IgrejaId { get; set; }
    public Igreja? Igreja { get; set; }

    public int? UsuarioId { get; set; }
    public Usuario? Usuario { get; set; }

    public AcaoLogEnum Acao { get; set; }
    public string? Entidade { get; set; }
    public int? EntidadeId { get; set; }
    public string? Descricao { get; set; }
    
    public string? DadosAnteriores { get; set; } 
    public string? DadosNovos { get; set; }
    
    public string? Ip { get; set; }
    public string? UserAgent { get; set; }
}
