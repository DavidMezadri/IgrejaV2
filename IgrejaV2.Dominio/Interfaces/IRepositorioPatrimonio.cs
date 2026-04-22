using IgrejaV2.Dominio.Entidades;

namespace IgrejaV2.Dominio.Interfaces
{
    public interface IRepositorioPatrimonio : IRepositorio<Patrimonio>
    {
        Task<IEnumerable<Patrimonio>> ObterPorIgrejaAsync(int igrejaId, CancellationToken ct = default);
    }
}
