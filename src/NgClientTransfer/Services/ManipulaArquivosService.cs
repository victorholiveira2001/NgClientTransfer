using System.Globalization;
using NgClientTransfer.Services;

namespace NgClientTransfer.Services
{
    internal class ManipulaArquivosService : IManipulaArquivosService
    {
        private readonly ProcedureService _procedureService;

        public ManipulaArquivosService(ProcedureService procedureService)
        {
               _procedureService = procedureService;
        }
        private DateTime Data { get; set; }
        public void VerificaArquivos()
        {
            while (true)
            {
                Data = DateTime.ParseExact(DateTime.Now.ToString(), "yyyyMMdd", CultureInfo.InvariantCulture);
                string[] arquivos = Directory.GetFiles($@"\\172.16.1.1\Consinco\EDI\").Select(Path.GetFileName).ToArray();
                MovimentaArquivos(arquivos);

                if (DateTime.Now.ToString("HH:mm") == "05:00")
                {
                    _procedureService.ExecutarProcedure();
                }
                else if (DateTime.Now.ToString("HH:mm") == "19:00")
                {
                    string[] àrquivos = Directory.GetFiles(@"C:\NeoGridClient\documents\sent\").Select(Path.GetFileName).ToArray();
                    ConfereExisteArquivos(arquivos);
                }

                Thread.Sleep(60000);
            }
        }

        public void MovimentaArquivos(string[] arquivos)
        {
            while (true)
            {
                Data = DateTime.ParseExact(DateTime.Now.ToString(), "yyyyMMdd", CultureInfo.InvariantCulture);
                if (arquivos.Length >= 4)
                {
                    try
                    {
                        foreach (var arquivo in arquivos)
                        {
                            if (arquivo.Length < 17)
                            {
                                File.Delete($@"\\172.16.1.1\Consinco\EDI\{arquivo}");
                            }
                            else if (arquivo.Substring(9, 8) == Data.ToString() || 
                                     arquivo.Substring(8, 8) == Data.ToString())
                            {
                                File.Move($@"\\172.16.1.1\Consinco\EDI\{arquivo}",
                                          $@"C:\NeoGridClient\documents\out\{arquivo}");
                            }
                            else
                            {
                                File.Delete($@"\\172.16.1.1\Consinco\EDI\{arquivo}");
                            }
                        }

                        var serviceManager = new ServiceManagerService();
                        serviceManager.IniciarServico();

                    }
                    catch (Exception ex)
                    {
                         // TODO Implementar mensageria.
                         // TODO Tratamento, se houver.
                    }
                }
            }
        }


        public void ConfereExisteArquivos(string[] arquivos)
        {
            int contador = 0;
            Data = DateTime.ParseExact(DateTime.Now.ToString(), "yyyyMMdd", CultureInfo.InvariantCulture);

            foreach (string arquivo in arquivos)
            {
                if (arquivo.Substring(9, 8) == Data.ToString() || 
                    arquivo.Substring(8, 8) == Data.ToString())
                {
                    contador++;
                }
            }
            if (contador < 4)
            {
                _procedureService.ExecutarProcedure();
            }
        }
    }
}
