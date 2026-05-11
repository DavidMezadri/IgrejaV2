namespace IgrejaV2.Aplicacao.DTOs.Versiculos;

public record AtualizarVericuloDto
{
    public int Livro { get; set; }
    public int Capitulo { get; set; }
    public int Numero { get; set; }
    public string Texto { get; set; } = string.Empty;
    public int TraducaoId { get; set; }
}
