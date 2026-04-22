using IgrejaV2.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IgrejaV2.Infraestrutura.Contexto.Mapeamento
{
    public class UsuarioMapeamento : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("usuarios");

            builder.Property(u => u.NomeUsuario).HasColumnName("nome_usuario").IsRequired().HasMaxLength(150);
            builder.Property(u => u.Senha).HasColumnName("senha").IsRequired().HasMaxLength(500); // Senhas hasheadas
            builder.Property(u => u.TipoUsuario).HasColumnName("tipo_usuario").IsRequired();
            builder.Property(u => u.PrimeiroAcesso).HasColumnName("primeiro_acesso").IsRequired().HasDefaultValue(true);
            builder.Property(u => u.UltimoLogin).HasColumnName("ultimo_login");
            builder.Property(u => u.IpUltimoLogin).HasColumnName("ip_ultimo_login").HasMaxLength(50);
            builder.Property(u => u.PessoaId).HasColumnName("pessoa_id");

            builder.HasOne(u => u.Pessoa)
                .WithMany()
                .HasForeignKey(u => u.PessoaId)
                .HasConstraintName("fk_usuarios_pessoas_pessoa_id")
                .OnDelete(DeleteBehavior.SetNull); // Mantem usuário mas sem pessoa amarrada se a pessoa sumir (ou vice versa, depende da politica)
        }
    }
}
