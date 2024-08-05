using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NgClientTransfer.Services
{
    internal interface IManipulaArquivosService
    {
        public void VerificaArquivos();
        public void MovimentaArquivos(string[] arquivos);
        public void ConfereExisteArquivos();
    }
}
