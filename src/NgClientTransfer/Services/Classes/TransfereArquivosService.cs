namespace NgClientTransfer.Services
{
    internal class TransfereArquivos : ITransfereArquivosService
    {
        #region Dependences Injection
        private readonly IProcedureService _procedureService;
        private readonly IServiceManagerService _serviceManager;
        private readonly IEmailService _emailService;
        private readonly IVerificaArquivosService _verificaArquivos;
        private string Data { get; set; }

        public TransfereArquivos(IProcedureService procedureService, IServiceManagerService serviceManager, IEmailService emailService, IVerificaArquivosService verificaArquivos)
        {
            _procedureService = procedureService;
            _serviceManager = serviceManager;
            _emailService = emailService;
            _verificaArquivos = verificaArquivos;
        }

        #endregion

        #region Verificar Arquivos
        public void VerificaArquivos()
        {
            while (true)
            {
                Data = DateTime.Now.ToString("yyyyMMdd");

                
                _verificaArquivos.VerificaDiretorios();
                MovimentaArquivos();

                if (DateTime.Now.ToString("HH:mm") == "04:00")
                {
                    _procedureService.ExecutarProcedure();
                }
                // Validar novamente, caso haja algum erro de importação.
                else if (DateTime.Now.ToString("HH:mm") == "06:00")
                {
                    _verificaArquivos.ConfereExisteArquivos();
                }

                Thread.Sleep(60000);
            }
            
        }   

        #endregion   

        #region Movimenta Arquivos
        public void MovimentaArquivos()
        {
            try
            {
                string[] arquivos = Directory.GetFiles(Edi).Select(Path.GetFileName).ToArray();

                if (arquivos.Length >= 4)
                {
                    foreach (var arquivo in arquivos)
                    {
                        //Se menor que 17, devo excluir para não dar erro na comparação de datas atrelado ao nome do arquivo.
                        if (arquivo.Length < 17)
                        {
                            File.Delete($@"{Edi}\{arquivo}");
                        }
                        else if (arquivo.Substring(9, 8) == DateTime.Now.ToString("yyyyMMdd") ||
                                 arquivo.Substring(8, 8) == DateTime.Now.ToString("yyyyMMdd"))
                        {
                            File.Move($@"{Edi}\{arquivo}",
                                      $@"{Out}\{arquivo}");
                        }
                        else
                        {
                            File.Delete($@"{Edi}\{arquivo}");
                        }
                    }

                    _serviceManager.IniciarServico();
                }  
            }
                catch (Exception ex)
                {
                    var exception = new Exceptions(_emailService, _serviceManager);
                    exception.TratarExcessao(ex);
                }
        }

        #endregion
    }
}
