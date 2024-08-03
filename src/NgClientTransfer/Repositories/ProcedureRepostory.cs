using Oracle.ManagedDataAccess.Client;

namespace NgClientTransfer.Repositories
{
    internal class ProcedureRepostory : IProcedureRepository
    {
        public void ConexaoDb(string connectionString, OracleCredential oracleCredential)
        {
            using (var conexao = new OracleConnection(connectionString, oracleCredential))
            {
                try
                {
                    conexao.Open();

                    using (var comando = new OracleCommand("pkg_exp_biscompany.sp_exportadadosauto", conexao))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    // TODO Medidas corretivas, se houver.
                    // TODO Serviço de mensageria.
                }
            }
        }
    }
}
