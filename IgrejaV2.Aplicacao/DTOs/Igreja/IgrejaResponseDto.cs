namespace IgrejaV2.Aplicacao.DTOs.Igreja;

public class IgrejaResponseDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Cnpj { get; set; }
    public string? Telefone { get; set; }
    public string? Email { get; set; }
    public string? Lema { get; set; }
    public int? EnderecoId { get; set; }
    public DateTime? DataFundacao { get; set; }
    public bool Ativa { get; set; }
    public string? Observacoes { get; set; }
}
