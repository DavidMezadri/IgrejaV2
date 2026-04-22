using IgrejaV2.Dominio.Entidades;

namespace IgrejaV2.Dominio.Interfaces
{
    public interface IRepositorioFamilia : IRepositorio<Familia>
    {
        Task<Familia?> ObterComMembrosAsync(int id, CancellationToken ct = default);
    }
}
