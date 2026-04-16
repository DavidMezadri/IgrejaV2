namespace IgrejaV2.Dominio.Entidades;

public abstract class EntidadeBase
{
    public int Id { get; set; }
    
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    public DateTime? DataAtualizacao { get; set; }
    
    public int? CriadoPorId { get; set; } 
    public int? AtualizadoPorId { get; set; }
    
    public bool Deletado { get; set; } = false;
    public DateTime? DataDelecao { get; set; }
    public int? DeletadoPorId { get; set; }
}
