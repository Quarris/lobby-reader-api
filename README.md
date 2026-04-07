# Lobby Reader

A .NET 9.0 application designed to sync and manage game lobby information from Discord messages into Google Sheets.

## Projects

- **Lobby Reader API**: An ASP.NET Core Web API that provides endpoints to sync lobbies and lineups.
- **LobbyReader.MigrationProcess**: A console application to migrate historical data from older formats.
- **LobbyReader.Domain**: Core logic and port interfaces.
- **LobbyReader.Infrastructure**: Adapters for external services (Discord, Repositories).
- **LobbyReader.GoogleSheets**: Connector for Google Sheets API.
- **LobbyReader.MachineLearning**: ML.NET model for text classification of lobby types.
- **LobbyReader.Common**: Shared DTOs, extensions, and types.

## Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- A Google Cloud Project with the Google Sheets API enabled.
- A `credentials.json` file for a Service Account or OAuth2 client from the Google Cloud Console.
- A Discord Bot Token (for fetching user information).
- A Google Sheet ID where the data will be stored.

## Configuration

### 1. Google Credentials
Place your `credentials.json` file in the root of the following projects (or ensure it's copied to the output directory):
- `Lobby Reader API`
- `LobbyReader.MigrationProcess`

### 2. API Settings
Update `appsettings.json` in the `Lobby Reader API` project:

```json
{
  "DiscordBotKey": "YOUR_DISCORD_BOT_TOKEN",
  "GoogleSheetId": "YOUR_GOOGLE_SHEET_ID"
}
```

## How to Run

### Running the API
From the root directory:
```powershell
cd "Lobby Reader API"
dotnet run
```
The API will be available at `https://localhost:5001` (or the configured port). You can access the Swagger UI at `https://localhost:5001/swagger` in development mode.

### Running the Migration Tool
From the root directory:
```powershell
cd LobbyReader.MigrationProcess
dotnet run
```

### Running Tests
From the root directory:
```powershell
cd LobbyReader.Tests
dotnet test
```

## API Endpoints

- `POST /api/Sync/SyncLobbies`: Syncs lobby information from a list of Discord messages.
- `POST /api/Sync/SyncLineups`: Syncs lineup information for existing lobbies.

## Architecture

This project follows a Hexagonal Architecture (Ports and Adapters) pattern:
- **Domain**: Contains the business logic and defines interfaces (Ports).
- **Infrastructure**: Implements the interfaces for specific technologies like Discord or local repositories (Adapters).
- **GoogleSheets**: Specifically handles the integration with Google Sheets.
- **MachineLearning**: Contains the ML.NET model for classifying lobby messages.
- **API**: The entry point for the web application (Lobby Reader API).
