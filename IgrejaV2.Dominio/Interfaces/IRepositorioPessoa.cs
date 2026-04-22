using IgrejaV2.Dominio.Entidades;

namespace IgrejaV2.Dominio.Interfaces
{
    public interface IRepositorioPessoa : IRepositorio<Pessoa>
    {
        Task<IEnumerable<Pessoa>> ObterPorFamiliaAsync(int familiaId, CancellationToken ct = default);
        Task<IEnumerable<Pessoa>> ObterAtivosAsync(CancellationToken ct = default);
    }
}
