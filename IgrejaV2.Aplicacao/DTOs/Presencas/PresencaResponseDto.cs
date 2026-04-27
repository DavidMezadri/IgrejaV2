namespace IgrejaV2.Aplicacao.DTOs.Presencas;

public class PresencaResponseDto
{
    public int Id { get; set; }
    public int EventoId { get; set; }
    public string? EventoNome { get; set; }
    public int PessoaId { get; set; }
    public string? PessoaNome { get; set; }
    public bool Presente { get; set; }
    public int? RegistradoPorId { get; set; }
    public string? Observacao { get; set; }
    public DateTime DataCriacao { get; set; }
}
