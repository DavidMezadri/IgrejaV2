namespace IgrejaV2.Aplicacao.DTOs.Versiculos;

public record VericuloResponseDto
{
    public int Id { get; set; }
    public int Livro { get; set; }
    public int Capitulo { get; set; }
    public int Numero { get; set; }
    public string Texto { get; set; } = string.Empty;
    public int TraducaoId { get; set; }
    public string? TraducaoAbreviacao { get; set; }
    public DateTime DataCriacao { get; set; }
}
