using IgrejaV2.Dominio.Entidades;
using IgrejaV2.Dominio.Interfaces;
using IgrejaV2.Infraestrutura.Contexto;
using IgrejaV2.Infraestrutura.Repositorios.Base;
using Microsoft.EntityFrameworkCore;

namespace IgrejaV2.Infraestrutura.Repositorios
{
    public class RepositorioTraducao : RepositorioBase<Traducao>, IRepositorioTraducao
    {
        public RepositorioTraducao(IgrejaContexto contexto) : base(contexto) { }

        public async Task<Traducao?> ObterPorAbreviacaoAsync(string abreviacao)
            => await _dbSet.AsNoTracking()
                           .FirstOrDefaultAsync(t => t.Abreviacao == abreviacao && !t.Deletado);
    }
}
