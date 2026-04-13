using IgrejaV2.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;

namespace IgrejaV2.Infraestrutura.Contexto
{
    public class IgrejaContexto : DbContext
    {
        public IgrejaContexto(DbContextOptions<IgrejaContexto> opcoes) : base(opcoes) { }

        public DbSet<Usuario> Usuarios                          => Set<Usuario>();
        public DbSet<Log> Logs                                  => Set<Log>();
        public DbSet<Familia> Familias                          => Set<Familia>();
        public DbSet<PessoaFamilia> PessoaFamilias              => Set<PessoaFamilia>();
        public DbSet<Endereco> Enderecos                        => Set<Endereco>();
        public DbSet<TipoEvento> TiposEvento                    => Set<TipoEvento>();
        public DbSet<Evento> Eventos                            => Set<Evento>();
        public DbSet<EventoConteudo> EventoConteudos            => Set<EventoConteudo>();
        public DbSet<Presenca> Presencas                        => Set<Presenca>();
        public DbSet<Alerta> Alertas                            => Set<Alerta>();
        public DbSet<Observacao> Observacoes                    => Set<Observacao>();
        public DbSet<Escala> Escalas                            => Set<Escala>();
        public DbSet<EscalaSubstituicao> EscalaSubstituicoes    => Set<EscalaSubstituicao>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(IgrejaContexto).Assembly);
        }
    }


}
