namespace IgrejaV2.Aplicacao.DTOs.Traducoes;

public record CriarTraducaoDto
{
    public string Nome { get; set; } = string.Empty;
    public string Abreviacao { get; set; } = string.Empty;
    public string? Descricao { get; set; }
}
