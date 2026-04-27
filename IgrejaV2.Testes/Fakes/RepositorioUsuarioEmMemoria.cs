using IgrejaV2.Dominio.Entidades;
using IgrejaV2.Dominio.Interfaces;

namespace IgrejaV2.Testes.Fakes;

public class RepositorioUsuarioEmMemoria : RepositorioEmMemoria<Usuario>, IRepositorioUsuario
{
    public Task<Usuario?> ObterPorNomeUsuarioAsync(string nomeUsuario, CancellationToken ct = default)
        => Task.FromResult(Dados.FirstOrDefault(u => u.NomeUsuario == nomeUsuario));

    public Task<Usuario?> ObterPorEmailAsync(string email, CancellationToken ct = default)
        => Task.FromResult(Dados.FirstOrDefault(u => u.Email == email && !u.Deletado));

    public Task<Usuario?> ObterPorTokenRecuperacaoAsync(string token, CancellationToken ct = default)
        => Task.FromResult(Dados.FirstOrDefault(u => u.TokenRecuperacaoSenha == token && !u.Deletado));
}
