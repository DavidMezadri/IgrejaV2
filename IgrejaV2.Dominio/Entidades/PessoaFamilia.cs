using IgrejaV2.Dominio.Enums;

namespace IgrejaV2.Dominio.Entidades;

public class PessoaFamilia : EntidadeBase
{
    public int PessoaId { get; set; }
    public Pessoa? Pessoa { get; set; }

    public int FamiliaId { get; set; }
    public Familia? Familia { get; set; }

    public PapelFamiliarEnum? Papel { get; set; }
    
    public DateTime? DataSaida { get; set; }
    public bool Ativo { get; set; } = true;
}
