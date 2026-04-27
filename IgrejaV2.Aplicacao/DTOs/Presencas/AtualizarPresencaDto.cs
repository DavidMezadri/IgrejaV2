using System.ComponentModel.DataAnnotations;

namespace IgrejaV2.Aplicacao.DTOs.Presencas;

public class AtualizarPresencaDto
{
    public bool Presente { get; set; }

    [MaxLength(500)]
    public string? Observacao { get; set; }
}
