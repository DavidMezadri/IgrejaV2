using IgrejaV2.Aplicacao.DTOs.Usuarios;
using IgrejaV2.Dominio.Entidades;
using IgrejaV2.Dominio.Enums;
using IgrejaV2.Dominio.Interfaces;
using BC = BCrypt.Net.BCrypt;

namespace IgrejaV2.Aplicacao.Servico;

public class UsuarioServico(IRepositorioUsuario repositorio, LogServico logServico)
{
    public async Task<UsuarioResponseDto> CriarAsync(CriarUsuarioDto dto, CancellationToken ct = default)
    {
        var nomeJaExiste = await repositorio.ExisteAsync(u => u.NomeUsuario == dto.NomeUsuario && !u.Deletado, ct);
        if (nomeJaExiste)
            throw new InvalidOperationException("Nome de usuário já está em uso.");

        var emailJaExiste = await repositorio.ExisteAsync(u => u.Email == dto.Email && !u.Deletado, ct);
        if (emailJaExiste)
            throw new InvalidOperationException("E-mail já está cadastrado.");

        var usuario = new Usuario
        {
            NomeUsuario = dto.NomeUsuario,
            Email = dto.Email,
            Senha = BC.HashPassword(dto.Senha),
            TipoUsuario = dto.TipoUsuario,
            PrimeiroAcesso = true
        };

        await repositorio.AdicionarAsync(usuario, ct);
        await repositorio.SalvarAlteracoesAsync(ct);

        var usuarioDto = ToDto(usuario);

        await logServico.RegistrarAsync(
            AcaoLogEnum.Criacao,
            nameof(Usuario),
            usuario.Id,
            descricao: $"Usuário criado: {usuario.NomeUsuario}",
            dadosNovos: usuarioDto,
            ct: ct);

        return usuarioDto;
    }

    public async Task<UsuarioResponseDto?> ObterPorIdAsync(int id, CancellationToken ct = default)
    {
        var usuario = await repositorio.ObterPorIdAsync(id, ct);
        return usuario is null ? null : ToDto(usuario);
    }

    public async Task<IEnumerable<UsuarioResponseDto>> ListarTodosAsync(CancellationToken ct = default)
    {
        var usuarios = await repositorio.ListarTodosAsync(ct);
        return usuarios.Select(ToDto);
    }

    public async Task<UsuarioResponseDto?> AtualizarAsync(int id, AtualizarUsuarioDto dto, CancellationToken ct = default)
    {
        var usuario = await repositorio.ObterPorIdAsync(id, ct);
        if (usuario is null) return null;

        var usuarioAntes = ToDto(usuario);

        usuario.NomeUsuario = dto.NomeUsuario;
        usuario.TipoUsuario = dto.TipoUsuario;
        usuario.DataAtualizacao = DateTime.UtcNow;

        await repositorio.AtualizarAsync(usuario, ct);
        await repositorio.SalvarAlteracoesAsync(ct);

        var usuarioDepois = ToDto(usuario);

        await logServico.RegistrarAsync(
            AcaoLogEnum.Edicao,
            nameof(Usuario),
            usuario.Id,
            descricao: $"Usuário atualizado: {usuario.NomeUsuario}",
            dadosAnteriores: usuarioAntes,
            dadosNovos: usuarioDepois,
            ct: ct);

        return usuarioDepois;
    }

    public async Task<bool> RemoverAsync(int id, CancellationToken ct = default)
    {
        var usuario = await repositorio.ObterPorIdAsync(id, ct);
        if (usuario is null) return false;

        var usuarioDados = ToDto(usuario);

        await repositorio.RemoverPorIdAsync(id, ct);
        await repositorio.SalvarAlteracoesAsync(ct);

        await logServico.RegistrarAsync(
            AcaoLogEnum.Delecao,
            nameof(Usuario),
            id,
            descricao: $"Usuário removido: {usuario.NomeUsuario}",
            dadosAnteriores: usuarioDados,
            ct: ct);

        return true;
    }

    private static UsuarioResponseDto ToDto(Usuario u) => new()
    {
        Id = u.Id,
        NomeUsuario = u.NomeUsuario,
        Email = u.Email,
        TipoUsuario = u.TipoUsuario,
        PrimeiroAcesso = u.PrimeiroAcesso,
        UltimoLogin = u.UltimoLogin,
        DataCriacao = u.DataCriacao
    };
}
