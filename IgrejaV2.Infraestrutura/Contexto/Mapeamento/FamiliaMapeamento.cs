using IgrejaV2.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IgrejaV2.Infraestrutura.Contexto.Mapeamento
{
    public class FamiliaMapeamento : IEntityTypeConfiguration<Familia>
    {
        public void Configure(EntityTypeBuilder<Familia> builder)
        {
            builder.ToTable("familias");

            builder.Property(f => f.Nome).HasColumnName("nome").IsRequired().HasMaxLength(200);
            builder.Property(f => f.ResponsavelId).HasColumnName("responsavel_id");
            builder.Property(f => f.Ativo).HasColumnName("ativo").IsRequired().HasDefaultValue(true);
            builder.Property(f => f.Observacoes).HasColumnName("observacoes").HasMaxLength(500);

            builder.HasOne(f => f.Responsavel)
                .WithMany()
                .HasForeignKey(f => f.ResponsavelId)
                .HasConstraintName("fk_familias_pessoas_responsavel_id")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(f => f.Membros)
                .WithOne(p => p.Familia)
                .HasForeignKey(p => p.FamiliaId)
                .HasConstraintName("fk_pessoas_familias_familia_id")
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
