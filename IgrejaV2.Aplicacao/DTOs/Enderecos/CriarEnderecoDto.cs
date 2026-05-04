using System.ComponentModel.DataAnnotations;

namespace IgrejaV2.Aplicacao.DTOs.Enderecos;

public class CriarEnderecoDto
{
    [Required]
    [MaxLength(200)]
    public string Rua { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? Complemento { get; set; }

    [Required]
    [MaxLength(20)]
    public string Numero { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Bairro { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Cidade { get; set; } = string.Empty;

    [Required]
    [MaxLength(2)]
    public string Estado { get; set; } = string.Empty;

    [Required]
    [MaxLength(10)]
    [RegularExpression(@"^\d{5}-\d{3}$", ErrorMessage = "CEP deve estar no formato XXXXX-XXX")]
    public string Cep { get; set; } = string.Empty;
}
