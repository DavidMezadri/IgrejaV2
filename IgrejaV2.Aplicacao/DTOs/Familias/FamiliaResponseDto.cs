namespace IgrejaV2.Aplicacao.DTOs.Familias;

public class FamiliaResponseDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int? ResponsavelId { get; set; }
    public string? ResponsavelNome { get; set; }
    public string? Observacoes { get; set; }
    public bool Ativo { get; set; }
    public int TotalMembros { get; set; }
    public DateTime DataCriacao { get; set; }
}
