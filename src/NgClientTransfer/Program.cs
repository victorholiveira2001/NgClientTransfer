using Microsoft.Extensions.Hosting;
using NgClientTransfer;
using NgClientTransfer.Repositories;
using NgClientTransfer.Services;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddSingleton<IManipulaArquivosService, ManipulaArquivosService>();
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddTransient<IProcedureService, ProcedureService>();
builder.Services.AddTransient<IServiceManagerService, ServiceManagerService>();
builder.Services.AddTransient<IProcedureRepository, ProcedureRepostory>();

var host = builder.Build();
host.Run();