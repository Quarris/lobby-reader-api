using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using LobbyReader.Common.Dtos;
using LobbyReader.GoogleSheets.Connector.Interfaces;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace LobbyReader.GoogleSheets.Connector
{
    public class SpreadSheetConnector : ISpreadSheetConnector
    {
        private readonly string[] _scopes = [SheetsService.Scope.Spreadsheets]; // Change this if you're accessing Drive or Docs
        private const string _applicationName = "Lobby Reader";
        private readonly string _spreadsheetId; //PROD
        private SheetsService _sheetsService;
        public SpreadSheetConnector(IConfiguration config)
        {
            _spreadsheetId = config.GetValue<string>("GoogleSheetId") ?? throw new ArgumentNullException("GoogleSheetId is missing");
            ConnectToGoogle();
        }
        public void ConnectToGoogle()
        {
            GoogleCredential credential;

            // Put your credentials json file in the root of the solution and make sure copy to output dir property is set to always copy 
            using (var stream = new FileStream(Path.Combine("credentials.json"),
                FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(_scopes);
            }

            // Create Google Sheets API service.
            _sheetsService = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = _applicationName
            });
        }
        public string UpdateData(List<IList<object>> data)
        {
            const string range = "Lobbies!A2:E";
            const string valueInputOption = "USER_ENTERED";

            // The new values to apply to the spreadsheet.
            var updateData = new List<Google.Apis.Sheets.v4.Data.ValueRange>();
            var dataValueRange = new Google.Apis.Sheets.v4.Data.ValueRange
            {
                Range = range,
                Values = data
            };
            updateData.Add(dataValueRange);

            var requestBody = new Google.Apis.Sheets.v4.Data.BatchUpdateValuesRequest
            {
                ValueInputOption = valueInputOption,
                Data = updateData
            };

            var request = _sheetsService.Spreadsheets.Values.BatchUpdate(requestBody, _spreadsheetId);

            var response = request.Execute();
            // Data.BatchUpdateValuesResponse response = await request.ExecuteAsync(); // For async 

            return JsonConvert.SerializeObject(response);
        }
        public string UpdateUserData(List<IList<object>> data)
        {
            const string range = "Crewmates!A2:C";
            const string valueInputOption = "USER_ENTERED";

            // The new values to apply to the spreadsheet.
            var updateData = new List<Google.Apis.Sheets.v4.Data.ValueRange>();
            var dataValueRange = new Google.Apis.Sheets.v4.Data.ValueRange
            {
                Range = range,
                Values = data
            };
            updateData.Add(dataValueRange);

            var requestBody = new Google.Apis.Sheets.v4.Data.BatchUpdateValuesRequest();
            requestBody.ValueInputOption = valueInputOption;
            requestBody.Data = updateData;

            var request = _sheetsService.Spreadsheets.Values.BatchUpdate(requestBody, _spreadsheetId);

            var response = request.Execute();
            // Data.BatchUpdateValuesResponse response = await request.ExecuteAsync(); // For async 

            return JsonConvert.SerializeObject(response);
        }
        public List<LobbyEntryDto> GetData()
        {
            const string range = "Lobbies!A2:E";
            var request = _sheetsService.Spreadsheets.Values.Get(_spreadsheetId, range);

            var response = request.Execute();
            // Data.BatchUpdateValuesResponse response = await request.ExecuteAsync(); // For async 

            var data = new List<LobbyEntryDto>();
            if (response.Values == null) return data;
            foreach (var item in response.Values)
            {
                var lobby = new LobbyEntryDto
                {
                    //StartingCell = ++count,
                    Id = item[0]?.ToString(),
                    Date = DateTime.Parse(item[1]?.ToString()),
                    Game = item[2]?.ToString(),
                    Host = item[3]?.ToString()
                };
                if (item.Count > 4)
                {
                    lobby.Lineup = item[4]?.ToString();
                }
                data.Add(lobby);
            }

            return data;
        }
        public List<DiscordUserDto> GetUserData()
        {
            const string range = "Crewmates!A2:C";
            var request = _sheetsService.Spreadsheets.Values.Get(_spreadsheetId, range);

            var response = request.Execute();
            // Data.BatchUpdateValuesResponse response = await request.ExecuteAsync(); // For async 

            var data = new List<DiscordUserDto>();
            if (response.Values == null) return data;
            data.AddRange(response.Values.Select(item => new DiscordUserDto
            {
                //StartingCell = ++count,
                Id = item[0]?.ToString(), Username = item[1]?.ToString(), GlobalName = item[2]?.ToString()
            }));

            return data;
        }
        public List<IList<object>> ConvertToSheet(List<LobbyEntryDto> list)
        {
            return list.Select(item => (List<object>)[item.Id ?? "", item.Date.ToString() ?? "", item.Game ?? "", item.Host ?? "", item.Lineup ?? "",]).Cast<IList<object>>().ToList();
        }
        public List<IList<object>> ConvertToUserSheet(List<DiscordUserDto> list)
        {
            return list.Select(item => (List<object>)[item.Id ?? "", item.Username ?? "", item.GlobalName ?? ""]).Cast<IList<object>>().ToList();
        }
    }
}
