using System.ServiceProcess;

namespace NgClientTransfer.Services
{
    public class ServiceManagerService : IServiceManagerService
    {

        private readonly IHostApplicationLifetime _applicationLifetime;

        public ServiceManagerService(IHostApplicationLifetime applicationLifetime)
        {
            _applicationLifetime = applicationLifetime;
        }

        #region Iniciar Serviço
        public void IniciarServico()
        {
            var sc = new ServiceController("ngclient");

            if (sc.Status.Equals(ServiceControllerStatus.Stopped))
            {
                sc.Start();
                Thread.Sleep(1800000);
                PararServico();
            }
        }
        #endregion

        #region Parar Serviço
        public void PararServico()
        {
            var sc = new ServiceController("ngclient");

            if (!sc.Equals(ServiceControllerStatus.Stopped))
            {
                sc?.Stop();
            }
        }
        #endregion

        #region Encerra Host Lifetime
        public void EncerrarHost()
        {
            _applicationLifetime.StopApplication();
        }
        #endregion
    }
}
