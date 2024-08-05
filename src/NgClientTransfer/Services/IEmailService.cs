using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NgClientTransfer.Services
{
    public interface IEmailService
    {
        public void DisparaEmail(string conteudo, List<string> emails);
    }
}
