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
            using var conn = new NpgsqlConnection(_connectionString);

            var scriptPath = Path.Combine(AppContext.BaseDirectory, "Scripts", "01_TabelasIniciais.sql");
            if (File.Exists(scriptPath))
            {
                var script = await File.ReadAllTextAsync(scriptPath);
                try
                {
                    await conn.OpenAsync();
                    await SqlMapper.ExecuteAsync(conn, script);
                }
                catch (PostgresException ex) when (ex.SqlState == "42P07")
                {
                    // Ignorar erro 42P07: "relation already exists" (Tabelas já foram criadas)
                }
            }
        }
    }
}
