using System.ComponentModel.DataAnnotations;

namespace IgrejaV2.Aplicacao.DTOs.Familias;

public class AtualizarFamiliaDto
{
    [Required]
    [MaxLength(200)]
    public string Nome { get; set; } = string.Empty;

    public int? ResponsavelId { get; set; }

    [MaxLength(1000)]
    public string? Observacoes { get; set; }

    public bool Ativo { get; set; } = true;
}
