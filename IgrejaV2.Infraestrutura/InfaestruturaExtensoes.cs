using IgrejaV2.Dominio.Interfaces;
using IgrejaV2.Infraestrutura.Contexto;
using IgrejaV2.Infraestrutura.Repositorios;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IgrejaV2.Infraestrutura
{
    public static class InfraestruturaExtensoes
    {
        /// <summary>
        /// Extensão de DI — registra toda a camada de infraestrutura.
        /// </summary>
        public static IServiceCollection AddInfraestrutura(this IServiceCollection services, IConfiguration configuracao)
        {
            var connectionString = configuracao.GetConnectionString("Postgres")
                ?? throw new InvalidOperationException("String de conexão 'Postgres' não configurada.");

            // EF Core — PostgreSQL
            services.AddDbContext<IgrejaContexto>(opcoes => opcoes.UseNpgsql(connectionString));

            var ormConfig = configuracao["ConfiguracoesDeBanco:ProviderOrmSelecionado"] ?? "EFCore";

            if (ormConfig.Equals("Dapper", StringComparison.OrdinalIgnoreCase))
            {
                // Inicializador e Repositórios Dapper
                services.AddScoped<DatabaseConfig.IInicializadorBanco, DatabaseConfig.InicializadorBancoDapper>();

                services.AddScoped<IRepositorioEndereco>(p       => new Repositorios.Dapper.RepositorioEnderecoDapper(connectionString));
                services.AddScoped<IRepositorioEvento>(p         => new Repositorios.Dapper.RepositorioEventoDapper(connectionString));
                services.AddScoped<IRepositorioFamilia>(p        => new Repositorios.Dapper.RepositorioFamiliaDapper(connectionString));
                services.AddScoped<IRepositorioIgreja>(p         => new Repositorios.Dapper.RepositorioIgrejaDapper(connectionString));
                services.AddScoped<IRepositorioLog>(p            => new Repositorios.Dapper.RepositorioLogDapper(connectionString));
                services.AddScoped<IRepositorioPatrimonio>(p     => new Repositorios.Dapper.RepositorioPatrimonioDapper(connectionString));
                services.AddScoped<IRepositorioPessoa>(p         => new Repositorios.Dapper.RepositorioPessoaDapper(connectionString));
                services.AddScoped<IRepositorioPessoaEndereco>(p => new Repositorios.Dapper.RepositorioPessoaEnderecoDapper(connectionString));
                services.AddScoped<IRepositorioPresenca>(p       => new Repositorios.Dapper.RepositorioPresencaDapper(connectionString));
                services.AddScoped<IRepositorioTipoEvento>(p     => new Repositorios.Dapper.RepositorioTipoEventoDapper(connectionString));
                services.AddScoped<IRepositorioUsuario>(p        => new Repositorios.Dapper.RepositorioUsuarioDapper(connectionString));
            }
            else
            {
                // Inicializador e Repositórios EF Core
                services.AddScoped<DatabaseConfig.IInicializadorBanco, DatabaseConfig.InicializadorBancoEF>();

                services.AddScoped<IRepositorioEndereco, RepositorioEndereco>();
                services.AddScoped<IRepositorioEvento, RepositorioEvento>();
                services.AddScoped<IRepositorioFamilia, RepositorioFamilia>();
                services.AddScoped<IRepositorioIgreja, RepositorioIgreja>();
                services.AddScoped<IRepositorioLog, RepositorioLog>();
                services.AddScoped<IRepositorioPatrimonio, RepositorioPatrimonio>();
                services.AddScoped<IRepositorioPessoa, RepositorioPessoa>();
                services.AddScoped<IRepositorioPessoaEndereco, RepositorioPessoaEndereco>();
                services.AddScoped<IRepositorioPresenca, RepositorioPresenca>();
                services.AddScoped<IRepositorioTipoEvento, RepositorioTipoEvento>();
                services.AddScoped<IRepositorioUsuario, RepositorioUsuario>();
            }

            return services;
        }
    }
}
