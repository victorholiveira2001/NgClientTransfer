using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NgClientTransfer.Services
{
    public interface IServiceManagerService
    {
        public void IniciarServico();
        public void PararServico();
        public void EncerrarHost();
    }
}
