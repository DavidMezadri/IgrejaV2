namespace IgrejaV2.Dominio.Entidades;

public class Igreja : EntidadeBase
{
    public string Nome { get; set; } = string.Empty;
    public string? Cnpj { get; set; }
    public string? Telefone { get; set; }
    public string? Email { get; set; }

    public int? EnderecoId { get; set; }
    public Endereco? Endereco { get; set; }

    public int? PastorResponsavelId { get; set; }
    public Pessoa? PastorResponsavel { get; set; }

    public DateTime? DataFundacao { get; set; }
    public bool Ativa { get; set; } = true;
    public string? Observacoes { get; set; }

    // Navegação
    public ICollection<Patrimonio> Patrimonios { get; set; } = new List<Patrimonio>();
    public ICollection<Log> Logs { get; set; } = new List<Log>();
}
