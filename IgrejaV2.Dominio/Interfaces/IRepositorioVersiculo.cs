namespace IgrejaV2.Dominio.Interfaces
{
    using Entidades;

    public interface IRepositorioVersiculo : IRepositorio<Versiculo>
    {
        Task<Versiculo?> ObterPorLivroCaptituloNumeroAsync(int livro, int capitulo, int numero, int traducaoId);
        Task<IEnumerable<Versiculo>> ObterPorLivroAsync(int livro, int traducaoId);
        Task<IEnumerable<Versiculo>> ObterPorLivroCaptituloAsync(int livro, int capitulo, int traducaoId);
    }
}
