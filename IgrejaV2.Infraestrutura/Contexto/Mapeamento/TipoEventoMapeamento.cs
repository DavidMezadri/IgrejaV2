using IgrejaV2.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IgrejaV2.Infraestrutura.Contexto.Mapeamento
{
    public class TipoEventoMapeamento : IEntityTypeConfiguration<TipoEvento>
    {
        public void Configure(EntityTypeBuilder<TipoEvento> builder)
        {
            builder.ToTable("tipos_evento");

            builder.Property(t => t.Nome).HasColumnName("nome").IsRequired().HasMaxLength(200);
            builder.Property(t => t.Descricao).HasColumnName("descricao").HasMaxLength(1500);
            builder.Property(t => t.PublicoAlvo).HasColumnName("publico_alvo");
            builder.Property(t => t.RequerPresenca).HasColumnName("requer_presenca").IsRequired().HasDefaultValue(true);
            builder.Property(t => t.Ativo).HasColumnName("ativo").IsRequired().HasDefaultValue(true);

            builder.HasMany(t => t.Eventos)
                .WithOne(e => e.TipoEvento)
                .HasForeignKey(e => e.TipoEventoId)
                .HasConstraintName("fk_eventos_tipos_evento_tipo_evento_id")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
