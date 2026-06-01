using IgrejaV2.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IgrejaV2.Infraestrutura.Contexto.Mapeamento;

public class ConfiguracaoMapeamento : IEntityTypeConfiguration<Configuracao>
{
    public void Configure(EntityTypeBuilder<Configuracao> builder)
    {
        builder.ToTable("configuracoes");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasColumnName("id");

        builder.Property(c => c.Chave)
            .HasColumnName("chave")
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Valor)
            .HasColumnName("valor")
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(c => c.CriadoEm)
            .HasColumnName("criado_em")
            .HasDefaultValueSql("NOW()");

        builder.Property(c => c.AtualizadoEm)
            .HasColumnName("atualizado_em")
            .HasDefaultValueSql("NOW()");

        builder.HasIndex(c => c.Chave)
            .IsUnique()
            .HasDatabaseName("idx_configuracoes_chave_unica");
    }
}
