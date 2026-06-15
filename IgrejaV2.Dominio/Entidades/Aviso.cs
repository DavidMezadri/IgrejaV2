namespace IgrejaV2.Dominio.Entidades;

public class Aviso : EntidadeBase
{
    public string Titulo { get; set; } = string.Empty;
    public string Resumo { get; set; } = string.Empty;
    public string? Conteudo { get; set; }

    public DateTime Data { get; set; }
    public string? Categoria { get; set; }

    public bool Ativo { get; set; } = true;
}
