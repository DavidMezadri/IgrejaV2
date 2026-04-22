using IgrejaV2.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IgrejaV2.Infraestrutura.Contexto.Mapeamento
{
    public class PatrimonioMapeamento : IEntityTypeConfiguration<Patrimonio>
    {
        public void Configure(EntityTypeBuilder<Patrimonio> builder)
        {
            builder.ToTable("patrimonios");

            builder.Property(p => p.IgrejaId).HasColumnName("igreja_id");
            builder.Property(p => p.Nome).HasColumnName("nome").IsRequired().HasMaxLength(200);
            builder.Property(p => p.Descricao).HasColumnName("descricao").HasMaxLength(1000);
            builder.Property(p => p.NumeroPatrimonio).HasColumnName("numero_patrimonio").HasMaxLength(100);
            builder.Property(p => p.Categoria).HasColumnName("categoria").HasMaxLength(100);
            // Numeric funciona perfeitamente em Postgres. Decimal pode gerar erros.  
            builder.Property(p => p.ValorAquisicao).HasColumnName("valor_aquisicao").HasColumnType("numeric(18,2)");
            builder.Property(p => p.Observacoes).HasColumnName("observacoes").HasMaxLength(1000);
            
            builder.Property(p => p.DataAquisicao).HasColumnName("data_aquisicao");
            builder.Property(p => p.Ativo).HasColumnName("ativo").IsRequired().HasDefaultValue(true);
            builder.Property(p => p.EstadoConservacao).HasColumnName("estado_conservacao");

            builder.HasOne(p => p.Igreja)
                .WithMany(i => i.Patrimonios)
                .HasForeignKey(p => p.IgrejaId)
                .HasConstraintName("fk_patrimonios_igrejas_igreja_id")
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
