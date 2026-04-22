using IgrejaV2.Dominio.Entidades;
using IgrejaV2.Dominio.Interfaces;
using IgrejaV2.Infraestrutura.Contexto;
using IgrejaV2.Infraestrutura.Repositorios.Base;
using Microsoft.EntityFrameworkCore;

namespace IgrejaV2.Infraestrutura.Repositorios
{
    public class RepositorioPresenca : RepositorioBase<Presenca>, IRepositorioPresenca
    {
        public RepositorioPresenca(IgrejaContexto contexto) : base(contexto) { }

        public async Task<IEnumerable<Presenca>> ObterPorEventoAsync(int eventoId, CancellationToken ct = default)
            => await _contexto.Presencas.Where(p => p.EventoId == eventoId).ToListAsync(ct);

        public async Task<IEnumerable<Presenca>> ObterPorPessoaAsync(int pessoaId, CancellationToken ct = default)
            => await _contexto.Presencas.Where(p => p.PessoaId == pessoaId).OrderByDescending(p => p.DataCriacao).ToListAsync(ct);
    }
}
