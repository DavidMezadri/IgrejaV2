namespace IgrejaV2.Aplicacao.DTOs.Eventos;

public class EventoResponseDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public int TipoEventoId { get; set; }
    public string? TipoEventoNome { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime? DataFim { get; set; }
    public string? Local { get; set; }
    public int? CapacidadeMaxima { get; set; }
    public bool RequerInscricao { get; set; }
    public bool Ativo { get; set; }
    public DateTime DataCriacao { get; set; }
}
