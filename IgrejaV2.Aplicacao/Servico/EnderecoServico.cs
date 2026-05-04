using IgrejaV2.Aplicacao.DTOs.Enderecos;
using IgrejaV2.Dominio.Entidades;
using IgrejaV2.Dominio.Enums;
using IgrejaV2.Dominio.Interfaces;

namespace IgrejaV2.Aplicacao.Servico;

public class EnderecoServico(IRepositorioEndereco repositorio, LogServico logServico)
{
    public async Task<EnderecoResponseDto> CriarAsync(CriarEnderecoDto dto, CancellationToken ct = default)
    {
        var endereco = new Endereco
        {
            Rua = dto.Rua,
            Complemento = dto.Complemento,
            Numero = dto.Numero,
            Bairro = dto.Bairro,
            Cidade = dto.Cidade,
            Estado = dto.Estado,
            Cep = dto.Cep
        };

        await repositorio.AdicionarAsync(endereco, ct);
        await repositorio.SalvarAlteracoesAsync(ct);

        await logServico.RegistrarAsync(
            AcaoLogEnum.Criacao,
            nameof(Endereco),
            endereco.Id,
            descricao: $"Endereço criado: {endereco.Rua}, {endereco.Numero}, {endereco.Cidade}",
            dadosNovos: ToDto(endereco),
            ct: ct);

        return ToDto(endereco);
    }

    public async Task<EnderecoResponseDto?> ObterPorIdAsync(int id, CancellationToken ct = default)
    {
        var endereco = await repositorio.ObterPorIdAsync(id, ct);
        return endereco is null ? null : ToDto(endereco);
    }

    public async Task<IEnumerable<EnderecoResponseDto>> ListarTodosAsync(CancellationToken ct = default)
    {
        var enderecos = await repositorio.ListarTodosAsync(ct);
        return enderecos.Select(ToDto);
    }

    public async Task<EnderecoResponseDto?> AtualizarAsync(int id, AtualizarEnderecoDto dto, CancellationToken ct = default)
    {
        var endereco = await repositorio.ObterPorIdAsync(id, ct);
        if (endereco is null) return null;

        var enderecoAntes = ToDto(endereco);

        endereco.Rua = dto.Rua;
        endereco.Complemento = dto.Complemento;
        endereco.Numero = dto.Numero;
        endereco.Bairro = dto.Bairro;
        endereco.Cidade = dto.Cidade;
        endereco.Estado = dto.Estado;
        endereco.Cep = dto.Cep;
        endereco.DataAtualizacao = DateTime.UtcNow;

        await repositorio.AtualizarAsync(endereco, ct);
        await repositorio.SalvarAlteracoesAsync(ct);

        var enderecoDepois = ToDto(endereco);

        await logServico.RegistrarAsync(
            AcaoLogEnum.Edicao,
            nameof(Endereco),
            endereco.Id,
            descricao: $"Endereço atualizado: {endereco.Rua}, {endereco.Numero}, {endereco.Cidade}",
            dadosAnteriores: enderecoAntes,
            dadosNovos: enderecoDepois,
            ct: ct);

        return enderecoDepois;
    }

    public async Task<bool> RemoverAsync(int id, CancellationToken ct = default)
    {
        var endereco = await repositorio.ObterPorIdAsync(id, ct);
        if (endereco is null) return false;

        var enderecoDados = ToDto(endereco);

        await repositorio.RemoverPorIdAsync(id, ct);
        await repositorio.SalvarAlteracoesAsync(ct);

        await logServico.RegistrarAsync(
            AcaoLogEnum.Delecao,
            nameof(Endereco),
            id,
            descricao: $"Endereço removido: {endereco.Rua}, {endereco.Numero}, {endereco.Cidade}",
            dadosAnteriores: enderecoDados,
            ct: ct);

        return true;
    }

    private static EnderecoResponseDto ToDto(Endereco e) => new()
    {
        Id = e.Id,
        Rua = e.Rua ?? string.Empty,
        Complemento = e.Complemento,
        Numero = e.Numero ?? string.Empty,
        Bairro = e.Bairro ?? string.Empty,
        Cidade = e.Cidade ?? string.Empty,
        Estado = e.Estado ?? string.Empty,
        Cep = e.Cep ?? string.Empty,
        DataCriacao = e.DataCriacao
    };
}
