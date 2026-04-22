using IgrejaV2.Dominio.Entidades;
using IgrejaV2.Dominio.Interfaces;
using IgrejaV2.Infraestrutura.Contexto;
using IgrejaV2.Infraestrutura.Repositorios.Base;
using Microsoft.EntityFrameworkCore;

namespace IgrejaV2.Infraestrutura.Repositorios
{
    public class RepositorioFamilia : RepositorioBase<Familia>, IRepositorioFamilia
    {
        public RepositorioFamilia(IgrejaContexto contexto) : base(contexto) { }

        public async Task<Familia?> ObterComMembrosAsync(int id, CancellationToken ct = default)
            => await _dbSet.AsNoTracking().Include(f => f.Membros).FirstOrDefaultAsync(f => f.Id == id, ct);
    }
}
