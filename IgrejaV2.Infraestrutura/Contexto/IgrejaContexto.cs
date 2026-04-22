using IgrejaV2.Dominio.Entidades;
using IgrejaV2.Infraestrutura.Contexto.Mapeamento;
using Microsoft.EntityFrameworkCore;

namespace IgrejaV2.Infraestrutura.Contexto
{
    public class IgrejaContexto : DbContext
    {
        public IgrejaContexto(DbContextOptions<IgrejaContexto> opcoes) : base(opcoes) { }

        public DbSet<Endereco> Enderecos                        => Set<Endereco>();
        public DbSet<Evento> Eventos                            => Set<Evento>();
        public DbSet<Familia> Familias                          => Set<Familia>();
        public DbSet<Igreja> Igrejas                            => Set<Igreja>();
        public DbSet<Log> Logs                                  => Set<Log>();
        public DbSet<Patrimonio> Patrimonios                    => Set<Patrimonio>();
        public DbSet<PessoaEndereco> PessoasEnderecos           => Set<PessoaEndereco>();
        public DbSet<Pessoa> Pessoas                            => Set<Pessoa>();
        public DbSet<Presenca> Presencas                        => Set<Presenca>();
        public DbSet<TipoEvento> TiposEvento                    => Set<TipoEvento>();
        public DbSet<Usuario> Usuarios                          => Set<Usuario>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Aplica sua configuração universal de letras minúsculas (Id, DataCriacao, Deletado...) em TODAS as tabelas
            modelBuilder.ApplyBaseEntityConfiguration();
            
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(IgrejaContexto).Assembly);
        }
    }
}
