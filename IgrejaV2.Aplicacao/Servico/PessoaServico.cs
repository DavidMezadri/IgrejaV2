using IgrejaV2.Aplicacao.DTOs.Pessoas;
using IgrejaV2.Dominio.Entidades;
using IgrejaV2.Dominio.Enums;
using IgrejaV2.Dominio.Interfaces;

namespace IgrejaV2.Aplicacao.Servico;

public class PessoaServico(IRepositorioPessoa repositorio, LogServico logServico)
{
    public async Task<PessoaResponseDto> CriarAsync(CriarPessoaDto dto, CancellationToken ct = default)
    {
        var pessoa = new Pessoa
        {
            Nome = dto.Nome,
            DataNascimento = dto.DataNascimento,
            Sexo = dto.Sexo,
            Email = dto.Email,
            Telefone = dto.Telefone,
            DataBatismo = dto.DataBatismo,
            MembroDesde = dto.MembroDesde,
            EstadoCivil = dto.EstadoCivil,
            Observacoes = dto.Observacoes,
            FamiliaId = dto.FamiliaId,
            Ativo = true
        };

        await repositorio.AdicionarAsync(pessoa, ct);
        await repositorio.SalvarAlteracoesAsync(ct);

        var pessoaDto = ToDto(pessoa);

        await logServico.RegistrarAsync(
            AcaoLogEnum.Criacao,
            nameof(Pessoa),
            pessoa.Id,
            descricao: $"Pessoa criada: {pessoa.Nome}",
            dadosNovos: pessoaDto,
            ct: ct);

        return pessoaDto;
    }

    public async Task<PessoaResponseDto?> ObterPorIdAsync(int id, CancellationToken ct = default)
    {
        var pessoa = await repositorio.ObterPorIdAsync(id, ct);
        return pessoa is null ? null : ToDto(pessoa);
    }

    public async Task<IEnumerable<PessoaResponseDto>> ListarTodosAsync(CancellationToken ct = default)
    {
        var pessoas = await repositorio.ListarTodosAsync(ct);
        return pessoas.Select(ToDto);
    }

    public async Task<IEnumerable<PessoaResponseDto>> ListarAtivosAsync(CancellationToken ct = default)
    {
        var pessoas = await repositorio.ObterAtivosAsync(ct);
        return pessoas.Select(ToDto);
    }

    public async Task<IEnumerable<PessoaResponseDto>> ListarPorFamiliaAsync(int familiaId, CancellationToken ct = default)
    {
        var pessoas = await repositorio.ObterPorFamiliaAsync(familiaId, ct);
        return pessoas.Select(ToDto);
    }

    public async Task<IEnumerable<PessoaResponseDto>> BuscarPorNomeAsync(string nome, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(nome))
            return Enumerable.Empty<PessoaResponseDto>();

        var pessoas = await repositorio.BuscarPorNomeAsync(nome, ct);
        return pessoas.Select(ToDto);
    }

    public async Task<PessoaResponseDto?> AtualizarAsync(int id, AtualizarPessoaDto dto, CancellationToken ct = default)
    {
        var pessoa = await repositorio.ObterPorIdAsync(id, ct);
        if (pessoa is null) return null;

        var pessoaAntes = ToDto(pessoa);

        pessoa.Nome = dto.Nome;
        pessoa.DataNascimento = dto.DataNascimento;
        pessoa.Sexo = dto.Sexo;
        pessoa.Email = dto.Email;
        pessoa.Telefone = dto.Telefone;
        pessoa.DataBatismo = dto.DataBatismo;
        pessoa.MembroDesde = dto.MembroDesde;
        pessoa.EstadoCivil = dto.EstadoCivil;
        pessoa.Observacoes = dto.Observacoes;
        pessoa.FamiliaId = dto.FamiliaId;
        pessoa.Ativo = dto.Ativo;
        pessoa.DataAtualizacao = DateTime.UtcNow;

        await repositorio.AtualizarAsync(pessoa, ct);
        await repositorio.SalvarAlteracoesAsync(ct);

        var pessoaDepois = ToDto(pessoa);

        await logServico.RegistrarAsync(
            AcaoLogEnum.Edicao,
            nameof(Pessoa),
            pessoa.Id,
            descricao: $"Pessoa atualizada: {pessoa.Nome}",
            dadosAnteriores: pessoaAntes,
            dadosNovos: pessoaDepois,
            ct: ct);

        return pessoaDepois;
    }

    public async Task<bool> RemoverAsync(int id, CancellationToken ct = default)
    {
        var pessoa = await repositorio.ObterPorIdAsync(id, ct);
        if (pessoa is null) return false;

        var pessoaDados = ToDto(pessoa);

        await repositorio.RemoverPorIdAsync(id, ct);
        await repositorio.SalvarAlteracoesAsync(ct);

        await logServico.RegistrarAsync(
            AcaoLogEnum.Delecao,
            nameof(Pessoa),
            id,
            descricao: $"Pessoa removida: {pessoa.Nome}",
            dadosAnteriores: pessoaDados,
            ct: ct);

        return true;
    }

    private static PessoaResponseDto ToDto(Pessoa p) => new()
    {
        Id = p.Id,
        Nome = p.Nome,
        DataNascimento = p.DataNascimento,
        Sexo = p.Sexo,
        Email = p.Email,
        Telefone = p.Telefone,
        DataBatismo = p.DataBatismo,
        MembroDesde = p.MembroDesde,
        EstadoCivil = p.EstadoCivil,
        Observacoes = p.Observacoes,
        FamiliaId = p.FamiliaId,
        Ativo = p.Ativo,
        DataCriacao = p.DataCriacao
    };
}
