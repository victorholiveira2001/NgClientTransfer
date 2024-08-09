using NgClientTransfer.Services;

namespace NgClientTransfer
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IMovimentadorService _manipulaArquivosService;
        public Worker(ILogger<Worker> logger, IMovimentadorService manipulaArquivosService)
        {
            _logger = logger;
            _manipulaArquivosService = manipulaArquivosService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Serviço iniciado as: {time}", DateTimeOffset.Now);

                _manipulaArquivosService.GerenciadorDeTransferencia();
                
            }
        }
    }
}
