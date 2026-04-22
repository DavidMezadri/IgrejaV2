using IgrejaV2.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IgrejaV2.Infraestrutura.Contexto.Mapeamento
{
    public class EnderecoMapeamento : IEntityTypeConfiguration<Endereco>
    {
        public void Configure(EntityTypeBuilder<Endereco> builder)
        {
            builder.ToTable("enderecos");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Rua).HasColumnName("rua").HasMaxLength(250);

            builder.Property(e => e.Numero).HasColumnName("numero").HasMaxLength(50);

            builder.Property(e => e.Complemento).HasColumnName("complemento").HasMaxLength(150);

            builder.Property(e => e.Bairro).HasColumnName("bairro").HasMaxLength(150);

            builder.Property(e => e.Cidade).HasColumnName("cidade").HasMaxLength(150);

            builder.Property(e => e.Estado).HasColumnName("estado").HasMaxLength(2);

            builder.Property(e => e.Cep).HasColumnName("cep").HasMaxLength(20);

            // Relacionamentos
            builder.HasMany(e => e.PessoasEnderecos).WithOne(pe => pe.Endereco).HasConstraintName("fk_pessoas_enderecos_endereco_id").HasForeignKey(pe => pe.EnderecoId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
