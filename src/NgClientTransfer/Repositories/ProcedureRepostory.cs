using NgClientTransfer.Services;
using Oracle.ManagedDataAccess.Client;

namespace NgClientTransfer.Repositories
{
    internal class ProcedureRepostory : IProcedureRepository
    {
        private readonly IExceptionService _exceptionService;
        public ProcedureRepostory(IExceptionService exceptionService)
        {
            _exceptionService = exceptionService;
        }

        #region Conexão Bd
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
                catch (OracleException oex)
                {
                    _exceptionService.TratarExcessao(oex);
                }
            }
        }
        #endregion
    }
}
