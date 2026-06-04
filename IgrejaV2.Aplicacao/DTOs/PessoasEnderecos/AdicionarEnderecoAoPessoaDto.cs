using System.ComponentModel.DataAnnotations;

namespace IgrejaV2.Aplicacao.DTOs.PessoasEnderecos;

public class AdicionarEnderecoAoPessoaDto
{
    [Required]
    public int EnderecoId { get; set; }

    public bool Principal { get; set; } = false;
}
