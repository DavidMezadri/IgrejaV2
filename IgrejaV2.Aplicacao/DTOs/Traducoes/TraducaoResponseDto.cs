namespace IgrejaV2.Aplicacao.DTOs.Traducoes;

public record TraducaoResponseDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Abreviacao { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public DateTime DataCriacao { get; set; }
}
