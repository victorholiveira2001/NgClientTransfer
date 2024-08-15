using Microsoft.Extensions.Hosting;
using NgClientTransfer;
using NgClientTransfer.Repositories;
using NgClientTransfer.Services;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddSingleton<IMovimentadorService, MovimentadorService>();
builder.Services.AddTransient<IVerificadorService, VerificadorService>();
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddTransient<IProcedureService, ProcedureService>();
builder.Services.AddTransient<IServiceManagerService, ServiceManagerService>();
builder.Services.AddTransient<IProcedureRepository, ProcedureRepostory>();
builder.Services.AddTransient<IExceptionService, ExceptionService>();

var host = builder.Build();
host.Run();