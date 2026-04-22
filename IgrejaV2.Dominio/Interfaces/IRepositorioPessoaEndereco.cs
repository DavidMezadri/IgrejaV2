using IgrejaV2.Dominio.Entidades;

namespace IgrejaV2.Dominio.Interfaces
{
    public interface IRepositorioPessoaEndereco : IRepositorio<PessoaEndereco>
    {
        Task<IEnumerable<PessoaEndereco>> ObterPorPessoaAsync(int pessoaId, CancellationToken ct = default);
    }
}
