using System.ComponentModel.DataAnnotations;

namespace IgrejaV2.Aplicacao.DTOs.Presencas;

public class CriarPresencaDto
{
    [Required]
    public int EventoId { get; set; }

    [Required]
    public int PessoaId { get; set; }

    public bool Presente { get; set; } = false;

    public int? RegistradoPorId { get; set; }

    [MaxLength(500)]
    public string? Observacao { get; set; }
}
