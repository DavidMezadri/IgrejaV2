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
        public DbSet<Pessoa> Pessoas                            => Set<Pessoa>();
        public DbSet<Endereco> Enderecos                        => Set<Endereco>();
        public DbSet<PessoaEndereco> PessoasEnderecos           => Set<PessoaEndereco>();
        public DbSet<TipoEvento> TiposEvento                    => Set<TipoEvento>();
        public DbSet<Evento> Eventos                            => Set<Evento>();
        public DbSet<Presenca> Presencas                        => Set<Presenca>();
        public DbSet<Igreja> Igrejas                            => Set<Igreja>();
        public DbSet<Patrimonio> Patrimonios                    => Set<Patrimonio>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(IgrejaContexto).Assembly);
        }
    }
}
