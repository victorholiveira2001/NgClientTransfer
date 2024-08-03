using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NgClientTransfer.Services
{
    internal interface IServiceManagerService
    {
        public void IniciarServico();
        public void PararServico();
        public void ReiniciarServico();
    }
}
