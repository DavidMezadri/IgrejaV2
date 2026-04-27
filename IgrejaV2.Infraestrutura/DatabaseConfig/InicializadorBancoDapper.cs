using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace IgrejaV2.Infraestrutura.DatabaseConfig
{
    public class InicializadorBancoDapper : IInicializadorBanco
    {
        private readonly string _connectionString;

        public InicializadorBancoDapper(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Postgres")
                ?? throw new InvalidOperationException("Connection string 'Postgres' não encontrada."); ;
        }

        public async Task InicializarAsync()
        {
            var scriptsDir = Path.Combine(AppContext.BaseDirectory, "Scripts");
            if (!Directory.Exists(scriptsDir))
                return;

            var scripts = Directory.GetFiles(scriptsDir, "*.sql")
                .OrderBy(f => f)
                .ToList();

            using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            foreach (var scriptPath in scripts)
            {
                var script = await File.ReadAllTextAsync(scriptPath);
                try
                {
                    await SqlMapper.ExecuteAsync(conn, script);
                }
                catch (PostgresException ex) when (ex.SqlState == "42P07")
                {
                    // Ignorar: tabela já existe (script 01 em banco já inicializado)
                }
            }
        }
    }
}
