using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;

namespace NgClientTransfer.Services
{
    internal class EmailService : IEmailService
    {
        private string Email { get; set; }
        private string Senha { get; set; }
        private string Host { get; set; }
        private int Port { get; set; }


        private readonly Configuration _configuration;

        public EmailService(Configuration configuration)
        {
            _configuration = configuration;
        }
        public void DisparaEmail(string conteudo, List<string> emails)
        {
            _configuration.GetSectionGroup("EmailSettings");

             

            var smtpClient = new SmtpClient(Host, Port);
            smtpClient.Credentials = new NetworkCredential(Email, Senha);
        }
    }
}
