namespace IgrejaV2.Dominio.Interfaces
{
    public interface IEmailServico
    {
        Task EnviarRecuperacaoSenhaAsync(string email, string link, CancellationToken ct = default);
    }
}
