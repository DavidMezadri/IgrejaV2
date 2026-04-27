using IgrejaV2.Dominio.Entidades;
using IgrejaV2.Dominio.Interfaces;
using IgrejaV2.Infraestrutura.Contexto;
using IgrejaV2.Infraestrutura.Repositorios.Base;
using Microsoft.EntityFrameworkCore;

namespace IgrejaV2.Infraestrutura.Repositorios
{
    public class RepositorioUsuario : RepositorioBase<Usuario>, IRepositorioUsuario
    {
        public RepositorioUsuario(IgrejaContexto contexto) : base(contexto) { }

        public async Task<Usuario?> ObterPorNomeUsuarioAsync(string nomeUsuario, CancellationToken ct = default)
            => await _contexto.Usuarios.FirstOrDefaultAsync(u => u.NomeUsuario == nomeUsuario, ct);

        public async Task<Usuario?> ObterPorEmailAsync(string email, CancellationToken ct = default)
            => await _contexto.Usuarios.FirstOrDefaultAsync(u => u.Email == email && !u.Deletado, ct);

        public async Task<Usuario?> ObterPorTokenRecuperacaoAsync(string token, CancellationToken ct = default)
            => await _contexto.Usuarios.FirstOrDefaultAsync(u => u.TokenRecuperacaoSenha == token && !u.Deletado, ct);
    }
}
