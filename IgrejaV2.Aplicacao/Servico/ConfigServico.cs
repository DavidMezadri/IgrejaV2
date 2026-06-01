using IgrejaV2.Aplicacao.DTOs.Config;
using IgrejaV2.Dominio.Interfaces;

namespace IgrejaV2.Aplicacao.Servico;

public class ConfigServico(IRepositorioConfig repositorio) : IConfigServico
{
    public async Task<List<ConfigDto>> ObterTodasAsync(CancellationToken ct = default)
    {
        var configs = await repositorio.ObterTodasAsync(ct);
        return configs.Select(c => new ConfigDto
        {
            Chave = c.Chave,
            Valor = c.Valor
        }).ToList();
    }

    public async Task AtualizarAsync(List<ConfigDto> dtos, CancellationToken ct = default)
    {
        if (dtos is null || dtos.Count == 0)
            throw new ArgumentException("Nenhuma configuração para atualizar");

        foreach (var dto in dtos)
        {
            if (string.IsNullOrWhiteSpace(dto.Chave))
                throw new ArgumentException("Chave não pode ser vazia");

            await repositorio.AtualizarOuCriarAsync(dto.Chave, dto.Valor, ct);
        }
    }
}
