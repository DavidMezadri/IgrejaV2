using IgrejaV2.Dominio.Entidades;
using IgrejaV2.Dominio.Interfaces;
using IgrejaV2.Infraestrutura.Contexto;
using IgrejaV2.Infraestrutura.Repositorios.Base;
using Microsoft.EntityFrameworkCore;

namespace IgrejaV2.Infraestrutura.Repositorios;

public class RepositorioConfig : RepositorioBase<Configuracao>, IRepositorioConfig
{
    public RepositorioConfig(IgrejaContexto contexto) : base(contexto) { }

    public async Task<Configuracao?> ObterPorChaveAsync(string chave, CancellationToken ct = default)
        => await _dbSet.FirstOrDefaultAsync(c => c.Chave == chave, ct);

    public async Task<IEnumerable<Configuracao>> ObterTodasAsync(CancellationToken ct = default)
        => await _dbSet.OrderBy(c => c.Chave).ToListAsync(ct);

    public async Task AtualizarOuCriarAsync(string chave, string valor, CancellationToken ct = default)
    {
        var config = await ObterPorChaveAsync(chave, ct);

        if (config is null)
        {
            config = new Configuracao
            {
                Chave = chave,
                Valor = valor,
                CriadoEm = DateTime.UtcNow,
                AtualizadoEm = DateTime.UtcNow
            };
            await AdicionarAsync(config, ct);
        }
        else
        {
            config.Valor = valor;
            config.AtualizadoEm = DateTime.UtcNow;
            await AtualizarAsync(config, ct);
        }

        await SalvarAlteracoesAsync(ct);
    }
}
