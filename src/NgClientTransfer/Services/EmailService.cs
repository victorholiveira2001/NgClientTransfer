using System.Configuration;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
namespace NgClientTransfer.Services
{
    public class EmailService : IEmailService
    {
        private string Email { get; set; }
        private string Senha { get; set; }
        private string Host { get; set; }
        private int Port { get; set; }


        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
            
            var smtpConfig = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("smtpsettings.json", optional: false, reloadOnChange: true)
                .Build();
            
            Email = smtpConfig["SMTP:Email"] ?? throw new ArgumentNullException(nameof(Email));
            Senha = smtpConfig["SMTP:Senha"] ?? throw new ArgumentNullException(nameof(Senha));
            Host = smtpConfig["SMTP:Host"] ?? throw new ArgumentNullException(nameof(Host));
            Port = int.TryParse(smtpConfig["SMTP:Port"], out var port) ? port : throw new ArgumentNullException(nameof(Port));
        }
        public void DisparaEmail(string conteudo, List<string> emails)
        {
            var smtpClient = new SmtpClient(Host, Port)
            {
                Credentials = new NetworkCredential(Email, Senha),
                EnableSsl = true
            };

            foreach (var email in emails)
            {
                var mailMessage = new MailMessage()
                {
                    From = new MailAddress(Email),
                    Subject = "Falha geracao arquivos NeoGrid",
                    Body = conteudo,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(email);
                smtpClient.Send(mailMessage);
            }
        }
    }
}
