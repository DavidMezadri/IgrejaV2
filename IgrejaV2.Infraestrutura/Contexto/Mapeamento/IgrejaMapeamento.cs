using IgrejaV2.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IgrejaV2.Infraestrutura.Contexto.Mapeamento
{
    public class IgrejaMapeamento : IEntityTypeConfiguration<Igreja>
    {
        public void Configure(EntityTypeBuilder<Igreja> builder)
        {
            builder.ToTable("igrejas");

            builder.Property(i => i.Nome).HasColumnName("nome").IsRequired().HasMaxLength(200);
            builder.Property(i => i.Cnpj).HasColumnName("cnpj").HasMaxLength(20);
            builder.Property(i => i.Telefone).HasColumnName("telefone").HasMaxLength(20);
            builder.Property(i => i.Email).HasColumnName("email").HasMaxLength(100);
            builder.Property(i => i.Observacoes).HasColumnName("observacoes").HasMaxLength(500);

            builder.Property(i => i.DataFundacao).HasColumnName("data_fundacao");
            builder.Property(i => i.Ativa).HasColumnName("ativa").IsRequired().HasDefaultValue(true);
            
            builder.Property(i => i.EnderecoId).HasColumnName("endereco_id");
            builder.Property(i => i.PastorResponsavelId).HasColumnName("pastor_responsavel_id");

            builder.HasOne(i => i.Endereco)
                .WithMany()
                .HasForeignKey(i => i.EnderecoId)
                .HasConstraintName("fk_igrejas_enderecos_endereco_id")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(i => i.PastorResponsavel)
                .WithMany()
                .HasForeignKey(i => i.PastorResponsavelId)
                .HasConstraintName("fk_igrejas_pessoas_pastor_responsavel_id")
                .OnDelete(DeleteBehavior.Restrict);

            // A constraint reversa de Patrimonio está no proprio PatrimonioMapeamento, 
            // mas podemos declarar também aqui para explicitação
            builder.HasMany(i => i.Patrimonios)
                .WithOne(p => p.Igreja)
                .HasForeignKey(p => p.IgrejaId)
                .HasConstraintName("fk_patrimonios_igrejas_igreja_id")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
