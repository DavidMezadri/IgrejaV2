using System.ComponentModel.DataAnnotations;

namespace IgrejaV2.Aplicacao.DTOs.Igreja;

public class AtualizarIgrejaDto
{
    [Required]
    [MaxLength(200)]
    public string Nome { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? Cnpj { get; set; }

    [MaxLength(20)]
    public string? Telefone { get; set; }

    [EmailAddress]
    [MaxLength(200)]
    public string? Email { get; set; }

    [MaxLength(500)]
    public string? Lema { get; set; }

    public int? EnderecoId { get; set; }
    public DateTime? DataFundacao { get; set; }
    public bool Ativa { get; set; } = true;

    [MaxLength(500)]
    public string? Observacoes { get; set; }
}
