using IgrejaV2.Dominio.Entidades;
using IgrejaV2.Dominio.Interfaces;
using IgrejaV2.Infraestrutura.Contexto;
using IgrejaV2.Infraestrutura.Repositorios.Base;
using Microsoft.EntityFrameworkCore;

namespace IgrejaV2.Infraestrutura.Repositorios
{
    public class RepositorioTipoEvento : RepositorioBase<TipoEvento>, IRepositorioTipoEvento
    {
        public RepositorioTipoEvento(IgrejaContexto contexto) : base(contexto) { }

        public async Task<IEnumerable<TipoEvento>> ObterAtivosAsync(CancellationToken ct = default)
            => await _dbSet.AsNoTracking().Where(t => t.Ativo).OrderBy(t => t.Nome).ToListAsync(ct);
    }
}
