namespace NgClientTransfer.Services
{
    internal class ManipulaArquivosService : IManipulaArquivosService
    {
        private readonly IProcedureService _procedureService;
        private readonly IServiceManagerService _serviceManager;
        private readonly IEmailService _emailService;

        private string Edi = Environment.GetEnvironmentVariable("EDI");
        private string NgClientOut = Environment.GetEnvironmentVariable("NgClientOut");
        private string NgClientSent = Environment.GetEnvironmentVariable("NgClientSent");
        private string Data { get; set; }

        public ManipulaArquivosService(IProcedureService procedureService, IServiceManagerService serviceManager, IEmailService emailService)
        {
            _procedureService = procedureService;
            _serviceManager = serviceManager;
            _emailService = emailService;
        }

        public async void VerificaArquivos()
        {
            while (true)
            {
                try
                {
                    Data = DateTime.Now.ToString("yyyyMMdd");
                    string[] arquivos = Directory.GetFiles(Edi).Select(Path.GetFileName).ToArray();
                    MovimentaArquivos(arquivos);

                    if (DateTime.Now.ToString("HH:mm") == "05:00")
                    {
                        _procedureService.ExecutarProcedure();
                    }
                    // Validar novamente, caso haja algum erro de importação.
                    else if (DateTime.Now.ToString("HH:mm") == "19:00")
                    {
                        ConfereExisteArquivos();
                    }

                    Thread.Sleep(60000);
                }
                catch (Exception ex)
                {
                    _emailService.DisparaEmail("Erro Neogrid Client Tranfer",
                       $"""
                        Ocorreu um erro durante a verificação de arquivos.

                        Erro: {ex}.

                        Caso necessário, entre em contato com o desenvolvedor do sistema.
                        """);
                }                
            }
        }

        public void MovimentaArquivos(string[] arquivos)
        {
            Data = DateTime.Now.ToString("yyyyMMdd");
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
                    _emailService.DisparaEmail("Erro Neogrid Client Tranfer",
                        $"""
                            Ocorreu um erro durante a execução da transferência de arquivos.

                            Erro: {ex.Message}.

                            Caso necessário, entre em contato com o desenvolvedor do sistema.
                         """);
                }
            }
        }


        public void ConfereExisteArquivos()
        {
            Data = DateTime.Now.ToString("yyyyMMdd");

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
