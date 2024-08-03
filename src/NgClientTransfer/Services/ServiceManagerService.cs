using System.ServiceProcess;

namespace NgClientTransfer.Services
{
    public class ServiceManagerService : IServiceManagerService
    {
        public void IniciarServico()
        {

            try
            {
                var sc = new ServiceController("ngclient");

                if (sc.Status.Equals(ServiceControllerStatus.Stopped))
                {
                    sc.Start();
                    Thread.Sleep(1800000);
                }
            }
            catch (Exception ex)
            {
                // TODO Implementar mesageria
                // Tratamento, se houver
            }
        }

        public void PararServico()
        {
            try
            {
                var sc = new ServiceController();

                if (sc.Status.Equals(ServiceControllerStatus.Running) ||
                    sc.Status.Equals(ServiceControllerStatus.ContinuePending) ||
                    sc.Status.Equals(ServiceControllerStatus.Paused) ||
                    sc.Status.Equals(ServiceControllerStatus.StartPending))
                {
                    Thread.Sleep(30000); //Pausar a thread por cinco minuto.
                }
            }
            catch (Exception ex)
            {
                // TODO Implementar mensageria
                // TODO Tratamento se Houver 
            }
        }

        public void ReiniciarServico()
        {
            var sc = new ServiceController();

            if (!sc.Status.Equals(ServiceControllerStatus.Stopped) ||
                !sc.Equals(ServiceControllerStatus.StopPending))
            {
                sc?.Stop();
            }
        }
    }
}
