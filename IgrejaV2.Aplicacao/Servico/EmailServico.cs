using IgrejaV2.Dominio.Interfaces;
using System.Net.Mail;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace IgrejaV2.Aplicacao.Servico
{
    public class EmailServico : IEmailServico
    {
        private readonly EmailConfig _config;

        public EmailServico(EmailConfig config)
        {
            _config = config;
        }

        public async Task EnviarRecuperacaoSenhaAsync(string email, string link, CancellationToken ct = default)
        {
            var mensagem = new MimeMessage();
            mensagem.From.Add(new MailboxAddress(_config.NomeRemetente, _config.EmailRemetente));
            mensagem.To.Add(new MailboxAddress("", email));
            mensagem.Subject = "Recuperação de senha";
            mensagem.Body = new TextPart("html")
            {
                Text = $"<p>Clique no link para redefinir sua senha:</p><a href='{link}'>{link}</a>"
            };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_config.Host, _config.Porta, SecureSocketOptions.StartTls, ct);
            await smtp.AuthenticateAsync(_config.Usuario, _config.Senha, ct);
            await smtp.SendAsync(mensagem, ct);
            await smtp.DisconnectAsync(true, ct);
        }
    }

    public class EmailConfig
    {
        public string Host { get; set; } = string.Empty;
        public int Porta { get; set; }
        public string Usuario { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public string EmailRemetente { get; set; } = string.Empty;
        public string NomeRemetente { get; set; } = string.Empty;
    }
}
