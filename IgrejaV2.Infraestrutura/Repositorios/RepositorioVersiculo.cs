using IgrejaV2.Dominio.Entidades;
using IgrejaV2.Dominio.Interfaces;
using IgrejaV2.Infraestrutura.Contexto;
using IgrejaV2.Infraestrutura.Repositorios.Base;
using Microsoft.EntityFrameworkCore;

namespace IgrejaV2.Infraestrutura.Repositorios
{
    public class RepositorioVersiculo : RepositorioBase<Versiculo>, IRepositorioVersiculo
    {
        public RepositorioVersiculo(IgrejaContexto contexto) : base(contexto) { }

        public async Task<Versiculo?> ObterPorLivroCaptituloNumeroAsync(int livro, int capitulo, int numero, int traducaoId)
            => await _dbSet.AsNoTracking()
                           .Include(v => v.Traducao)
                           .FirstOrDefaultAsync(v => v.Livro == livro &&
                                                      v.Capitulo == capitulo &&
                                                      v.Numero == numero &&
                                                      v.TraducaoId == traducaoId &&
                                                      !v.Deletado);

        public async Task<IEnumerable<Versiculo>> ObterPorLivroAsync(int livro, int traducaoId)
            => await _dbSet.AsNoTracking()
                           .Include(v => v.Traducao)
                           .Where(v => v.Livro == livro &&
                                      v.TraducaoId == traducaoId &&
                                      !v.Deletado)
                           .OrderBy(v => v.Capitulo)
                           .ThenBy(v => v.Numero)
                           .ToListAsync();

        public async Task<IEnumerable<Versiculo>> ObterPorLivroCaptituloAsync(int livro, int capitulo, int traducaoId)
            => await _dbSet.AsNoTracking()
                           .Include(v => v.Traducao)
                           .Where(v => v.Livro == livro &&
                                      v.Capitulo == capitulo &&
                                      v.TraducaoId == traducaoId &&
                                      !v.Deletado)
                           .OrderBy(v => v.Numero)
                           .ToListAsync();
    }
}
