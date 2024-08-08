using NgClientTransfer.Services;
using Oracle.ManagedDataAccess.Client;

namespace NgClientTransfer.Repositories
{
    internal class ProcedureRepostory : IProcedureRepository
    {
        private readonly IEmailService _emailService;
        public ProcedureRepostory(IEmailService emailService)
        {
            _emailService = emailService;
        }

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
                    _emailService.DisparaEmail("Erro na execução da procedure",
                    $"""
                    Ocorreu um erro durante a execução da procedure.

                    Erro: {oex.Message}.

                    Caso necessário, entre em contato com o desenvolvedor do sistema.
                    """);
                }
            }
        }
    }
}
