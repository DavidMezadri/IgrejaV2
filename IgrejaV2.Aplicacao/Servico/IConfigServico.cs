using IgrejaV2.Aplicacao.DTOs.Config;

namespace IgrejaV2.Aplicacao.Servico;

public interface IConfigServico
{
    Task<List<ConfigDto>> ObterTodasAsync(CancellationToken ct = default);
    Task AtualizarAsync(List<ConfigDto> configs, CancellationToken ct = default);
}
