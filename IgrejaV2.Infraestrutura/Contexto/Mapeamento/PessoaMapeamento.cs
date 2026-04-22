using IgrejaV2.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IgrejaV2.Infraestrutura.Contexto.Mapeamento
{
    public class PessoaMapeamento : IEntityTypeConfiguration<Pessoa>
    {
        public void Configure(EntityTypeBuilder<Pessoa> builder)
        {
            builder.ToTable("pessoas");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Nome).HasColumnName("nome").IsRequired().HasMaxLength(200);
            builder.Property(p => p.DataNascimento).HasColumnName("data_nascimento");
            builder.Property(p => p.Sexo).HasColumnName("sexo");
            builder.Property(p => p.Email).HasColumnName("email").HasMaxLength(100);
            builder.Property(p => p.Telefone).HasColumnName("telefone").HasMaxLength(20);

            builder.Property(p => p.DataBatismo).HasColumnName("data_batismo");
            builder.Property(p => p.MembroDesde).HasColumnName("membro_desde");
            builder.Property(p => p.EstadoCivil).HasColumnName("estado_civil");
            builder.Property(p => p.Observacoes).HasColumnName("observacoes").HasMaxLength(500);


            builder.Property(p => p.Ativo).HasColumnName("ativo").IsRequired();
            builder.HasOne(p => p.Familia).WithMany(f => f.Membros).HasForeignKey(p => p.FamiliaId).OnDelete(DeleteBehavior.SetNull);
        }
    }
}
