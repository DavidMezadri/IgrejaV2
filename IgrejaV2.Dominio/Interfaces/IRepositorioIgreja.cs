using IgrejaV2.Dominio.Entidades;

namespace IgrejaV2.Dominio.Interfaces
{
    public interface IRepositorioIgreja : IRepositorio<Igreja>
    {
        Task<Igreja?> ObterComPatrimoniosAsync(int id, CancellationToken ct = default);
    }
}
