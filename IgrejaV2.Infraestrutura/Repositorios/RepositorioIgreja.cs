using IgrejaV2.Dominio.Entidades;
using IgrejaV2.Dominio.Interfaces;
using IgrejaV2.Infraestrutura.Contexto;
using IgrejaV2.Infraestrutura.Repositorios.Base;
using Microsoft.EntityFrameworkCore;

namespace IgrejaV2.Infraestrutura.Repositorios
{
    public class RepositorioIgreja : RepositorioBase<Igreja>, IRepositorioIgreja
    {
        public RepositorioIgreja(IgrejaContexto contexto) : base(contexto) { }

        public async Task<Igreja?> ObterComPatrimoniosAsync(int id, CancellationToken ct = default)
            => await _dbSet.AsNoTracking().Include(i => i.Patrimonios).FirstOrDefaultAsync(i => i.Id == id, ct);
    }
}
