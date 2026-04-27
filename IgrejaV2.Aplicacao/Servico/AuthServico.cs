using System.Security.Cryptography;
using IgrejaV2.Aplicacao.DTOs.Auth;
using IgrejaV2.Dominio.Entidades;
using IgrejaV2.Dominio.Interfaces;
using BC = BCrypt.Net.BCrypt;

namespace IgrejaV2.Aplicacao.Servico;

public class AuthServico(IRepositorioUsuario repositorio)
{
    public async Task<Usuario?> ValidarCredenciaisAsync(LoginDto dto, CancellationToken ct = default)
    {
        var usuario = await repositorio.ObterPorNomeUsuarioAsync(dto.NomeUsuario, ct);
        if (usuario is null || !BC.Verify(dto.Senha, usuario.Senha))
            return null;

        usuario.UltimoLogin = DateTime.UtcNow;
        await repositorio.AtualizarAsync(usuario, ct);
        await repositorio.SalvarAlteracoesAsync(ct);

        return usuario;
    }

    public async Task<string?> GerarTokenRecuperacaoAsync(RecuperarSenhaDto dto, CancellationToken ct = default)
    {
        var usuario = await repositorio.ObterPorEmailAsync(dto.Email, ct);
        if (usuario is null)
            return null;

        var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(48))
            .Replace("+", "-").Replace("/", "_").Replace("=", "");

        usuario.TokenRecuperacaoSenha = token;
        usuario.TokenRecuperacaoSenhaExpiracao = DateTime.UtcNow.AddHours(2);
        await repositorio.AtualizarAsync(usuario, ct);
        await repositorio.SalvarAlteracoesAsync(ct);

        return token;
    }

    public async Task<bool> ResetarSenhaAsync(ResetarSenhaDto dto, CancellationToken ct = default)
    {
        var usuario = await repositorio.ObterPorTokenRecuperacaoAsync(dto.Token, ct);
        if (usuario is null || usuario.TokenRecuperacaoSenhaExpiracao < DateTime.UtcNow)
            return false;

        usuario.Senha = BC.HashPassword(dto.NovaSenha);
        usuario.TokenRecuperacaoSenha = null;
        usuario.TokenRecuperacaoSenhaExpiracao = null;
        usuario.DataAtualizacao = DateTime.UtcNow;

        await repositorio.AtualizarAsync(usuario, ct);
        await repositorio.SalvarAlteracoesAsync(ct);

        return true;
    }
}
