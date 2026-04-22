using IgrejaV2.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IgrejaV2.Infraestrutura.Contexto.Mapeamento
{
    public class LogMapeamento : IEntityTypeConfiguration<Log>
    {
        public void Configure(EntityTypeBuilder<Log> builder)
        {
            builder.ToTable("logs");

            builder.Property(l => l.IgrejaId).HasColumnName("igreja_id");
            builder.Property(l => l.UsuarioId).HasColumnName("usuario_id");
            builder.Property(l => l.Acao).HasColumnName("acao").IsRequired();
            builder.Property(l => l.Entidade).HasColumnName("entidade").HasMaxLength(150);
            builder.Property(l => l.EntidadeId).HasColumnName("entidade_id");
            builder.Property(l => l.Descricao).HasColumnName("descricao").HasMaxLength(1500);
            builder.Property(l => l.DadosAnteriores).HasColumnName("dados_anteriores");
            builder.Property(l => l.DadosNovos).HasColumnName("dados_novos");
            builder.Property(l => l.Ip).HasColumnName("ip").HasMaxLength(50);
            builder.Property(l => l.UserAgent).HasColumnName("user_agent").HasMaxLength(500);

            builder.HasOne(l => l.Igreja)
                .WithMany()
                .HasForeignKey(l => l.IgrejaId)
                .HasConstraintName("fk_logs_igrejas_igreja_id")
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(l => l.Usuario)
                .WithMany()
                .HasForeignKey(l => l.UsuarioId)
                .HasConstraintName("fk_logs_usuarios_usuario_id")
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
