using System.ComponentModel.DataAnnotations;

namespace IgrejaV2.Aplicacao.DTOs.Eventos;

public class CriarEventoDto
{
    [Required]
    [MaxLength(200)]
    public string Nome { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Descricao { get; set; }

    [Required]
    public int TipoEventoId { get; set; }

    [Required]
    public DateTime DataInicio { get; set; }

    public DateTime? DataFim { get; set; }

    [MaxLength(300)]
    public string? Local { get; set; }

    public int? CapacidadeMaxima { get; set; }
    public bool RequerInscricao { get; set; } = false;
}
