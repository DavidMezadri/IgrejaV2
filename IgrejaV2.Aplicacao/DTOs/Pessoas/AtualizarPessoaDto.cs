using System.ComponentModel.DataAnnotations;
using IgrejaV2.Dominio.Enums;

namespace IgrejaV2.Aplicacao.DTOs.Pessoas;

public class AtualizarPessoaDto
{
    [Required]
    [MaxLength(200)]
    public string Nome { get; set; } = string.Empty;

    public DateTime? DataNascimento { get; set; }
    public SexoEnum? Sexo { get; set; }

    [EmailAddress]
    [MaxLength(200)]
    public string? Email { get; set; }

    [MaxLength(20)]
    public string? Telefone { get; set; }

    public DateTime? DataBatismo { get; set; }
    public DateTime? MembroDesde { get; set; }
    public EstadoCivilEnum? EstadoCivil { get; set; }

    [MaxLength(1000)]
    public string? Observacoes { get; set; }

    public int? FamiliaId { get; set; }
    public bool Ativo { get; set; } = true;
}
