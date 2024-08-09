using NgClientTransfer.Repositories;
using Oracle.ManagedDataAccess.Client;
using System.Security;

namespace NgClientTransfer.Services
{
    public class ProcedureService : IProcedureService
    {
        private readonly IProcedureRepository _procedureRepository;
        private readonly IExceptionService _exceptionService;
        public ProcedureService(IProcedureRepository procedureRepository, IExceptionService exceptionService)
        {
            _procedureRepository = procedureRepository;
            _exceptionService = exceptionService;
        }

        public void ExecutarProcedure()
        {
            try
            {
                var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
                var username = Environment.GetEnvironmentVariable("DB_USER");
                var password = Environment.GetEnvironmentVariable("DB_PASSWORD");

                var oraclCredential = CreateOracleCredential(username, password);

                _procedureRepository.ConexaoDb(connectionString, oraclCredential);

            }
            catch (InvalidOperationException ioex)
            {
                _exceptionService.TratarExcessao(ioex);
            }
            catch (OracleException oex)
            {
                _exceptionService.TratarExcessao(oex);
            }
            
            
        }


        private OracleCredential CreateOracleCredential(string username, string password)
        {
            SecureString secPw = new SecureString();

            foreach (char letter in password)
            {
                secPw.AppendChar(letter);
            }

            secPw.MakeReadOnly();
            return new OracleCredential(username, secPw);
        }
    }
}

