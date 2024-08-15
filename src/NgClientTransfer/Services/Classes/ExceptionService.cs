
using Oracle.ManagedDataAccess.Client;

namespace NgClientTransfer.Services
{
    class ExceptionService : IExceptionService
    {
        private readonly IEmailService _emailService;
        private readonly IServiceManagerService _serviceManager;

        public ExceptionService(IEmailService emailService, IServiceManagerService serviceManager)
        {
            _emailService = emailService;
            _serviceManager = serviceManager;
        }

        #region Tratar Excessão
        public void TratarExcessao(Exception ex)
        {
            if (ex is InvalidOperationException ioex)
            {
            _emailService.DisparaEmail("Erro na transferência",
                $"""
                    Ocorreu um erro durante a execução da transferência de arquivos.

                    Erro: {ioex.Message}.

                    Caso necessário, entre em contato com o desenvolvedor do sistema.
                    """);
                    // Encerrando o serviço até o problema ser solucionado pelo usuário (Variáveis serem criadas)
                    _serviceManager.EncerrarHost();
            }
            else if (ex is OracleException oex)
            {
            _emailService.DisparaEmail("Erro na transferência",
                $"""
                    Ocorreu um erro durante a execução do banco de dados oracle.

                    Erro: {oex.Message}.

                    Caso necessário, entre em contato com o desenvolvedor do sistema.
                    """);
                    // Encerrando o serviço até o problema ser solucionado pelo usuário (Variáveis serem criadas)
                    _serviceManager.EncerrarHost();
            }
            else
            {
                _emailService.DisparaEmail("Erro na transferência",
                $"""
                Ocorreu um erro durante a execução da transferência de arquivos.

                Erro: {ex.Message}.

                Caso necessário, entre em contato com o desenvolvedor do sistema.
                """);  
            }
        }

        #endregion
    }
}