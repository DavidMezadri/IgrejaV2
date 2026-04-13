using IgrejaV2.Dominio.Interfaces;
using IgrejaV2.Infraestrutura.Contexto;
using IgrejaV2.Infraestrutura.Repositorios;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IgrejaV2.Infraestrutura
{
    public static class InfaestruturaExtensoes
    {
        /// <summary>
        /// Extensão de DI — registra toda a camada de infraestrutura.
        /// </summary>
        public static class InfraestruturaExtensoes
        {
            public static IServiceCollection AddInfraestrutura(this IServiceCollection services, IConfiguration configuracao)
            {
                var connectionString = configuracao.GetConnectionString("Postgres")
                    ?? throw new InvalidOperationException("String de conexão 'Postgres' não configurada.");

                // EF Core — PostgreSQL
                services.AddDbContext<IgrejaContexto>(opcoes => opcoes.UseNpgsql(connectionString).UseSnakeCaseNamingConvention());

                var ormConfig = configuracao["ConfiguracoesDeBanco:ProviderOrmSelecionado"] ?? "EFCore";

                if (ormConfig.Equals("Dapper", StringComparison.OrdinalIgnoreCase))
                {
                    // Inicializador e Repositórios Dapper
                    services.AddScoped<DatabaseConfig.IInicializadorBanco, DatabaseConfig.InicializadorBancoDapper>();

                    services.AddScoped<IRepositorioAlerta>(p            => new Repositorios.Dapper.RepositorioAlertaDapper(connectionString));
                    services.AddScoped<IRepositorioDashboard>(p         => new Repositorios.Dapper.RepositorioDashboardDapper(connectionString));
                    services.AddScoped<IRepositorioEvento>(p            => new Repositorios.Dapper.RepositorioEventoDapper(connectionString));
                    services.AddScoped<IRepositorioLog>(p               => new Repositorios.Dapper.RepositorioLogDapper(connectionString));
                    services.AddScoped<IRepositorioPresenca>(p          => new Repositorios.Dapper.RepositorioPresencaDapper(connectionString));
                    services.AddScoped<IRepositorioUsuario>(p           => new Repositorios.Dapper.RepositorioUsuarioDapper(connectionString));
                }
                else
                {
                    // Inicializador e Repositórios EF Core
                    services.AddScoped<DatabaseConfig.IInicializadorBanco, DatabaseConfig.InicializadorBancoEF>();

                    services.AddScoped<IRepositorioAlerta, RepositorioAlerta>();
                    services.AddScoped<IRepositorioDashboard, RepositorioDashboard>();
                    services.AddScoped<IRepositorioEvento, RepositorioEvento>();
                    services.AddScoped<IRepositorioLog, RepositorioLog>();
                    services.AddScoped<IRepositorioPresenca, RepositorioPresenca>();
                    services.AddScoped<IRepositorioUsuario, RepositorioUsuario>();
                }

                return services;
            }

        }
    }
}
