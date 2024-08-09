namespace NgClientTransfer.Services
{
    class VerificadorService : IVerificadorService
    {
        private  readonly IProcedureService _procedureService;

        public VerificadorService(IProcedureService procedureService)
        {
            _procedureService = procedureService;
        }

        public void VerificaDiretorios(string ediPath, string outPath, string sentPath)
        {
            if (!Directory.Exists(ediPath))
            {
                Directory.CreateDirectory(ediPath);
            }
            if (!Directory.Exists(outPath))
            {
                Directory.CreateDirectory(outPath);
            }
            if (!Directory.Exists(sentPath))
            {
                Directory.CreateDirectory(sentPath);
            }
        }
    }
}