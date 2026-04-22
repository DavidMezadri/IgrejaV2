using IgrejaV2.Dominio.Entidades;

namespace IgrejaV2.Dominio.Interfaces
{
    public interface IRepositorioEvento : IRepositorio<Evento>
    {
        Task<IEnumerable<Evento>> ObterEventosAtivosAsync(CancellationToken ct = default);
        Task<Evento?> ObterComPresencasAsync(int id, CancellationToken ct = default);
    }
}
