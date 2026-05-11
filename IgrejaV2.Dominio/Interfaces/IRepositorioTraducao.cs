namespace IgrejaV2.Dominio.Interfaces
{
    using Entidades;

    public interface IRepositorioTraducao : IRepositorio<Traducao>
    {
        Task<Traducao?> ObterPorAbreviacaoAsync(string abreviacao);
    }
}
