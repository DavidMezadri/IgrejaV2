using IgrejaV2.Dominio.Entidades;

namespace IgrejaV2.Dominio.Interfaces
{
    public interface IRepositorioPresenca : IRepositorio<Presenca>
    {
        Task<IEnumerable<Presenca>> ObterPorEventoAsync(int eventoId, CancellationToken ct = default);
        Task<IEnumerable<Presenca>> ObterPorPessoaAsync(int pessoaId, CancellationToken ct = default);
    }
}
