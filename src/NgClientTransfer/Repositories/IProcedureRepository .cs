using Oracle.ManagedDataAccess.Client;

namespace NgClientTransfer.Repositories
{
    public interface IProcedureRepository
    {
        void ConexaoDb(string connectionString, OracleCredential oracleCredential);
    }
}
