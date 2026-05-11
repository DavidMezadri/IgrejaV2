using IgrejaV2.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IgrejaV2.Infraestrutura.Contexto.Mapeamento
{
    public class VerisculoMapeamento : IEntityTypeConfiguration<Versiculo>
    {
        public void Configure(EntityTypeBuilder<Versiculo> builder)
        {
            builder.ToTable("versiculos");

            builder.HasKey(v => v.Id);

            builder.Property(v => v.Livro).HasColumnName("livro").IsRequired();

            builder.Property(v => v.Capitulo).HasColumnName("capitulo").IsRequired();

            builder.Property(v => v.Numero).HasColumnName("numero").IsRequired();

            builder.Property(v => v.Texto).HasColumnName("texto").HasMaxLength(5000).IsRequired();

            builder.Property(v => v.TraducaoId).HasColumnName("traducao_id").IsRequired();

            // Índices para melhorar performance nas buscas
            builder.HasIndex(v => new { v.Livro, v.Capitulo, v.Numero, v.TraducaoId }).IsUnique();

            builder.HasIndex(v => new { v.Livro, v.TraducaoId });

            builder.HasIndex(v => new { v.Livro, v.Capitulo, v.TraducaoId });

            // Relacionamentos
            builder.HasOne(v => v.Traducao).WithMany(t => t.Versiculos).HasConstraintName("fk_versiculos_traducao_id").HasForeignKey(v => v.TraducaoId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
