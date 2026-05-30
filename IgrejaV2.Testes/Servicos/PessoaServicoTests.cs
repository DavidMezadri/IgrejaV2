using Xunit;
using IgrejaV2.Aplicacao.DTOs.Pessoas;
using IgrejaV2.Aplicacao.Servico;
using IgrejaV2.Dominio.Entidades;
using IgrejaV2.Testes.Fakes;

namespace IgrejaV2.Testes.Servicos;

public class PessoaServicoTests
{
    private static PessoaServico CriarServico(
        out RepositorioPessoaEmMemoria repositorioPessoa,
        out RepositorioFamiliaEmMemoria repositorioFamilia)
    {
        repositorioPessoa = new RepositorioPessoaEmMemoria();
        repositorioFamilia = new RepositorioFamiliaEmMemoria();
        var logServico = new LogServico(new RepositorioLogEmMemoria());
        return new PessoaServico(repositorioPessoa, repositorioFamilia, logServico);
    }

    private static CriarPessoaDto CriarPessoaDtoValida(string nome = "João Silva", int? familiaId = null)
    {
        return new CriarPessoaDto
        {
            Nome = nome,
            Email = $"{nome.Replace(" ", "").ToLower()}@example.com",
            DataNascimento = new DateTime(1990, 1, 1),
            Telefone = "11999999999",
            FamiliaId = familiaId
        };
    }

    private static AtualizarPessoaDto AtualizarPessoaDtoValida(string nome = "João Silva", int? familiaId = null)
    {
        return new AtualizarPessoaDto
        {
            Nome = nome,
            Email = $"{nome.Replace(" ", "").ToLower()}@example.com",
            DataNascimento = new DateTime(1990, 1, 1),
            Telefone = "11999999999",
            FamiliaId = familiaId,
            Ativo = true
        };
    }

    [Fact]
    public async Task CriarAsync_SemFamilia_DeveInserirPessoaSucesso()
    {
        var servico = CriarServico(out _, out _);
        var dto = CriarPessoaDtoValida();

        var resultado = await servico.CriarAsync(dto);

        Assert.True(resultado.Id > 0);
        Assert.Equal("João Silva", resultado.Nome);
        Assert.Null(resultado.FamiliaId);
    }

    [Fact]
    public async Task CriarAsync_ComFamiliaValida_DeveInserirPessoaSucesso()
    {
        var servico = CriarServico(out _, out var repositorioFamilia);

        var familia = new Familia { Nome = "Silva", Ativo = true };
        await repositorioFamilia.AdicionarAsync(familia);

        var dto = CriarPessoaDtoValida(familiaId: familia.Id);
        var resultado = await servico.CriarAsync(dto);

        Assert.True(resultado.Id > 0);
        Assert.Equal(familia.Id, resultado.FamiliaId);
    }

    [Fact]
    public async Task CriarAsync_ComFamiliaInvalida_DeveThrowInvalidOperationException()
    {
        var servico = CriarServico(out _, out _);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            servico.CriarAsync(CriarPessoaDtoValida(familiaId: 999))
        );

        Assert.Contains("família", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task AtualizarAsync_RemoverFamilia_DeveAtualizar()
    {
        var servico = CriarServico(out _, out var repositorioFamilia);

        var familia = new Familia { Nome = "Silva", Ativo = true };
        await repositorioFamilia.AdicionarAsync(familia);

        var criada = await servico.CriarAsync(CriarPessoaDtoValida(familiaId: familia.Id));

        var atualizada = await servico.AtualizarAsync(criada.Id, AtualizarPessoaDtoValida(familiaId: null));

        Assert.NotNull(atualizada);
        Assert.Null(atualizada.FamiliaId);
    }

    [Fact]
    public async Task AtualizarAsync_ComFamiliaInvalida_DeveThrowInvalidOperationException()
    {
        var servico = CriarServico(out _, out _);

        var criada = await servico.CriarAsync(CriarPessoaDtoValida());

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            servico.AtualizarAsync(criada.Id, AtualizarPessoaDtoValida(familiaId: 999))
        );

        Assert.Contains("família", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task CriarAsync_DeveRetornarPessoaComIdAtribuido()
    {
        var servico = CriarServico(out _, out _);

        var resultado = await servico.CriarAsync(CriarPessoaDtoValida("Maria Silva"));

        Assert.True(resultado.Id > 0);
        Assert.Equal("Maria Silva", resultado.Nome);
    }

    [Fact]
    public async Task ObterPorIdAsync_PessoaExistente_DeveRetornarDto()
    {
        var servico = CriarServico(out _, out _);
        var criada = await servico.CriarAsync(CriarPessoaDtoValida());

        var resultado = await servico.ObterPorIdAsync(criada.Id);

        Assert.NotNull(resultado);
        Assert.Equal(criada.Id, resultado.Id);
    }

    [Fact]
    public async Task ListarTodosAsync_SemPessoas_DeveRetornarListaVazia()
    {
        var servico = CriarServico(out _, out _);

        var resultado = await servico.ListarTodosAsync();

        Assert.Empty(resultado);
    }

    [Fact]
    public async Task ListarTodosAsync_ComPessoas_DeveRetornarTodas()
    {
        var servico = CriarServico(out _, out _);
        await servico.CriarAsync(CriarPessoaDtoValida("João"));
        await servico.CriarAsync(CriarPessoaDtoValida("Maria"));

        var resultado = await servico.ListarTodosAsync();

        Assert.Equal(2, resultado.Count());
    }

    [Fact]
    public async Task RemoverAsync_PessoaExistente_DeveRetornarTrue()
    {
        var servico = CriarServico(out _, out _);
        var criada = await servico.CriarAsync(CriarPessoaDtoValida());

        var removido = await servico.RemoverAsync(criada.Id);

        Assert.True(removido);
    }
}
