namespace IgrejaV2.Aplicacao.DTOs.Enderecos;

public class EnderecoResponseDto
{
    public int Id { get; set; }
    public string Rua { get; set; } = string.Empty;
    public string? Complemento { get; set; }
    public string Numero { get; set; } = string.Empty;
    public string Bairro { get; set; } = string.Empty;
    public string Cidade { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public string Cep { get; set; } = string.Empty;
    public DateTime DataCriacao { get; set; }
}
