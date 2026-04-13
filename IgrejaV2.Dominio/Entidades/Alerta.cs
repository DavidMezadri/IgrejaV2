using IgrejaV2.Dominio.Enums;

namespace IgrejaV2.Dominio.Entidades
{
    public class Alerta
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int? FamiliaId { get; set; }
        public TipoAlertaEnum Tipo { get; set; } = TipoAlertaEnum.Ausencia;
        public StatusAlertaEnum Status { get; set; } = StatusAlertaEnum.Pendente;
        public string? Descricao { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        public int? ResolvidoPorId { get; set; }
        public DateTime? DataResolucao { get; set; }

        // Navegação
        public Usuario? Usuario { get; set; }
        public Familia? Familia { get; set; }
        public Usuario? ResolvidoPor
        {
            get; set;
        }
    }
}
