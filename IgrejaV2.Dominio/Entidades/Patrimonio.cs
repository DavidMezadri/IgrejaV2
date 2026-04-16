using IgrejaV2.Dominio.Enums;

namespace IgrejaV2.Dominio.Entidades;

public class Patrimonio : EntidadeBase
{
    public int? IgrejaId { get; set; }
    public Igreja? Igreja { get; set; }

    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public string? NumeroPatrimonio { get; set; }
    public string? Categoria { get; set; }
    public decimal? ValorAquisicao { get; set; }
    public DateTime? DataAquisicao { get; set; }
    public EstadoConservacaoEnum? EstadoConservacao { get; set; }
    
    public bool Ativo { get; set; } = true;
    public string? Observacoes { get; set; }
}
