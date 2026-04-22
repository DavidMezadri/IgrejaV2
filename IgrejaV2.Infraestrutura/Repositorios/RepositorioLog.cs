using IgrejaV2.Dominio.Entidades;
using IgrejaV2.Dominio.Interfaces;
using IgrejaV2.Infraestrutura.Contexto;
using IgrejaV2.Infraestrutura.Repositorios.Base;
using Microsoft.EntityFrameworkCore;

namespace IgrejaV2.Infraestrutura.Repositorios
{
    public class RepositorioLog : RepositorioBase<Log>, IRepositorioLog
    {
        public RepositorioLog(IgrejaContexto contexto) : base(contexto) { }

        public async Task<IEnumerable<Log>> ObterPorUsuarioAsync(int usuarioId, CancellationToken ct = default)
            => await _contexto.Logs.Where(l => l.UsuarioId == usuarioId).OrderByDescending(l => l.DataCriacao).ToListAsync(ct);

        public async Task<IEnumerable<Log>> ObterPorEntidadeAsync(string entidade, int entidadeId, CancellationToken ct = default)
            => await _contexto.Logs.Where(l => l.Entidade == entidade && l.EntidadeId == entidadeId).OrderByDescending(l => l.DataCriacao).ToListAsync(ct);
    }
}
