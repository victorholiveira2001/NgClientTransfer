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
        private readonly IServiceManagerService _serviceManager;
        string? Data { get; set; }

        public MovimentadorService(IProcedureService procedureService, IVerificadorService verificadorService, IExceptionService exceptionService, IServiceManagerService serviceManager)
        {
            _procedureService = procedureService;
            _verificadorService = verificadorService;
            _exceptionService = exceptionService;
            _serviceManager = serviceManager;
        }

        #region Gerenciador de transferência
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
        #endregion

        #region Transfere arquivo
        public void TransfereArquivos()
        {
            try
            {        
                string ediPath = Environment.GetEnvironmentVariable("EDI") ?? throw new InvalidOperationException("Não foi possível localizar a variável de ambiente \"EDI\".");
                string outPath = Environment.GetEnvironmentVariable("OUT") ?? throw new InvalidOperationException("Não foi possível localizar a variável de ambiente \"OUT\".");
                
                _verificadorService.VerificaDiretorios(ediPath, outPath);
                
                string[] arquivos = Directory.GetFiles(ediPath).Select(Path.GetFileName).ToArray();

                if (arquivos.Length >= 4)
                {
                    foreach (var arquivo in arquivos)
                    {
                        if (arquivo.Contains(Data))
                            File.Move(Path.Combine(ediPath, arquivo), Path.Combine(outPath, arquivo));
                        else
                            File.Delete(Path.Combine(ediPath, arquivo));
                    }

                    _serviceManager.IniciarServico();
                    Thread.Sleep(1800000);
                    _serviceManager.PararServico();
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
