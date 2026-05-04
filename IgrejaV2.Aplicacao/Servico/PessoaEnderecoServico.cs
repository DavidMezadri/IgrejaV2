using IgrejaV2.Aplicacao.DTOs.Enderecos;
using IgrejaV2.Aplicacao.DTOs.Pessoas;
using IgrejaV2.Aplicacao.DTOs.PessoasEnderecos;
using IgrejaV2.Dominio.Entidades;
using IgrejaV2.Dominio.Enums;
using IgrejaV2.Dominio.Interfaces;

namespace IgrejaV2.Aplicacao.Servico;

public class PessoaEnderecoServico(
    IRepositorioPessoaEndereco repositorio,
    IRepositorioPessoa repositorioPessoa,
    IRepositorioEndereco repositorioEndereco,
    LogServico logServico)
{
    public async Task<PessoaEnderecoResponseDto> VincularAsync(CriarPessoaEnderecoDto dto, CancellationToken ct = default)
    {
        var pessoaExiste = await repositorioPessoa.ExisteAsync(p => p.Id == dto.PessoaId, ct);
        if (!pessoaExiste)
            throw new InvalidOperationException($"Pessoa com ID {dto.PessoaId} não encontrada.");

        var enderecoExiste = await repositorioEndereco.ExisteAsync(e => e.Id == dto.EnderecoId, ct);
        if (!enderecoExiste)
            throw new InvalidOperationException($"Endereço com ID {dto.EnderecoId} não encontrado.");

        var pessoaEndereco = new PessoaEndereco
        {
            EnderecoId = dto.EnderecoId,
            PessoaId = dto.PessoaId
        };

        await repositorio.AdicionarAsync(pessoaEndereco, ct);
        await repositorio.SalvarAlteracoesAsync(ct);

        var pessoaEnderecoCompleto = await repositorio.ObterPorIdAsync(pessoaEndereco.Id, ct);

        await logServico.RegistrarAsync(
            AcaoLogEnum.Criacao,
            nameof(PessoaEndereco),
            pessoaEndereco.Id,
            descricao: $"Endereço {dto.EnderecoId} vinculado à pessoa {dto.PessoaId}",
            dadosNovos: ToDto(pessoaEnderecoCompleto!),
            ct: ct);

        return ToDto(pessoaEnderecoCompleto!);
    }

    public async Task<IEnumerable<PessoaEnderecoResponseDto>> ObterEnderecosPorPessoaAsync(int pessoaId, CancellationToken ct = default)
    {
        var pessoaExiste = await repositorioPessoa.ExisteAsync(p => p.Id == pessoaId, ct);
        if (!pessoaExiste)
            throw new InvalidOperationException($"Pessoa com ID {pessoaId} não encontrada.");

        var pessoasEnderecos = await repositorio.ObterPorPessoaAsync(pessoaId, ct);
        return pessoasEnderecos.Select(ToDto);
    }

    public async Task<bool> DesvincularAsync(int pessoaEnderecoId, CancellationToken ct = default)
    {
        var pessoaEndereco = await repositorio.ObterPorIdAsync(pessoaEnderecoId, ct);
        if (pessoaEndereco is null) return false;

        var pessoaEnderecoDados = ToDto(pessoaEndereco);

        await repositorio.RemoverPorIdAsync(pessoaEnderecoId, ct);
        await repositorio.SalvarAlteracoesAsync(ct);

        await logServico.RegistrarAsync(
            AcaoLogEnum.Delecao,
            nameof(PessoaEndereco),
            pessoaEnderecoId,
            descricao: $"Vinculação de endereço {pessoaEndereco.EnderecoId} da pessoa {pessoaEndereco.PessoaId} removida",
            dadosAnteriores: pessoaEnderecoDados,
            ct: ct);

        return true;
    }

    private static PessoaEnderecoResponseDto ToDto(PessoaEndereco pe) => new()
    {
        Id = pe.Id,
        EnderecoId = pe.EnderecoId,
        PessoaId = pe.PessoaId,
        Endereco = pe.Endereco is not null ? new EnderecoResponseDto
        {
            Id = pe.Endereco.Id,
            Rua = pe.Endereco.Rua ?? string.Empty,
            Complemento = pe.Endereco.Complemento,
            Numero = pe.Endereco.Numero ?? string.Empty,
            Bairro = pe.Endereco.Bairro ?? string.Empty,
            Cidade = pe.Endereco.Cidade ?? string.Empty,
            Estado = pe.Endereco.Estado ?? string.Empty,
            Cep = pe.Endereco.Cep ?? string.Empty,
            DataCriacao = pe.Endereco.DataCriacao
        } : null,
        Pessoa = pe.Pessoa is not null ? new PessoaResponseDto
        {
            Id = pe.Pessoa.Id,
            Nome = pe.Pessoa.Nome,
            DataNascimento = pe.Pessoa.DataNascimento,
            Sexo = pe.Pessoa.Sexo,
            Email = pe.Pessoa.Email,
            Telefone = pe.Pessoa.Telefone,
            DataBatismo = pe.Pessoa.DataBatismo,
            MembroDesde = pe.Pessoa.MembroDesde,
            EstadoCivil = pe.Pessoa.EstadoCivil,
            Observacoes = pe.Pessoa.Observacoes,
            FamiliaId = pe.Pessoa.FamiliaId,
            Ativo = pe.Pessoa.Ativo,
            DataCriacao = pe.Pessoa.DataCriacao
        } : null,
        DataCriacao = pe.DataCriacao
    };
}
