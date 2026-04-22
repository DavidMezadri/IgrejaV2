using IgrejaV2.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IgrejaV2.Infraestrutura.Contexto.Mapeamento
{
    public class PessoaEnderecoMapeamento : IEntityTypeConfiguration<PessoaEndereco>
    {
        public void Configure(EntityTypeBuilder<PessoaEndereco> builder)
        {
            builder.ToTable("pessoas_enderecos");

            builder.Property(pe => pe.PessoaId).HasColumnName("pessoa_id").IsRequired();
            builder.Property(pe => pe.EnderecoId).HasColumnName("endereco_id").IsRequired();

            builder.HasOne(pe => pe.Pessoa)
                .WithMany(p => p.Enderecos)
                .HasForeignKey(pe => pe.PessoaId)
                .HasConstraintName("fk_pessoas_enderecos_pessoa_id")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(pe => pe.Endereco)
                .WithMany(e => e.PessoasEnderecos)
                .HasForeignKey(pe => pe.EnderecoId)
                .HasConstraintName("fk_pessoas_enderecos_endereco_id")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
