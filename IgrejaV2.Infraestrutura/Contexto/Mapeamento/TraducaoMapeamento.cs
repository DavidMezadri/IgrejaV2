using IgrejaV2.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IgrejaV2.Infraestrutura.Contexto.Mapeamento
{
    public class TraducaoMapeamento : IEntityTypeConfiguration<Traducao>
    {
        public void Configure(EntityTypeBuilder<Traducao> builder)
        {
            builder.ToTable("traducoes");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Nome).HasColumnName("nome").HasMaxLength(100).IsRequired();

            builder.Property(t => t.Abreviacao).HasColumnName("abreviacao").HasMaxLength(10).IsRequired();

            builder.Property(t => t.Descricao).HasColumnName("descricao").HasMaxLength(500);

            // Relacionamentos
            builder.HasMany(t => t.Versiculos).WithOne(v => v.Traducao).HasConstraintName("fk_versiculos_traducao_id").HasForeignKey(v => v.TraducaoId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
