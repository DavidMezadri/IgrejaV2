namespace IgrejaV2.Dominio.Entidades;

public class PessoaEndereco : EntidadeBase
{
    public int EnderecoId { get; set; }
    public Endereco? Endereco { get; set; }

    public int PessoaId { get; set; }
    public Pessoa? Pessoa { get; set; }
}
