using System.Net;
using System.Net.Mail;
namespace NgClientTransfer.Services
{
    public class EmailService : IEmailService
    {
        private string Email { get; set; }
        private string Senha { get; set; }
        private string Host { get; set; }
        private int Port { get; set; }

        public EmailService()
        {            
            var smtpConfig = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("smtpsettings.json", optional: false, reloadOnChange: true)
                .Build();
            
            Email = smtpConfig["SMTP:Email"] ?? throw new ArgumentNullException(nameof(Email));
            Senha = smtpConfig["SMTP:Senha"] ?? throw new ArgumentNullException(nameof(Senha));
            Host = smtpConfig["SMTP:Host"] ?? throw new ArgumentNullException(nameof(Host));
            Port = int.TryParse(smtpConfig["SMTP:Port"], out var port) ? port : throw new ArgumentNullException(nameof(Port));
        }

        public void DisparaEmail(string assunto, string conteudo)
        {
            var smtpClient = new SmtpClient(Host, Port)
            {
                Credentials = new NetworkCredential(Email, Senha),
                EnableSsl = true
            };

            var emails = new List<string>()
            {
                "victorholiveira2001+ngclienttransfer@gmail.com"
                //"osmar.oliveira@moreira.com.br",
                //"mateus.araujo@moreira.com.br"
            };

            foreach (var email in emails)
            {
                var mailMessage = new MailMessage()
                {
                    From = new MailAddress(Email),
                    Subject = assunto,
                    Body = conteudo,
                    IsBodyHtml = false
                };

                mailMessage.To.Add(email);
                smtpClient.Send(mailMessage);
            }
        }
    }
}
