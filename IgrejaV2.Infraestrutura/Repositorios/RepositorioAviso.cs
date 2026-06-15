using IgrejaV2.Dominio.Entidades;
using IgrejaV2.Dominio.Interfaces;
using IgrejaV2.Infraestrutura.Contexto;
using IgrejaV2.Infraestrutura.Repositorios.Base;
using Microsoft.EntityFrameworkCore;

namespace IgrejaV2.Infraestrutura.Repositorios
{
    public class RepositorioAviso : RepositorioBase<Aviso>, IRepositorioAviso
    {
        public RepositorioAviso(IgrejaContexto contexto) : base(contexto) { }

        public async Task<IEnumerable<Aviso>> ObterAvisosAtivosAsync(CancellationToken ct = default)
            => await _dbSet.AsNoTracking()
                           .Where(a => a.Ativo && !a.Deletado)
                           .OrderByDescending(a => a.Data)
                           .ToListAsync(ct);
    }
}
