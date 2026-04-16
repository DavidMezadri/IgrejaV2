using IgrejaV2.Dominio.Enums;

namespace IgrejaV2.Dominio.Entidades;

public class TipoEvento : EntidadeBase
{
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public PublicoAlvoEnum? PublicoAlvo { get; set; }
    public bool RequerPresenca { get; set; } = true;
    public bool Ativo { get; set; } = true;

    // Navegacao
    public ICollection<Evento> Eventos { get; set; } = new List<Evento>();
}
