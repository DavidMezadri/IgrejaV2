using System.Linq.Expressions;

namespace IgrejaV2.Dominio.Interfaces
{
    /// <summary>
    /// Contrato genérico de repositório para operações de persistência e consulta.
    /// </summary>
    /// <typeparam name="T">Tipo da entidade de domínio.</typeparam>
    public interface IRepositorio<T> where T : class
    {
        // ── Consultas ────────────────────────────────────────────────────────────

        /// <summary>Obtém uma entidade pela chave primária. Retorna null se não encontrada.</summary>
        Task<T?> ObterPorIdAsync(int id, CancellationToken ct = default);

        /// <summary>Obtém a primeira entidade que satisfaz o predicado. Retorna null se não encontrada.</summary>
        Task<T?> ObterPrimeiroAsync(Expression<Func<T, bool>> predicado, CancellationToken ct = default);

        /// <summary>Retorna todas as entidades do repositório.</summary>
        Task<IEnumerable<T>> ListarTodosAsync(CancellationToken ct = default);

        /// <summary>Retorna as entidades que satisfazem o predicado informado.</summary>
        Task<IEnumerable<T>> ListarAsync(Expression<Func<T, bool>> predicado, CancellationToken ct = default);

        /// <summary>Verifica se existe alguma entidade que satisfaz o predicado.</summary>
        Task<bool> ExisteAsync(Expression<Func<T, bool>> predicado, CancellationToken ct = default);

        /// <summary>Conta as entidades que satisfazem o predicado. Se nulo, conta todas.</summary>
        Task<int> ContarAsync(Expression<Func<T, bool>>? predicado = null, CancellationToken ct = default);

        // ── Persistência ─────────────────────────────────────────────────────────

        /// <summary>Adiciona uma nova entidade ao repositório.</summary>
        Task AdicionarAsync(T entidade, CancellationToken ct = default);

        /// <summary>Adiciona um conjunto de entidades ao repositório.</summary>
        Task AdicionarVariosAsync(IEnumerable<T> entidades, CancellationToken ct = default);

        /// <summary>Atualiza uma entidade existente no repositório.</summary>
        Task AtualizarAsync(T entidade, CancellationToken ct = default);

        /// <summary>Remove uma entidade do repositório.</summary>
        Task RemoverAsync(T entidade, CancellationToken ct = default);

        /// <summary>Remove a entidade com a chave primária informada.</summary>
        Task RemoverPorIdAsync(int id, CancellationToken ct = default);

        // ── Unidade de Trabalho ───────────────────────────────────────────────────

        /// <summary>Persiste todas as alterações pendentes na base de dados.</summary>
        Task<int> SalvarAlteracoesAsync(CancellationToken ct = default);
    }
}

