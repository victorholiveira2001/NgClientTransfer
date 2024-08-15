namespace NgClientTransfer.Services
{
    class VerificadorService : IVerificadorService
    {
        private  readonly IProcedureService _procedureService;

        public VerificadorService(IProcedureService procedureService)
        {
            _procedureService = procedureService;
        }

        #region Verificador de diret�rios
        public void VerificaDiretorios(string ediPath, string outPath)
        {
            if (!Directory.Exists(ediPath))
            {
                Directory.CreateDirectory(ediPath);
            }
            if (!Directory.Exists(outPath))
            {
                Directory.CreateDirectory(outPath);
            }
        }
        #endregion
    }
}