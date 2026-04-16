namespace IgrejaV2.Dominio.Entidades;

public class Evento : EntidadeBase
{
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    
    public int TipoEventoId { get; set; }
    public TipoEvento? TipoEvento { get; set; }

    public Usuario? CriadoPor { get; set; } // O Id agora vem da EntityBase (CriadoPorId)

    public DateTime DataInicio { get; set; }
    public DateTime? DataFim { get; set; }

    public string? Local { get; set; }
    public string? Endereco { get; set; }

    public int? CapacidadeMaxima { get; set; }
    public bool RequerInscricao { get; set; } = false;
    public bool Ativo { get; set; } = true;

    // Navegacao
    public ICollection<Presenca> Presencas { get; set; } = new List<Presenca>();
}
