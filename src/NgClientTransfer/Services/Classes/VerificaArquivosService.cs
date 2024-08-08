using NgClientTransfer.Services;

public class VerificaArquivosService : IVerificaArquivosService
{
    private readonly IProcedureService _procedureService;
    private readonly IExceptions _exceptions;
    public string Edi { get; set; }
    public string Out { get; set; }
    public string Sent { get; set; }

    public VerificaArquivosService(IProcedureService procedureService, IExceptions exceptions)
    {
        _procedureService = procedureService;
        _exceptions = exceptions;
    }

    public void ConfereExisteArquivos()
        {   
            try{
                var arquivos = Directory.GetFiles(Sent)
                    .Where(arquivo =>
                    {
                        var nomearquivo = Path.GetFileName(arquivo);
                        return nomearquivo.Substring(9, 8) == DateTime.Now.ToString("yyyyMMdd") || 
                               nomearquivo.Substring(8, 8) == DateTime.Now.ToString("yyyyMMdd");
                    }).Select(Path.GetFileName).ToArray();

                if (arquivos.Length < 4)
                {
                    _procedureService.ExecutarProcedure();
                }
            }
            catch (Exception ex)
            {
                _exceptions.TratarExcessao(ex);
            }      
        }

        public void VerificaDiretorios()
        {
            if (!Directory.Exists(Edi)) 
                Directory.CreateDirectory(Edi);
            if (!Directory.Exists(Out)) 
                Directory.CreateDirectory(Out);
            if (!Directory.Exists(Sent)) 
                Directory.CreateDirectory(Sent);
        }

        public void VerificaVariaveis()
        {
            try
            {
                string Edi = Environment.GetEnvironmentVariable("EDI") ?? throw new InvalidOperationException("Não foi possível localizar a variável de ambiente \"EDI\".");
                string Out = Environment.GetEnvironmentVariable("OUT") ?? throw new InvalidOperationException("Não foi possível localizar a variável de ambiente \"OUT\".");
                string Sent = Environment.GetEnvironmentVariable("SENT") ?? throw new InvalidOperationException("Não foi possível localizar a variável de ambiente \"SENT\".");
            }
            catch(InvalidOperationException ioex)
            {
                _exceptions.TratarExcessao(ioex);
            }
            
        }
}