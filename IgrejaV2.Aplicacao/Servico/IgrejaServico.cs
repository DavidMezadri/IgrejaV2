using IgrejaV2.Aplicacao.DTOs.Igreja;
using IgrejaV2.Dominio.Entidades;
using IgrejaV2.Dominio.Enums;
using IgrejaV2.Dominio.Interfaces;

namespace IgrejaV2.Aplicacao.Servico;

public class IgrejaServico(IRepositorioIgreja repositorio, LogServico logServico)
{
    public async Task<IgrejaResponseDto> CriarAsync(CriarIgrejaDto dto, CancellationToken ct = default)
    {
        var nomeJaExiste = await repositorio.ExisteAsync(u => u.Nome == dto.Nome && u.Ativa, ct);

        if (nomeJaExiste)
            throw new InvalidOperationException("Igreja já existente.");

        var igreja = new Igreja
        {
            Nome = dto.Nome,
            Cnpj = dto.Cnpj,
            Telefone = dto.Telefone,
            Email = dto.Email,
            Lema = dto.Lema,
            EnderecoId = dto.EnderecoId,
            DataFundacao = dto.DataFundacao,
            Observacoes = dto.Observacoes,
            Ativa = true
        };

        await repositorio.AdicionarAsync(igreja, ct);
        await repositorio.SalvarAlteracoesAsync(ct);

        var igrejaDto = ToDto(igreja);

        await logServico.RegistrarAsync(
            AcaoLogEnum.Criacao,
            nameof(Igreja),
            igreja.Id,
            descricao: $"Igreja criada: {igreja.Nome}",
            dadosNovos: igrejaDto,
            ct: ct);

        return igrejaDto;
    }

    public async Task<IgrejaResponseDto?> ObterPorIdAsync(int id, CancellationToken ct = default)
    {
        var igreja = await repositorio.ObterPorIdAsync(id, ct);
        return igreja is null ? null : ToDto(igreja);
    }

    public async Task<IEnumerable<IgrejaResponseDto>> ListarTodosAsync(CancellationToken ct = default)
    {
        var igrejas = await repositorio.ListarTodosAsync(ct);
        return igrejas.Select(ToDto);
    }

    public async Task<IgrejaResponseDto?> AtualizarAsync(int id, AtualizarIgrejaDto dto, CancellationToken ct = default)
    {
        var igreja = await repositorio.ObterPorIdAsync(id, ct);
        if (igreja is null) return null;

        var nomeJaExiste = await repositorio.ExisteAsync(u => u.Nome == dto.Nome && u.Ativa, ct);

        if (nomeJaExiste && dto.Nome != igreja.Nome)
            throw new InvalidOperationException("Igreja já existente.");

        var igrejaAntes = ToDto(igreja);

        igreja.Nome = dto.Nome;
        igreja.Cnpj = dto.Cnpj;
        igreja.Telefone = dto.Telefone;
        igreja.Email = dto.Email;
        igreja.Lema = dto.Lema;
        igreja.EnderecoId = dto.EnderecoId;
        igreja.DataFundacao = dto.DataFundacao;
        igreja.Ativa = dto.Ativa;
        igreja.Observacoes = dto.Observacoes;
        igreja.DataAtualizacao = DateTime.UtcNow;

        await repositorio.AtualizarAsync(igreja, ct);
        await repositorio.SalvarAlteracoesAsync(ct);

        var igrejaDepois = ToDto(igreja);

        await logServico.RegistrarAsync(
            AcaoLogEnum.Edicao,
            nameof(Igreja),
            igreja.Id,
            descricao: $"Igreja atualizada: {igreja.Nome}",
            dadosAnteriores: igrejaAntes,
            dadosNovos: igrejaDepois,
            ct: ct);

        return igrejaDepois;
    }

    public async Task<bool> RemoverAsync(int id, CancellationToken ct = default)
    {
        var igreja = await repositorio.ObterPorIdAsync(id, ct);
        if (igreja is null) return false;

        var igrejaAntes = ToDto(igreja);

        await repositorio.RemoverAsync(igreja, ct);
        await repositorio.SalvarAlteracoesAsync(ct);

        await logServico.RegistrarAsync(
            AcaoLogEnum.Delecao,
            nameof(Igreja),
            id,
            descricao: $"Igreja removida: {igreja.Nome}",
            dadosAnteriores: igrejaAntes,
            ct: ct);

        return true;
    }

    private static IgrejaResponseDto ToDto(Igreja igreja) =>
        new()
        {
            Id = igreja.Id,
            Nome = igreja.Nome,
            Cnpj = igreja.Cnpj,
            Telefone = igreja.Telefone,
            Email = igreja.Email,
            Lema = igreja.Lema,
            EnderecoId = igreja.EnderecoId,
            DataFundacao = igreja.DataFundacao,
            Ativa = igreja.Ativa,
            Observacoes = igreja.Observacoes
        };
}
