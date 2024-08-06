using NgClientTransfer.Repositories;
using Oracle.ManagedDataAccess.Client;
using System.Security;

namespace NgClientTransfer.Services
{
    public class ProcedureService : IProcedureService
    {
        private readonly IProcedureRepository _procedureRepository;
        private readonly IConfiguration _configuration;

        public ProcedureService(IProcedureRepository procedureRepository, IConfiguration configuration)
        {
            _procedureRepository = procedureRepository;
            _configuration = configuration;
        }

        public void ExecutarProcedure()
        {
            var connectionString = _configuration["DB_CONNECTION_STRING"];
            var username = _configuration["DB_USERNAME"];
            var password = _configuration["DB_PASSWORD"];

            var oraclCredential = CreateOracleCredential(username, password);

            _procedureRepository.ConexaoDb(connectionString, oraclCredential);
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

