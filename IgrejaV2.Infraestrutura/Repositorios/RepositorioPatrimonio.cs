using IgrejaV2.Dominio.Entidades;
using IgrejaV2.Dominio.Interfaces;
using IgrejaV2.Infraestrutura.Contexto;
using IgrejaV2.Infraestrutura.Repositorios.Base;
using Microsoft.EntityFrameworkCore;

namespace IgrejaV2.Infraestrutura.Repositorios
{
    public class RepositorioPatrimonio : RepositorioBase<Patrimonio>, IRepositorioPatrimonio
    {
        public RepositorioPatrimonio(IgrejaContexto contexto) : base(contexto) { }

        public async Task<IEnumerable<Patrimonio>> ObterPorIgrejaAsync(int igrejaId, CancellationToken ct = default)
            => await _dbSet.AsNoTracking().Where(p => p.IgrejaId == igrejaId).ToListAsync(ct);
    }
}
