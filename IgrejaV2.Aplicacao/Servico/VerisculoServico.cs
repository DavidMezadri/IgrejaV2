using IgrejaV2.Aplicacao.DTOs.Versiculos;
using IgrejaV2.Dominio.Entidades;
using IgrejaV2.Dominio.Enums;
using IgrejaV2.Dominio.Interfaces;

namespace IgrejaV2.Aplicacao.Servico;

public class VerisculoServico(IRepositorioVersiculo repositorio, IRepositorioTraducao repositorioTraducao, LogServico logServico)
{
    public async Task<VericuloResponseDto> CriarAsync(CriarVericuloDto dto, CancellationToken ct = default)
    {
        var versiculo = new Versiculo
        {
            Livro = dto.Livro,
            Capitulo = dto.Capitulo,
            Numero = dto.Numero,
            Texto = dto.Texto,
            TraducaoId = dto.TraducaoId
        };

        await repositorio.AdicionarAsync(versiculo, ct);
        await repositorio.SalvarAlteracoesAsync(ct);

        var vericuloDto = await ToDtoAsync(versiculo, ct);

        await logServico.RegistrarAsync(
            AcaoLogEnum.Criacao,
            nameof(Versiculo),
            versiculo.Id,
            descricao: $"Versículo criado: Livro {versiculo.Livro} {versiculo.Capitulo}:{versiculo.Numero}",
            dadosNovos: vericuloDto,
            ct: ct);

        return vericuloDto;
    }

    public async Task<VericuloResponseDto?> ObterPorIdAsync(int id, CancellationToken ct = default)
    {
        var versiculo = await repositorio.ObterPorIdAsync(id, ct);
        return versiculo is null ? null : await ToDtoAsync(versiculo, ct);
    }

    public async Task<IEnumerable<VericuloResponseDto>> ListarTodosAsync(CancellationToken ct = default)
    {
        var versiculos = await repositorio.ListarTodosAsync(ct);
        var lista = new List<VericuloResponseDto>();
        foreach (var v in versiculos)
            lista.Add(await ToDtoAsync(v, ct));
        return lista;
    }

    public async Task<VericuloResponseDto?> AtualizarAsync(int id, AtualizarVericuloDto dto, CancellationToken ct = default)
    {
        var versiculo = await repositorio.ObterPorIdAsync(id, ct);
        if (versiculo is null) return null;

        var vericuloAntes = await ToDtoAsync(versiculo, ct);

        versiculo.Livro = dto.Livro;
        versiculo.Capitulo = dto.Capitulo;
        versiculo.Numero = dto.Numero;
        versiculo.Texto = dto.Texto;
        versiculo.TraducaoId = dto.TraducaoId;
        versiculo.DataAtualizacao = DateTime.UtcNow;

        await repositorio.AtualizarAsync(versiculo, ct);
        await repositorio.SalvarAlteracoesAsync(ct);

        var vericuloDepois = await ToDtoAsync(versiculo, ct);

        await logServico.RegistrarAsync(
            AcaoLogEnum.Edicao,
            nameof(Versiculo),
            versiculo.Id,
            descricao: $"Versículo atualizado: Livro {versiculo.Livro} {versiculo.Capitulo}:{versiculo.Numero}",
            dadosAnteriores: vericuloAntes,
            dadosNovos: vericuloDepois,
            ct: ct);

        return vericuloDepois;
    }

    public async Task<bool> RemoverAsync(int id, CancellationToken ct = default)
    {
        var versiculo = await repositorio.ObterPorIdAsync(id, ct);
        if (versiculo is null) return false;

        var vericuloDados = await ToDtoAsync(versiculo, ct);

        await repositorio.RemoverPorIdAsync(id, ct);
        await repositorio.SalvarAlteracoesAsync(ct);

        await logServico.RegistrarAsync(
            AcaoLogEnum.Delecao,
            nameof(Versiculo),
            id,
            descricao: $"Versículo removido: Livro {versiculo.Livro} {versiculo.Capitulo}:{versiculo.Numero}",
            dadosAnteriores: vericuloDados,
            ct: ct);

        return true;
    }

    public async Task<IEnumerable<VericuloResponseDto>> ObterPorLivroAsync(int livro, int traducaoId, CancellationToken ct = default)
    {
        var versiculos = await repositorio.ObterPorLivroAsync(livro, traducaoId);
        var lista = new List<VericuloResponseDto>();
        foreach (var v in versiculos)
            lista.Add(await ToDtoAsync(v, ct));
        return lista;
    }

    public async Task<IEnumerable<VericuloResponseDto>> ObterPorLivroCaptituloAsync(int livro, int capitulo, int traducaoId, CancellationToken ct = default)
    {
        var versiculos = await repositorio.ObterPorLivroCaptituloAsync(livro, capitulo, traducaoId);
        var lista = new List<VericuloResponseDto>();
        foreach (var v in versiculos)
            lista.Add(await ToDtoAsync(v, ct));
        return lista;
    }

    private async Task<VericuloResponseDto> ToDtoAsync(Versiculo v, CancellationToken ct = default)
    {
        var traducao = await repositorioTraducao.ObterPorIdAsync(v.TraducaoId, ct);
        return new VericuloResponseDto
        {
            Id = v.Id,
            Livro = v.Livro,
            Capitulo = v.Capitulo,
            Numero = v.Numero,
            Texto = v.Texto,
            TraducaoId = v.TraducaoId,
            TraducaoAbreviacao = traducao?.Abreviacao,
            DataCriacao = v.DataCriacao
        };
    }
}
