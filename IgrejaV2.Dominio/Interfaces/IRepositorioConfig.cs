using IgrejaV2.Dominio.Entidades;

namespace IgrejaV2.Dominio.Interfaces;

public interface IRepositorioConfig : IRepositorio<Configuracao>
{
    Task<Configuracao?> ObterPorChaveAsync(string chave, CancellationToken ct = default);
    Task<IEnumerable<Configuracao>> ObterTodasAsync(CancellationToken ct = default);
    Task AtualizarOuCriarAsync(string chave, string valor, CancellationToken ct = default);
}
