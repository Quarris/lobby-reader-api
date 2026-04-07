using LobbyReader.Domain.Ports;
using LobbyReader.Domain.Services;
using LobbyReader.Domain.Services.Interfaces;
using LobbyReader.GoogleSheets.Connector;
using LobbyReader.GoogleSheets.Connector.Interfaces;
using LobbyReader.Infrastructure.Adapters;
using LobbyReader.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddTransient<IDiscordPort, DiscordAdapter>();
builder.Services.AddTransient<IGoogleSheetsPort, GoogleSheetsAdapter>();
builder.Services.AddTransient<ILobbyAnalyzerPort, LobbyAnalyzerRepository>();
builder.Services.AddTransient<ISyncService, SyncService>();
builder.Services.AddTransient<IMigrationService, MigrationService>();
builder.Services.AddTransient<ISpreadSheetConnector, SpreadSheetConnector>();
builder.Services.AddTransient<IMigrationConnector, MigrationConnector>();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Lobby Reader API");
        c.DisplayRequestDuration();
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
