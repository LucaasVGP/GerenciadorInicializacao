using GerenciadorInicializacao.Models;
using GerenciadorInicializacao.Services;
using Microsoft.Extensions.Configuration;

var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

IConfiguration config = builder.Build();

var appSettings = config.Get<AppSettings>();

if (appSettings == null) Environment.Exit(500);
GeralService.AbrirProgramas(appSettings.Apps);
GeralService.TocarAudio();