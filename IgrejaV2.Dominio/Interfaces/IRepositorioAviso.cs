using IgrejaV2.Dominio.Entidades;

namespace IgrejaV2.Dominio.Interfaces
{
    public interface IRepositorioAviso : IRepositorio<Aviso>
    {
        Task<IEnumerable<Aviso>> ObterAvisosAtivosAsync(CancellationToken ct = default);
    }
}
