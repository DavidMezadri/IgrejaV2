using Xunit;
using IgrejaV2.Aplicacao.DTOs.Usuarios;
using IgrejaV2.Aplicacao.Servico;
using IgrejaV2.Dominio.Enums;
using IgrejaV2.Testes.Fakes;

namespace IgrejaV2.Testes.Servicos;

public class UsuarioServicoTests
{
    private static UsuarioServico CriarServico(out RepositorioUsuarioEmMemoria repositorio)
    {
        repositorio = new RepositorioUsuarioEmMemoria();
        return new UsuarioServico(repositorio);
    }

    [Fact]
    public async Task CriarAsync_DeveRetornarUsuarioComIdAtribuido()
    {
        var servico = CriarServico(out _);

        var resultado = await servico.CriarAsync(new CriarUsuarioDto
        {
            NomeUsuario = "joao",
            Senha = "senha123",
            TipoUsuario = TipoUsuarioEnum.Membro
        });

        Assert.True(resultado.Id > 0);
        Assert.Equal("joao", resultado.NomeUsuario);
        Assert.True(resultado.PrimeiroAcesso);
    }

    [Fact]
    public async Task CriarAsync_DoisUsuarios_DevemTerIdsDistintos()
    {
        var servico = CriarServico(out _);

        var u1 = await servico.CriarAsync(new CriarUsuarioDto { NomeUsuario = "joao", Senha = "a" });
        var u2 = await servico.CriarAsync(new CriarUsuarioDto { NomeUsuario = "maria", Senha = "b" });

        Assert.NotEqual(u1.Id, u2.Id);
    }

    [Fact]
    public async Task ObterPorIdAsync_UsuarioExistente_DeveRetornarDto()
    {
        var servico = CriarServico(out _);
        var criado = await servico.CriarAsync(new CriarUsuarioDto { NomeUsuario = "joao", Senha = "a" });

        var resultado = await servico.ObterPorIdAsync(criado.Id);

        Assert.NotNull(resultado);
        Assert.Equal(criado.Id, resultado.Id);
    }

    [Fact]
    public async Task ObterPorIdAsync_UsuarioInexistente_DeveRetornarNull()
    {
        var servico = CriarServico(out _);

        var resultado = await servico.ObterPorIdAsync(99);

        Assert.Null(resultado);
    }

    [Fact]
    public async Task ListarTodosAsync_SemUsuarios_DeveRetornarListaVazia()
    {
        var servico = CriarServico(out _);

        var resultado = await servico.ListarTodosAsync();

        Assert.Empty(resultado);
    }

    [Fact]
    public async Task ListarTodosAsync_ComUsuariosCriados_DeveRetornarTodos()
    {
        var servico = CriarServico(out _);
        await servico.CriarAsync(new CriarUsuarioDto { NomeUsuario = "joao", Senha = "a" });
        await servico.CriarAsync(new CriarUsuarioDto { NomeUsuario = "maria", Senha = "b" });

        var resultado = await servico.ListarTodosAsync();

        Assert.Equal(2, resultado.Count());
    }

    [Fact]
    public async Task AtualizarAsync_UsuarioExistente_DeveRefletirAlteracoes()
    {
        var servico = CriarServico(out _);
        var criado = await servico.CriarAsync(new CriarUsuarioDto
        {
            NomeUsuario = "joao",
            Senha = "a",
            TipoUsuario = TipoUsuarioEnum.Membro
        });

        var resultado = await servico.AtualizarAsync(criado.Id, new AtualizarUsuarioDto
        {
            NomeUsuario = "joao.silva",
            TipoUsuario = TipoUsuarioEnum.Pastor
        });

        Assert.NotNull(resultado);
        Assert.Equal("joao.silva", resultado.NomeUsuario);
        Assert.Equal(TipoUsuarioEnum.Pastor, resultado.TipoUsuario);
    }

    [Fact]
    public async Task AtualizarAsync_UsuarioInexistente_DeveRetornarNull()
    {
        var servico = CriarServico(out _);

        var resultado = await servico.AtualizarAsync(99, new AtualizarUsuarioDto { NomeUsuario = "x" });

        Assert.Null(resultado);
    }

    [Fact]
    public async Task RemoverAsync_UsuarioExistente_DeveRemoverERetornarTrue()
    {
        var servico = CriarServico(out _);
        var criado = await servico.CriarAsync(new CriarUsuarioDto { NomeUsuario = "joao", Senha = "a" });

        var removido = await servico.RemoverAsync(criado.Id);
        var aposRemocao = await servico.ListarTodosAsync();

        Assert.True(removido);
        Assert.Empty(aposRemocao);
    }

    [Fact]
    public async Task RemoverAsync_UsuarioInexistente_DeveRetornarFalse()
    {
        var servico = CriarServico(out _);

        var resultado = await servico.RemoverAsync(99);

        Assert.False(resultado);
    }
}
