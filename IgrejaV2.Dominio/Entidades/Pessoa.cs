using IgrejaV2.Dominio.Enums;

namespace IgrejaV2.Dominio.Entidades;

public class Pessoa : EntidadeBase
{
    public bool Ativo { get; set; } = true;
    public string Nome { get; set; } = string.Empty;
    public DateTime? DataNascimento { get; set; }
    public SexoEnum? Sexo { get; set; }
    public string? Email { get; set; }
    public string? Telefone { get; set; }

    // Igreja
    public DateTime? DataBatismo { get; set; }
    public DateTime? MembroDesde { get; set; }
    public EstadoCivilEnum? EstadoCivil { get; set; }
    public string? Observacoes { get; set; }

    // Navegacao
    public ICollection<PessoaEndereco> Enderecos { get; set; } = new List<PessoaEndereco>();
    public ICollection<Presenca> Presencas { get; set; } = new List<Presenca>();
    public ICollection<PessoaFamilia> Familias { get; set; } = new List<PessoaFamilia>();
}
