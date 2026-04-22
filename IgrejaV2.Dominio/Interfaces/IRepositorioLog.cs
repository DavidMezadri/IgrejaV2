using IgrejaV2.Dominio.Entidades;

namespace IgrejaV2.Dominio.Interfaces
{
    public interface IRepositorioLog : IRepositorio<Log>
    {
        Task<IEnumerable<Log>> ObterPorUsuarioAsync(int usuarioId, CancellationToken ct = default);
        Task<IEnumerable<Log>> ObterPorEntidadeAsync(string entidade, int entidadeId, CancellationToken ct = default);
    }
}
