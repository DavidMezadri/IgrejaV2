namespace IgrejaV2.Dominio.Entidades;

public class Traducao : EntidadeBase
{
    public string Nome { get; set; } = string.Empty;
    public string Abreviacao { get; set; } = string.Empty;
    public string? Descricao { get; set; }

    public ICollection<Versiculo> Versiculos { get; set; } = [];
}
