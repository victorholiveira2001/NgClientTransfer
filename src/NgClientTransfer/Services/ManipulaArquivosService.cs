using System.Globalization;
using System.Linq;
using System.Numerics;
using NgClientTransfer.Services;

namespace NgClientTransfer.Services
{
    internal class ManipulaArquivosService : IManipulaArquivosService
    {
        private readonly ProcedureService _procedureService;
        private readonly ServiceManagerService _serviceManager;
        private string Edi = Environment.GetEnvironmentVariable("EDI");
        private string NgClientOut = Environment.GetEnvironmentVariable("NgClientOut");
        private string NgClientSent = Environment.GetEnvironmentVariable("NgClientSent");
        private DateTime Data { get; set; }

        public ManipulaArquivosService(ProcedureService procedureService, ServiceManagerService serviceManager)
        {
            _procedureService = procedureService;
            _serviceManager = serviceManager;
        }

        public void VerificaArquivos()
        {
            while (true)
            {
                Data = DateTime.ParseExact(DateTime.Now.ToString(), "yyyyMMdd", CultureInfo.InvariantCulture);
                string[] arquivos = Directory.GetFiles(Edi).Select(Path.GetFileName).ToArray();
                MovimentaArquivos(arquivos);

                if (DateTime.Now.ToString("HH:mm") == "05:00")
                {
                    _procedureService.ExecutarProcedure();
                }
                else if (DateTime.Now.ToString("HH:mm") == "19:00")
                {
                    ConfereExisteArquivos();
                }

                Thread.Sleep(60000);
            }
        }

        public void MovimentaArquivos(string[] arquivos)
        {

            Data = DateTime.ParseExact(DateTime.Now.ToString(), "yyyyMMdd", CultureInfo.InvariantCulture);
            if (arquivos.Length >= 4)
            {
                try
                {
                    foreach (var arquivo in arquivos)
                    {
                        //Se menor que 17, devo excluir para não dar erro na comparação de datas atralado ao nome.
                        if (arquivo.Length < 17) 
                        {
                            File.Delete($@"{Edi}\{arquivo}");
                        }
                        else if (arquivo.Substring(9, 8) == Data.ToString() || 
                                 arquivo.Substring(8, 8) == Data.ToString())
                        {
                            File.Move($@"{Edi}\{arquivo}",
                                      $@"{NgClientOut}\{arquivo}");
                        }
                        else
                        {
                            File.Delete($@"{Edi}\{arquivo}");
                        }
                    }

                    _serviceManager.IniciarServico();

                }
                catch (Exception ex)
                {
                        // TODO Implementar mensageria.
                        // TODO Tratamento, se houver.
                }
            }
        }


        public void ConfereExisteArquivos()
        {
            Data = DateTime.ParseExact(DateTime.Now.ToString(), "yyyyMMdd", CultureInfo.InvariantCulture);


            var arquivos = Directory.GetFiles(NgClientSent)
                .Where(arquivo =>
                {
                    var nomearquivo = Path.GetFileName(arquivo);
                    return nomearquivo.Substring(9, 8) == Data.ToString() || nomearquivo.Substring(8, 8) == Data.ToString();
                })
                .Select(Path.GetFileName)
                .ToArray();

            if (arquivos.Length < 4)
            {
                _procedureService.ExecutarProcedure();
            }
        }
    }
}
