using IgrejaV2.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IgrejaV2.Infraestrutura.Repositorios
{
    public class RepositorioAlerta : IEntityTypeConfiguration<Alerta>
    {
        public void Configure(EntityTypeBuilder<Alerta> builder)
        {
            builder.ToTable("alertas");
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id).HasColumnName("id").UseIdentityAlwaysColumn();
            builder.Property(a => a.UsuarioId).HasColumnName("usuario_id");
            builder.Property(a => a.FamiliaId).HasColumnName("familia_id");
            builder.Property(a => a.Tipo).HasColumnName("tipo");
            builder.Property(a => a.Status).HasColumnName("status").HasConversion<string>().HasMaxLength(20);
            builder.Property(a => a.Descricao).HasColumnName("descricao").HasMaxLength(500);
            builder.Property(a => a.DataCriacao).HasColumnName("data_criacao");
            builder.Property(a => a.ResolvidoPorId).HasColumnName("resolvido_por");
            builder.Property(a => a.DataResolucao).HasColumnName("data_resolucao");

            builder.HasOne(a => a.Usuario)
                   .WithMany()
                   .HasForeignKey(a => a.UsuarioId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(a => a.Familia)
                   .WithMany()
                   .HasForeignKey(a => a.FamiliaId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(a => a.ResolvidoPor)
                   .WithMany()
                   .HasForeignKey(a => a.ResolvidoPorId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }

    public class ObservacaoConfiguracao : IEntityTypeConfiguration<Observacao>
    {
        public void Configure(EntityTypeBuilder<Observacao> builder)
        {
            builder.ToTable("observacoes");
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id).HasColumnName("id").UseIdentityAlwaysColumn();
            builder.Property(o => o.UsuarioId).HasColumnName("usuario_id");
            builder.Property(o => o.EventoId).HasColumnName("evento_id");
            builder.Property(o => o.Descricao).HasColumnName("descricao").IsRequired().HasMaxLength(1000);
            builder.Property(o => o.CriadoPorId).HasColumnName("criado_por");
            builder.Property(o => o.Data).HasColumnName("data");

            builder.HasOne(o => o.Usuario)
                   .WithMany()
                   .HasForeignKey(o => o.UsuarioId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(o => o.Evento)
                   .WithMany(e => e.Observacoes)
                   .HasForeignKey(o => o.EventoId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(o => o.CriadoPor)
                   .WithMany()
                   .HasForeignKey(o => o.CriadoPorId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
