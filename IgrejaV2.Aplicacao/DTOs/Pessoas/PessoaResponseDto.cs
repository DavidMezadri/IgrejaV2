using IgrejaV2.Dominio.Enums;

namespace IgrejaV2.Aplicacao.DTOs.Pessoas;

public class PessoaResponseDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public DateTime? DataNascimento { get; set; }
    public SexoEnum? Sexo { get; set; }
    public string? Email { get; set; }
    public string? Telefone { get; set; }
    public DateTime? DataBatismo { get; set; }
    public DateTime? MembroDesde { get; set; }
    public EstadoCivilEnum? EstadoCivil { get; set; }
    public string? Observacoes { get; set; }
    public int? FamiliaId { get; set; }
    public bool Ativo { get; set; }
    public DateTime DataCriacao { get; set; }
}
