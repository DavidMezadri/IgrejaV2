using IgrejaV2.Dominio.Enums;

namespace IgrejaV2.Aplicacao.DTOs.TiposEvento;

public class TipoEventoResponseDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public PublicoAlvoEnum? PublicoAlvo { get; set; }
    public bool RequerPresenca { get; set; }
    public bool Ativo { get; set; }
    public DateTime DataCriacao { get; set; }
}
