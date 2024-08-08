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

        public void PararServico()
        {
            var sc = new ServiceController("ngclient");

            if (!sc.Equals(ServiceControllerStatus.Stopped))
            {
                sc?.Stop();
            }
        }

        public void EncerrarHost()
        {
            _applicationLifetime.StopApplication();
        }
    }
}
