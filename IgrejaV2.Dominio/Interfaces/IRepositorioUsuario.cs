using IgrejaV2.Dominio.Entidades;

namespace IgrejaV2.Dominio.Interfaces
{
    public interface IRepositorioUsuario : IRepositorio<Usuario>
    {
        Task<Usuario?> ObterPorNomeUsuarioAsync(string nomeUsuario, CancellationToken ct = default);
        Task<Usuario?> ObterPorEmailAsync(string email, CancellationToken ct = default);
        Task<Usuario?> ObterPorTokenRecuperacaoAsync(string token, CancellationToken ct = default);
    }
}
