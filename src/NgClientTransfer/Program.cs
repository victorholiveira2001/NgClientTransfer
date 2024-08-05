using NgClientTransfer;
using NgClientTransfer.Services;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddSingleton<IEmailService, EmailService>();

var host = builder.Build();
host.Run();
