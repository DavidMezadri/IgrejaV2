namespace IgrejaV2.Dominio.Entidades;

public class Versiculo : EntidadeBase
{
    public int Livro { get; set; }
    public int Capitulo { get; set; }
    public int Numero { get; set; }
    public string Texto { get; set; } = string.Empty;

    public int TraducaoId { get; set; }
    public Traducao? Traducao { get; set; }
}
