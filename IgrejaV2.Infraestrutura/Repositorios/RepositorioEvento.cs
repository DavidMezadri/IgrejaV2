using IgrejaV2.Dominio.Entidades;
using IgrejaV2.Dominio.Interfaces;
using IgrejaV2.Infraestrutura.Contexto;
using Microsoft.EntityFrameworkCore;

namespace IgrejaV2.Infraestrutura.Repositorios
{
    public class RepositorioEvento : RepositorioBase<Evento>, IRepositorioEvento
    {
        public RepositorioEvento(IgrejaContexto contexto) : base(contexto) { }

        public async Task<IEnumerable<Evento>> ObterEventosAtivosAsync(CancellationToken ct = default)
            => await _dbSet.AsNoTracking()
                           .Include(e => e.TipoEvento)
                           .Where(e => e.Ativo)
                           .OrderByDescending(e => e.DataInicio)
                           .ToListAsync(ct);

        public async Task<Evento?> ObterComPresencasAsync(int id, CancellationToken ct = default)
            => await _dbSet.AsNoTracking()
                           .Include(e => e.Presencas)
                               .ThenInclude(p => p.Usuario)
                           .FirstOrDefaultAsync(e => e.Id == id, ct);

        public async Task<Evento?> ObterComConteudosAsync(int id, CancellationToken ct = default)
            => await _dbSet.AsNoTracking()
                           .Include(e => e.Conteudos)
                           .FirstOrDefaultAsync(e => e.Id == id, ct);

        public async Task<Evento?> ObterUltimoEventoAsync(CancellationToken ct = default)
            => await _dbSet.AsNoTracking()
                           .Include(e => e.TipoEvento)
                           .Where(e => e.Ativo)
                           .OrderByDescending(e => e.DataInicio)
                           .FirstOrDefaultAsync(ct);
    }
}
