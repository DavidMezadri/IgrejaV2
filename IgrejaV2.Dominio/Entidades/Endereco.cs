namespace IgrejaV2.Dominio.Entidades;

public class Endereco : EntidadeBase
{
    public string? Rua { get; set; }
    public string? Complemento { get; set; }
    public string? Numero { get; set; }
    public string? Bairro { get; set; }
    public string? Cidade { get; set; }
    public string? Estado { get; set; }
    public string? Cep { get; set; }
    
    // Navegacao
    public ICollection<PessoaEndereco> PessoasEnderecos { get; set; } = new List<PessoaEndereco>();
}
