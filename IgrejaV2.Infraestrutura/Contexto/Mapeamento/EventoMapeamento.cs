using IgrejaV2.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IgrejaV2.Infraestrutura.Contexto.Mapeamento
{
    public class EventoMapeamento : IEntityTypeConfiguration<Evento>
    {
        public void Configure(EntityTypeBuilder<Evento> builder)
        {
            builder.ToTable("eventos");

            builder.Property(e => e.Nome).HasColumnName("nome").IsRequired().HasMaxLength(200);
            builder.Property(e => e.Descricao).HasColumnName("descricao").HasMaxLength(1000);
            builder.Property(e => e.Local).HasColumnName("local").HasMaxLength(200);

            builder.Property(e => e.TipoEventoId).HasColumnName("tipo_evento_id").IsRequired();
            builder.Property(e => e.DataInicio).HasColumnName("data_inicio").IsRequired();
            builder.Property(e => e.DataFim).HasColumnName("data_fim");

            builder.Property(e => e.CapacidadeMaxima).HasColumnName("capacidade_maxima");
            builder.Property(e => e.RequerInscricao).HasColumnName("requer_inscricao").IsRequired().HasDefaultValue(false);
            builder.Property(e => e.Ativo).HasColumnName("ativo").IsRequired().HasDefaultValue(true);

            // Relacionamento com sombra (shadow property) dado que a entidade não os explicitou como int
            builder.HasOne(e => e.Endereco)
                .WithMany()
                .HasForeignKey("endereco_id")
                .HasConstraintName("fk_eventos_enderecos_endereco_id")
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(e => e.TipoEvento)
                .WithMany(t => t.Eventos)
                .HasForeignKey(e => e.TipoEventoId)
                .HasConstraintName("fk_eventos_tipos_evento_tipo_evento_id")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
