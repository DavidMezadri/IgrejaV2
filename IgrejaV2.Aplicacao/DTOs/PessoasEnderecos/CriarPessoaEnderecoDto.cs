using System.ComponentModel.DataAnnotations;

namespace IgrejaV2.Aplicacao.DTOs.PessoasEnderecos;

public class CriarPessoaEnderecoDto
{
    [Required]
    public int EnderecoId { get; set; }

    [Required]
    public int PessoaId { get; set; }
}
