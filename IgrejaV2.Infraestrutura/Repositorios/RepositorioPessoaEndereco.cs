using IgrejaV2.Dominio.Entidades;
using IgrejaV2.Dominio.Interfaces;
using IgrejaV2.Infraestrutura.Contexto;
using IgrejaV2.Infraestrutura.Repositorios.Base;
using Microsoft.EntityFrameworkCore;

namespace IgrejaV2.Infraestrutura.Repositorios
{
    public class RepositorioPessoaEndereco : RepositorioBase<PessoaEndereco>, IRepositorioPessoaEndereco
    {
        public RepositorioPessoaEndereco(IgrejaContexto contexto) : base(contexto) { }

        public async Task<IEnumerable<PessoaEndereco>> ObterPorPessoaAsync(int pessoaId, CancellationToken ct = default)
            => await _dbSet.AsNoTracking().Include(pe => pe.Endereco).Where(pe => pe.PessoaId == pessoaId).ToListAsync(ct);
    }
}
