using IgrejaV2.Dominio.Entidades;
using IgrejaV2.Dominio.Interfaces;
using IgrejaV2.Infraestrutura.Contexto;
using IgrejaV2.Infraestrutura.Repositorios.Base;
using Microsoft.EntityFrameworkCore;

namespace IgrejaV2.Infraestrutura.Repositorios
{
    public class RepositorioPessoa : RepositorioBase<Pessoa>, IRepositorioPessoa
    {
        public RepositorioPessoa(IgrejaContexto contexto) : base(contexto) { }

        public async Task<IEnumerable<Pessoa>> ObterAtivosAsync(CancellationToken ct = default)
            => await _dbSet.AsNoTracking().Where(p => p.Ativo).OrderBy(p => p.Nome).ToListAsync(ct);

        public async Task<IEnumerable<Pessoa>> ObterPorFamiliaAsync(int familiaId, CancellationToken ct = default)
            => await _dbSet.AsNoTracking().Where(p => p.FamiliaId == familiaId).OrderBy(p => p.Nome).ToListAsync(ct);

        public async Task<IEnumerable<Pessoa>> BuscarPorNomeAsync(string nome, CancellationToken ct = default)
            => await _dbSet.AsNoTracking().Where(p => p.Nome.Contains(nome)).OrderBy(p => p.Nome).ToListAsync(ct);
    }
}
