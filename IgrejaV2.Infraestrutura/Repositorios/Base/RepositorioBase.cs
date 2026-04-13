using IgrejaV2.Dominio.Interfaces;
using IgrejaV2.Infraestrutura.Contexto;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace IgrejaV2.Infraestrutura.Repositorios.Base
{
    /// <summary>
    /// Implementação genérica de repositório usando Entity Framework Core.
    /// </summary>
    /// <typeparam name="T">Tipo da entidade de domínio.</typeparam>
    public class RepositorioBase<T> : IRepositorio<T> where T : class
    {
        protected readonly IgrejaContexto _contexto;
        protected readonly DbSet<T> _dbSet;

        public RepositorioBase(IgrejaContexto contexto)
        {
            _contexto = contexto;
            _dbSet = contexto.Set<T>();
        }

        // ── Consultas ────────────────────────────────────────────────────────────

        /// <inheritdoc/>
        public async Task<T?> ObterPorIdAsync(int id, CancellationToken ct = default)
            => await _dbSet.FindAsync([id], ct);

        /// <inheritdoc/>
        public async Task<T?> ObterPrimeiroAsync(Expression<Func<T, bool>> predicado, CancellationToken ct = default)
            => await _dbSet.AsNoTracking().FirstOrDefaultAsync(predicado, ct);

        /// <inheritdoc/>
        public async Task<IEnumerable<T>> ListarTodosAsync(CancellationToken ct = default)
            => await _dbSet.AsNoTracking().ToListAsync(ct);

        /// <inheritdoc/>
        public async Task<IEnumerable<T>> ListarAsync(Expression<Func<T, bool>> predicado, CancellationToken ct = default)
            => await _dbSet.AsNoTracking().Where(predicado).ToListAsync(ct);

        /// <inheritdoc/>
        public async Task<bool> ExisteAsync(Expression<Func<T, bool>> predicado, CancellationToken ct = default)
            => await _dbSet.AnyAsync(predicado, ct);

        /// <inheritdoc/>
        public async Task<int> ContarAsync(Expression<Func<T, bool>>? predicado = null, CancellationToken ct = default)
            => predicado is null
                ? await _dbSet.CountAsync(ct)
                : await _dbSet.CountAsync(predicado, ct);

        // ── Persistência ─────────────────────────────────────────────────────────

        /// <inheritdoc/>
        public async Task AdicionarAsync(T entidade, CancellationToken ct = default)
            => await _dbSet.AddAsync(entidade, ct);

        /// <inheritdoc/>
        public async Task AdicionarVariosAsync(IEnumerable<T> entidades, CancellationToken ct = default)
            => await _dbSet.AddRangeAsync(entidades, ct);

        /// <inheritdoc/>
        public Task AtualizarAsync(T entidade, CancellationToken ct = default)
        {
            _dbSet.Update(entidade);
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task RemoverAsync(T entidade, CancellationToken ct = default)
        {
            _dbSet.Remove(entidade);
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public async Task RemoverPorIdAsync(int id, CancellationToken ct = default)
        {
            var entidade = await ObterPorIdAsync(id, ct)
                ?? throw new KeyNotFoundException($"Entidade do tipo {typeof(T).Name} com id {id} não foi encontrada.");
            _dbSet.Remove(entidade);
        }

        // ── Unidade de Trabalho ───────────────────────────────────────────────────

        /// <inheritdoc/>
        public async Task<int> SalvarAlteracoesAsync(CancellationToken ct = default)
            => await _contexto.SaveChangesAsync(ct);
    }
}

