using Xunit;
using IgrejaV2.Aplicacao.DTOs.Familias;
using IgrejaV2.Aplicacao.Servico;
using IgrejaV2.Testes.Fakes;

namespace IgrejaV2.Testes.Servicos;

public class FamiliaServicoTests
{
    private static FamiliaServico CriarServico(out RepositorioFamiliaEmMemoria repositorio)
    {
        repositorio = new RepositorioFamiliaEmMemoria();
        var logServico = new LogServico(new RepositorioLogEmMemoria());
        return new FamiliaServico(repositorio, logServico);
    }

    [Fact]
    public async Task CriarAsync_DeveRetornarFamiliaComIdAtribuido()
    {
        var servico = CriarServico(out _);

        var resultado = await servico.CriarAsync(new CriarFamiliaDto
        {
            Nome = "Silva"
        });

        Assert.True(resultado.Id > 0);
        Assert.Equal("Silva", resultado.Nome);
    }

    [Fact]
    public async Task CriarAsync_ComNomeDuplicado_DeveThrowException()
    {
        var servico = CriarServico(out _);
        await servico.CriarAsync(new CriarFamiliaDto { Nome = "Silva" });

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            servico.CriarAsync(new CriarFamiliaDto { Nome = "Silva" })
        );

        Assert.Contains("já existente", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task ListarTodosAsync_SemFamilias_DeveRetornarListaVazia()
    {
        var servico = CriarServico(out _);

        var resultado = await servico.ListarTodosAsync();

        Assert.Empty(resultado);
    }

    [Fact]
    public async Task ListarTodosAsync_ComFamilias_DeveRetornarTodas()
    {
        var servico = CriarServico(out _);
        await servico.CriarAsync(new CriarFamiliaDto { Nome = "Silva" });
        await servico.CriarAsync(new CriarFamiliaDto { Nome = "Santos" });

        var resultado = await servico.ListarTodosAsync();

        Assert.Equal(2, resultado.Count());
    }

    [Fact]
    public async Task ObterPorIdAsync_FamiliaExistente_DeveRetornarDto()
    {
        var servico = CriarServico(out _);
        var criada = await servico.CriarAsync(new CriarFamiliaDto { Nome = "Silva" });

        var resultado = await servico.ObterPorIdAsync(criada.Id);

        Assert.NotNull(resultado);
        Assert.Equal(criada.Id, resultado.Id);
        Assert.Equal("Silva", resultado.Nome);
    }

    [Fact]
    public async Task ObterPorIdAsync_FamiliaInexistente_DeveRetornarNull()
    {
        var servico = CriarServico(out _);

        var resultado = await servico.ObterPorIdAsync(999);

        Assert.Null(resultado);
    }

    [Fact]
    public async Task AtualizarAsync_FamiliaExistente_DeveRefletirAlteracoes()
    {
        var servico = CriarServico(out _);
        var criada = await servico.CriarAsync(new CriarFamiliaDto { Nome = "Silva" });

        var resultado = await servico.AtualizarAsync(criada.Id, new AtualizarFamiliaDto
        {
            Nome = "Silva Santos",
            Ativo = true
        });

        Assert.NotNull(resultado);
        Assert.Equal("Silva Santos", resultado.Nome);
    }

    [Fact]
    public async Task RemoverAsync_FamiliaExistente_DeveDesativarERetornarTrue()
    {
        var servico = CriarServico(out _);
        var criada = await servico.CriarAsync(new CriarFamiliaDto { Nome = "Silva" });

        var removido = await servico.RemoverAsync(criada.Id);

        Assert.True(removido);
    }

    [Fact]
    public async Task RemoverAsync_FamiliaInexistente_DeveRetornarFalse()
    {
        var servico = CriarServico(out _);

        var resultado = await servico.RemoverAsync(999);

        Assert.False(resultado);
    }

    [Fact]
    public async Task BuscarPorNomeAsync_SemParametro_DeveRetornarListaVazia()
    {
        var servico = CriarServico(out _);
        await servico.CriarAsync(new CriarFamiliaDto { Nome = "Silva" });

        var resultado = await servico.BuscarPorNomeAsync("", default);

        Assert.Empty(resultado);
    }

    [Fact]
    public async Task BuscarPorNomeAsync_ComNomeExato_DeveRetornarFamilia()
    {
        var servico = CriarServico(out _);
        await servico.CriarAsync(new CriarFamiliaDto { Nome = "Silva" });

        var resultado = await servico.BuscarPorNomeAsync("Silva", default);

        Assert.Single(resultado);
        Assert.Equal("Silva", resultado.First().Nome);
    }

    [Fact]
    public async Task BuscarPorNomeAsync_ComParcialNome_DeveRetornarFamilias()
    {
        var servico = CriarServico(out _);
        await servico.CriarAsync(new CriarFamiliaDto { Nome = "Silva" });
        await servico.CriarAsync(new CriarFamiliaDto { Nome = "Silva Santos" });
        await servico.CriarAsync(new CriarFamiliaDto { Nome = "Santos" });

        var resultado = await servico.BuscarPorNomeAsync("Silva", default);

        Assert.Equal(2, resultado.Count());
        Assert.All(resultado, f => Assert.Contains("Silva", f.Nome));
    }

    [Fact]
    public async Task BuscarPorNomeAsync_CaseInsensitive_DeveRetornarFamilias()
    {
        var servico = CriarServico(out _);
        await servico.CriarAsync(new CriarFamiliaDto { Nome = "Silva" });

        var resultadoMaiuscula = await servico.BuscarPorNomeAsync("SILVA", default);
        var resultadoMinuscula = await servico.BuscarPorNomeAsync("silva", default);

        Assert.Single(resultadoMaiuscula);
        Assert.Single(resultadoMinuscula);
    }

    [Fact]
    public async Task BuscarPorNomeAsync_SemResultados_DeveRetornarListaVazia()
    {
        var servico = CriarServico(out _);
        await servico.CriarAsync(new CriarFamiliaDto { Nome = "Silva" });

        var resultado = await servico.BuscarPorNomeAsync("Inexistente", default);

        Assert.Empty(resultado);
    }

    [Fact]
    public async Task BuscarPorNomeAsync_DeveRetornarOrdenadoPorNome()
    {
        var servico = CriarServico(out _);
        await servico.CriarAsync(new CriarFamiliaDto { Nome = "Zebra" });
        await servico.CriarAsync(new CriarFamiliaDto { Nome = "Alpha" });
        await servico.CriarAsync(new CriarFamiliaDto { Nome = "Beta" });

        var resultado = await servico.BuscarPorNomeAsync("a", default);

        var nomes = resultado.Select(f => f.Nome).ToList();
        Assert.Equal(["Alpha", "Beta", "Zebra"], nomes);
    }
}
