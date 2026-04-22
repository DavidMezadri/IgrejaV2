using IgrejaV2.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IgrejaV2.Infraestrutura.Contexto.Mapeamento
{
    public class PresencaMapeamento : IEntityTypeConfiguration<Presenca>
    {
        public void Configure(EntityTypeBuilder<Presenca> builder)
        {
            builder.ToTable("presencas");

            builder.Property(p => p.EventoId).HasColumnName("evento_id").IsRequired();
            builder.Property(p => p.PessoaId).HasColumnName("pessoa_id").IsRequired();
            builder.Property(p => p.Presente).HasColumnName("presente").IsRequired().HasDefaultValue(false);
            builder.Property(p => p.RegistradoPorId).HasColumnName("registrado_por_id");
            builder.Property(p => p.Observacao).HasColumnName("observacao").HasMaxLength(500);

            // Relacionamentos: O EF vai inferir via config feita em Pessoa e Evento opcionalmente, 
            // mas mantemos as FKs com Constraint Names nomeados caso precise gerar o DDL adequadamente
            // Em PessoaMapeamento já existe .WithMany(pr => pr.Pessoa).HasForeignKey(pr => pr.PessoaId)
            // Entao configuraremos só o lado do Usuario que a pessoa não mapeou diretamente e a constraint básica do evento
            builder.HasOne(p => p.Evento)
                .WithMany(e => e.Presencas)
                .HasForeignKey(p => p.EventoId)
                .HasConstraintName("fk_presencas_eventos_evento_id")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.RegistradoPor)
                .WithMany()
                .HasForeignKey(p => p.RegistradoPorId)
                .HasConstraintName("fk_presencas_usuarios_registrado_por_id")
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
