namespace IgrejaV2.Dominio.Entidades;

public class Familia : EntidadeBase
{
    public string Nome { get; set; } = string.Empty;

    public int? ResponsavelId { get; set; }
    public Pessoa? Responsavel { get; set; }

    public bool Ativo { get; set; } = true;
    public string? Observacoes { get; set; }

    // Navegacao
    public ICollection<PessoaFamilia> Membros { get; set; } = new List<PessoaFamilia>();
}
