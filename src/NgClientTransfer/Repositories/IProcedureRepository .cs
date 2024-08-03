using Oracle.ManagedDataAccess.Client;

namespace NgClientTransfer.Repositories
{
    internal interface IProcedureRepository
    {
        void ConexaoDb(string connectionString, OracleCredential oracleCredential);
    }
}
