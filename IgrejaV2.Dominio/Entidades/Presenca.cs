namespace IgrejaV2.Dominio.Entidades;

public class Presenca : EntidadeBase
{
    public int EventoId { get; set; }
    public Evento? Evento { get; set; }

    public int PessoaId { get; set; }
    public Pessoa? Pessoa { get; set; }

    public bool Presente { get; set; } = false;

    public int? RegistradoPorId { get; set; }
    public Usuario? RegistradoPor { get; set; }

    public string? Observacao { get; set; }
}
