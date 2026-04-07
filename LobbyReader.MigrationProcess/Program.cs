using LobbyReader.Domain.Ports;
using LobbyReader.Domain.Services;
using LobbyReader.Domain.Services.Interfaces;
using LobbyReader.GoogleSheets.Connector;
using LobbyReader.GoogleSheets.Connector.Interfaces;
using LobbyReader.Infrastructure.Adapters;
using LobbyReader.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Services.AddTransient<IDiscordPort, DiscordAdapter>();
builder.Services.AddTransient<IGoogleSheetsPort, GoogleSheetsAdapter>();
builder.Services.AddTransient<ILobbyAnalyzerPort, LobbyAnalyzerRepository>();
builder.Services.AddTransient<ISyncService, SyncService>();
builder.Services.AddTransient<IMigrationService, MigrationService>();
builder.Services.AddTransient<ISpreadSheetConnector, SpreadSheetConnector>();
builder.Services.AddTransient<IMigrationConnector, MigrationConnector>();

using IHost host = builder.Build();

Migrate(host.Services, "Lifetime 1");

await host.RunAsync();

static void Migrate(IServiceProvider hostProvider, string lifetime)
{
    var migrationService = hostProvider.GetRequiredService<IMigrationService>();
    migrationService.Migrate();

}