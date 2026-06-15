using System.ComponentModel.DataAnnotations;

namespace IgrejaV2.Aplicacao.DTOs.Avisos;

public class AtualizarAvisoDto
{
    [Required]
    [MaxLength(200)]
    public string Titulo { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string Resumo { get; set; } = string.Empty;

    [MaxLength(3000)]
    public string? Conteudo { get; set; }

    [Required]
    public DateTime Data { get; set; }

    [MaxLength(100)]
    public string? Categoria { get; set; }

    public bool Ativo { get; set; } = true;
}
