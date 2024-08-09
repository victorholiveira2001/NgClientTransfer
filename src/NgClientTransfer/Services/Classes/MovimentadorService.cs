using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.Arm;

namespace NgClientTransfer.Services
{
    class MovimentadorService : IMovimentadorService
    {
        private readonly IProcedureService _procedureService;
        private readonly IVerificadorService _verificadorService;
        private readonly IExceptionService _exceptionService;
        string? Data { get; set; }

        public MovimentadorService(IProcedureService procedureService, IVerificadorService verificadorService, IExceptionService exceptionService)
        {
            _procedureService = procedureService;
            _verificadorService = verificadorService;
            _exceptionService = exceptionService;
        }

        public void GerenciadorDeTransferencia()
        {
            while (true)
            {
                Data = DateTime.Now.ToString("yyyyMMdd");
                TransfereArquivos();

                if (DateTime.Now.ToString("HH:mm") == "04:00")
                {
                    _procedureService.ExecutarProcedure();
                }
            }
        }

        #region Transfere arquivo
        public void TransfereArquivos()
        {
            try
            {        
                string ediPath = Environment.GetEnvironmentVariable("EDI") ?? throw new InvalidOperationException("Não foi possível localizar a variável de ambiente \"EDI\".");
                string outPath = Environment.GetEnvironmentVariable("OUT") ?? throw new InvalidOperationException("Não foi possível localizar a variável de ambiente \"OUT\".");
                string sentPath = Environment.GetEnvironmentVariable("SENT") ?? throw new InvalidOperationException("Não foi possível localizar a variável de ambiente \"OUT\".");
                
                _verificadorService.VerificaDiretorios(ediPath, outPath, sentPath);
                
                string[] arquivos = Directory.GetFiles(ediPath).Select(Path.GetFileName).ToArray();

                foreach (var arquivo in arquivos)
                {
                    if (arquivo.Length < 17)
                    {
                        File.Delete(arquivo);
                    }
                    else if (arquivo.Substring(9,8) == Data || arquivo.Substring(8,8) == Data)
                    {
                        File.Move(Path.Combine(ediPath, arquivo), Path.Combine(outPath, arquivo));
                    }
                    else
                    {
                        File.Delete(arquivo);
                    }
                }
            }
            catch(InvalidOperationException ioex)
            {
                _exceptionService.TratarExcessao(ioex);
            }
        }
        #endregion
    }
}
