NgClient Transfer é um serviço windows que automatiza a exportação dos arquivos ERP TOTVS Consinco para os servidores da Neogrid com o utilitário NeoGridClient.

O serviço inicia uma conexão com o banco de dados oracle e executa um bloco anônimo para a execução da procedure de exportação, após isso ele faz validações,
e por fim a transferencia dos arquivos para o diretório da NgClient e inicia o serviço de envio FTP, depois de 30 minutos o serviço de envio.

As 04:00 é iniciado o procedimento de conexão com o banco, porém a todo o momento é verificado a presença de arquivos para serem transferidos, em caso de geração manual pelo consinco.
