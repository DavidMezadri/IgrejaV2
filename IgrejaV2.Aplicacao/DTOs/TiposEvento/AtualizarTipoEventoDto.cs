using System.ComponentModel.DataAnnotations;
using IgrejaV2.Dominio.Enums;

namespace IgrejaV2.Aplicacao.DTOs.TiposEvento;

public class AtualizarTipoEventoDto
{
    [Required]
    [MaxLength(100)]
    public string Nome { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Descricao { get; set; }

    public PublicoAlvoEnum? PublicoAlvo { get; set; }
    public bool RequerPresenca { get; set; } = true;
    public bool Ativo { get; set; } = true;
}
