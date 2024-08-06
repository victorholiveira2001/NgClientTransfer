using NgClientTransfer.Services;

namespace NgClientTransfer
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IManipulaArquivosService _manipulaArquivosService;
        public Worker(ILogger<Worker> logger, IManipulaArquivosService manipulaArquivosService)
        {
            _logger = logger;
            _manipulaArquivosService = manipulaArquivosService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                    _manipulaArquivosService.VerificaArquivos();
                    
                    // TODO Fazer o acionamento da regra e neg�cio.
                }
                await Task.Delay(600000, stoppingToken);
            }
        }
    }
}
