using Dapper;
using IgrejaV2.Dominio.Interfaces;
using Npgsql;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;

namespace IgrejaV2.Infraestrutura.Repositorios.Base
{
    /// <summary>
    /// Implementação genérica de repositório usando Dapper + SQL manual.
    /// Espelha o <see cref="RepositorioBase{T}"/> (EF Core), escrevendo as queries diretamente.
    /// </summary>
    /// <typeparam name="T">Tipo da entidade de domínio.</typeparam>
    public abstract class RepositorioBaseDapper<T> : IRepositorio<T> where T : class
    {
        private readonly string _connectionString;

        /// <summary>Nome da tabela no banco de dados (ex: "usuarios").</summary>
        protected abstract string NomeTabela { get; }

        /// <summary>
        /// Nome da propriedade/coluna da chave primária. Padrão: "Id".
        /// A coluna no banco deve ter o mesmo nome (case-insensitive no PostgreSQL).
        /// </summary>
        protected virtual string ChavePrimaria => "Id";

        // Tipos simples mapeáveis para colunas SQL — exclui navigation properties
        private static readonly HashSet<Type> TiposColuna = new()
        {
            typeof(bool),   typeof(bool?),
            typeof(byte),   typeof(byte?),
            typeof(short),  typeof(short?),
            typeof(int),    typeof(int?),
            typeof(long),   typeof(long?),
            typeof(float),  typeof(float?),
            typeof(double), typeof(double?),
            typeof(decimal),typeof(decimal?),
            typeof(string),
            typeof(char),   typeof(char?),
            typeof(DateTime),       typeof(DateTime?),
            typeof(DateTimeOffset), typeof(DateTimeOffset?),
            typeof(TimeSpan),       typeof(TimeSpan?),
            typeof(Guid),           typeof(Guid?),
            typeof(byte[])
        };

        protected RepositorioBaseDapper(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>Cria e retorna uma nova conexão com o banco de dados.</summary>
        protected IDbConnection CriarConexao() => new NpgsqlConnection(_connectionString);

        // ── Consultas ────────────────────────────────────────────────────────────

        /// <inheritdoc/>
        public async Task<T?> ObterPorIdAsync(int id, CancellationToken ct = default)
        {
            var sql = $"""SELECT * FROM "{NomeTabela}" WHERE "{ChavePrimaria}" = @Id""";
            using var conn = CriarConexao();
            return await conn.QueryFirstOrDefaultAsync<T>(
                new CommandDefinition(sql, new { Id = id }, cancellationToken: ct));
        }

        /// <inheritdoc/>
        /// <remarks>
        /// ⚠️ Carrega todos os registros e filtra em memória.
        /// Para grandes volumes, sobrescreva com SQL WHERE específico.
        /// </remarks>
        public virtual async Task<T?> ObterPrimeiroAsync(Expression<Func<T, bool>> predicado, CancellationToken ct = default)
        {
            var todos = await ListarTodosAsync(ct);
            return todos.AsQueryable().FirstOrDefault(predicado);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<T>> ListarTodosAsync(CancellationToken ct = default)
        {
            var sql = $"""SELECT * FROM "{NomeTabela}" """;
            using var conn = CriarConexao();
            return await conn.QueryAsync<T>(new CommandDefinition(sql, cancellationToken: ct));
        }

        /// <inheritdoc/>
        /// <remarks>
        /// ⚠️ Carrega todos os registros e filtra em memória.
        /// Para grandes volumes, sobrescreva com SQL WHERE específico.
        /// </remarks>
        public virtual async Task<IEnumerable<T>> ListarAsync(Expression<Func<T, bool>> predicado, CancellationToken ct = default)
        {
            var todos = await ListarTodosAsync(ct);
            return todos.AsQueryable().Where(predicado).ToList();
        }

        /// <inheritdoc/>
        /// <remarks>
        /// ⚠️ Carrega todos os registros e filtra em memória.
        /// Para grandes volumes, sobrescreva com SQL EXISTS/WHERE específico.
        /// </remarks>
        public virtual async Task<bool> ExisteAsync(Expression<Func<T, bool>> predicado, CancellationToken ct = default)
        {
            var todos = await ListarTodosAsync(ct);
            return todos.AsQueryable().Any(predicado);
        }

        /// <inheritdoc/>
        /// <remarks>
        /// Sem predicado: executa <c>COUNT(*)</c> diretamente no banco. 
        /// Com predicado: carrega todos e conta em memória — sobrescreva com SQL COUNT+WHERE.
        /// </remarks>
        public virtual async Task<int> ContarAsync(Expression<Func<T, bool>>? predicado = null, CancellationToken ct = default)
        {
            if (predicado is null)
            {
                var sql = $"""SELECT COUNT(*) FROM "{NomeTabela}" """;
                using var conn = CriarConexao();
                return await conn.ExecuteScalarAsync<int>(new CommandDefinition(sql, cancellationToken: ct));
            }

            var todos = await ListarTodosAsync(ct);
            return todos.AsQueryable().Count(predicado);
        }

        // ── Persistência ─────────────────────────────────────────────────────────

        /// <inheritdoc/>
        public async Task AdicionarAsync(T entidade, CancellationToken ct = default)
        {
            var sql = GerarSqlInsert();
            var parametros = GerarParametros(entidade);
            using var conn = CriarConexao();
            await conn.ExecuteAsync(new CommandDefinition(sql, parametros, cancellationToken: ct));
        }

        /// <inheritdoc/>
        public async Task AdicionarVariosAsync(IEnumerable<T> entidades, CancellationToken ct = default)
        {
            var sql = GerarSqlInsert();
            using var conn = CriarConexao();
            conn.Open();
            using var transacao = conn.BeginTransaction();
            try
            {
                foreach (var entidade in entidades)
                {
                    var parametros = GerarParametros(entidade);
                    await conn.ExecuteAsync(new CommandDefinition(sql, parametros, transacao, cancellationToken: ct));
                }
                transacao.Commit();
            }
            catch
            {
                transacao.Rollback();
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task AtualizarAsync(T entidade, CancellationToken ct = default)
        {
            var sql = GerarSqlUpdate();
            var parametros = GerarParametros(entidade, incluirPk: true);
            using var conn = CriarConexao();
            await conn.ExecuteAsync(new CommandDefinition(sql, parametros, cancellationToken: ct));
        }

        /// <inheritdoc/>
        public async Task RemoverAsync(T entidade, CancellationToken ct = default)
        {
            var sql = $"""DELETE FROM "{NomeTabela}" WHERE "{ChavePrimaria}" = @{ChavePrimaria}""";
            var prop = ObterPropriedadePk();
            var parametros = new DynamicParameters();
            parametros.Add(ChavePrimaria, prop?.GetValue(entidade));
            using var conn = CriarConexao();
            await conn.ExecuteAsync(new CommandDefinition(sql, parametros, cancellationToken: ct));
        }

        /// <inheritdoc/>
        public async Task RemoverPorIdAsync(int id, CancellationToken ct = default)
        {
            var sql = $"""DELETE FROM "{NomeTabela}" WHERE "{ChavePrimaria}" = @Id""";
            using var conn = CriarConexao();
            var afetados = await conn.ExecuteAsync(
                new CommandDefinition(sql, new { Id = id }, cancellationToken: ct));

            if (afetados == 0)
                throw new KeyNotFoundException($"Entidade do tipo {typeof(T).Name} com id {id} não foi encontrada.");
        }

        // ── Unidade de Trabalho ───────────────────────────────────────────────────

        /// <inheritdoc/>
        /// <remarks>
        /// Dapper não possui Unit of Work com change tracking.
        /// Cada operação já é auto-commitada. Este método é um no-op e retorna 0.
        /// </remarks>
        public Task<int> SalvarAlteracoesAsync(CancellationToken ct = default)
            => Task.FromResult(0);

        // ── Utilitários SQL para subclasses ───────────────────────────────────────

        /// <summary>Executa uma query SQL e retorna lista de <typeparamref name="T"/>.</summary>
        protected async Task<IEnumerable<T>> ConsultarAsync(string sql, object? parametros = null, CancellationToken ct = default)
        {
            using var conn = CriarConexao();
            return await conn.QueryAsync<T>(new CommandDefinition(sql, parametros, cancellationToken: ct));
        }

        /// <summary>Executa uma query SQL e retorna o primeiro <typeparamref name="T"/> encontrado.</summary>
        protected async Task<T?> ConsultarUmAsync(string sql, object? parametros = null, CancellationToken ct = default)
        {
            using var conn = CriarConexao();
            return await conn.QueryFirstOrDefaultAsync<T>(new CommandDefinition(sql, parametros, cancellationToken: ct));
        }

        /// <summary>Executa um comando SQL (INSERT/UPDATE/DELETE) e retorna linhas afetadas.</summary>
        protected async Task<int> ExecutarAsync(string sql, object? parametros = null, CancellationToken ct = default)
        {
            using var conn = CriarConexao();
            return await conn.ExecuteAsync(new CommandDefinition(sql, parametros, cancellationToken: ct));
        }

        /// <summary>Executa uma query e retorna um valor escalar.</summary>
        protected async Task<TResult?> EscalarAsync<TResult>(string sql, object? parametros = null, CancellationToken ct = default)
        {
            using var conn = CriarConexao();
            return await conn.ExecuteScalarAsync<TResult>(new CommandDefinition(sql, parametros, cancellationToken: ct));
        }

        // ── Helpers de reflexão (geração de SQL) ─────────────────────────────────

        private string GerarSqlInsert()
        {
            var props = ObterPropriedadesMapeadas(incluirPk: false);
            var colunas = string.Join(", ", props.Select(p => $"\"{p.Name}\""));
            var valores = string.Join(", ", props.Select(p => $"@{p.Name}"));
            return $"""INSERT INTO "{NomeTabela}" ({colunas}) VALUES ({valores})""";
        }

        private string GerarSqlUpdate()
        {
            var props = ObterPropriedadesMapeadas(incluirPk: false);
            var set = string.Join(", ", props.Select(p => $"\"{p.Name}\" = @{p.Name}"));
            return $"""UPDATE "{NomeTabela}" SET {set} WHERE "{ChavePrimaria}" = @{ChavePrimaria}""";
        }

        private DynamicParameters GerarParametros(T entidade, bool incluirPk = false)
        {
            var props = ObterPropriedadesMapeadas(incluirPk);
            var parametros = new DynamicParameters();
            foreach (var prop in props)
                parametros.Add(prop.Name, prop.GetValue(entidade));
            return parametros;
        }

        private IEnumerable<PropertyInfo> ObterPropriedadesMapeadas(bool incluirPk)
            => typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead
                    && EhTipoColuna(p.PropertyType)
                    && (incluirPk || !string.Equals(p.Name, ChavePrimaria, StringComparison.OrdinalIgnoreCase)));

        private PropertyInfo? ObterPropriedadePk()
            => typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .FirstOrDefault(p =>
                    string.Equals(p.Name, ChavePrimaria, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(p.Name, "Id", StringComparison.OrdinalIgnoreCase));

        private static bool EhTipoColuna(Type tipo)
            => TiposColuna.Contains(tipo)
               || tipo.IsEnum
               || (Nullable.GetUnderlyingType(tipo)?.IsEnum == true);
    }
}

