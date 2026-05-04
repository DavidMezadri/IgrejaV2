using IgrejaV2.Aplicacao.DTOs.Enderecos;
using IgrejaV2.Aplicacao.DTOs.Pessoas;

namespace IgrejaV2.Aplicacao.DTOs.PessoasEnderecos;

public class PessoaEnderecoResponseDto
{
    public int Id { get; set; }
    public int EnderecoId { get; set; }
    public int PessoaId { get; set; }
    public EnderecoResponseDto? Endereco { get; set; }
    public PessoaResponseDto? Pessoa { get; set; }
    public DateTime DataCriacao { get; set; }
}
