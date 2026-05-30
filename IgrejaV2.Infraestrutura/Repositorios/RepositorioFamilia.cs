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

        public async Task<Familia?> ObterPorNomeAsync(String nome, CancellationToken ct = default)
            => await _contexto.Familias.FirstOrDefaultAsync(n => n.Nome == nome, ct);

        public async Task<Familia?> ObterComMembrosAsync(int id, CancellationToken ct = default)
            => await _dbSet.AsNoTracking()
            .Include(f => f.Responsavel)
            .Include(f => f.Membros)
            .FirstOrDefaultAsync(f => f.Id == id, ct);

        public override async Task<Familia?> ObterPorIdAsync(int id, CancellationToken ct = default)
            => await _dbSet.AsNoTracking()
            .Include(f => f.Responsavel)
            .Include(f => f.Membros)
            .FirstOrDefaultAsync(f => f.Id == id, ct);

        public async Task<IEnumerable<Familia>> BuscarPorNomeAsync(string nome, CancellationToken ct = default)
            => await _dbSet.AsNoTracking()
            .Where(f => f.Nome.Contains(nome))
            .OrderBy(f => f.Nome)
            .ToListAsync(ct);
    }
}
