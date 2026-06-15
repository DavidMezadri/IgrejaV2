namespace IgrejaV2.Aplicacao.DTOs.Avisos;

public class AvisoResponseDto
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Resumo { get; set; } = string.Empty;
    public string? Conteudo { get; set; }

    public DateTime Data { get; set; }
    public string? Categoria { get; set; }

    public bool Ativo { get; set; }

    public DateTime DataCriacao { get; set; }
    public DateTime? DataAtualizacao { get; set; }
}
