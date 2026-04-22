using IgrejaV2.Dominio.Entidades;

namespace IgrejaV2.Dominio.Interfaces
{
    public interface IRepositorioTipoEvento : IRepositorio<TipoEvento>
    {
        Task<IEnumerable<TipoEvento>> ObterAtivosAsync(CancellationToken ct = default);
    }
}
