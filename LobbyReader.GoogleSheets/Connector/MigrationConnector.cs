using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using LobbyReader.Common.Dtos;
using LobbyReader.GoogleSheets.Connector.Interfaces;
using Newtonsoft.Json;

namespace LobbyReader.GoogleSheets.Connector
{
    public class MigrationConnector : IMigrationConnector
    {
        private string[] _scopes = { SheetsService.Scope.Spreadsheets }; // Change this if you're accessing Drive or Docs
        private string _applicationName = "Lobby Reader";
        private string _spreadsheetId = "";
        private SheetsService _sheetsService;
        public MigrationConnector()
        {
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
            String range = "New Data!A2:D";
            string valueInputOption = "USER_ENTERED";

            // The new values to apply to the spreadsheet.
            List<Google.Apis.Sheets.v4.Data.ValueRange> updateData = new List<Google.Apis.Sheets.v4.Data.ValueRange>();
            var dataValueRange = new Google.Apis.Sheets.v4.Data.ValueRange();
            dataValueRange.Range = range;
            dataValueRange.Values = data;
            updateData.Add(dataValueRange);

            Google.Apis.Sheets.v4.Data.BatchUpdateValuesRequest requestBody = new Google.Apis.Sheets.v4.Data.BatchUpdateValuesRequest();
            requestBody.ValueInputOption = valueInputOption;
            requestBody.Data = updateData;

            var request = _sheetsService.Spreadsheets.Values.BatchUpdate(requestBody, _spreadsheetId);

            Google.Apis.Sheets.v4.Data.BatchUpdateValuesResponse response = request.Execute();
            // Data.BatchUpdateValuesResponse response = await request.ExecuteAsync(); // For async 

            return JsonConvert.SerializeObject(response);
        }
        public string UpdateUserMigrationData(List<IList<object>> data)
        {
            String range = "Crewmate Normalization!A2:D";
            string valueInputOption = "USER_ENTERED";

            // The new values to apply to the spreadsheet.
            List<Google.Apis.Sheets.v4.Data.ValueRange> updateData = new List<Google.Apis.Sheets.v4.Data.ValueRange>();
            var dataValueRange = new Google.Apis.Sheets.v4.Data.ValueRange();
            dataValueRange.Range = range;
            dataValueRange.Values = data;
            updateData.Add(dataValueRange);

            Google.Apis.Sheets.v4.Data.BatchUpdateValuesRequest requestBody = new Google.Apis.Sheets.v4.Data.BatchUpdateValuesRequest();
            requestBody.ValueInputOption = valueInputOption;
            requestBody.Data = updateData;

            var request = _sheetsService.Spreadsheets.Values.BatchUpdate(requestBody, _spreadsheetId);

            Google.Apis.Sheets.v4.Data.BatchUpdateValuesResponse response = request.Execute();
            // Data.BatchUpdateValuesResponse response = await request.ExecuteAsync(); // For async 

            return JsonConvert.SerializeObject(response);
        }
        public List<OldLobbyDto> GetData()
        {
            String range = "Old Data!A2:T";
            var request = _sheetsService.Spreadsheets.Values.Get(_spreadsheetId, range);

            Google.Apis.Sheets.v4.Data.ValueRange response = request.Execute();
            // Data.BatchUpdateValuesResponse response = await request.ExecuteAsync(); // For async 

            List<OldLobbyDto> data = new List<OldLobbyDto>();
            int count = 0;
            if (response.Values != null)
            {
                foreach (var item in response.Values)
                {
                    OldLobbyDto lobby = new OldLobbyDto
                    {
                        //StartingCell = ++count,
                        Date = DateTime.Parse(item[0]?.ToString()),
                        Time = item[1]?.ToString(),
                        LobbyType = item[2]?.ToString(),
                        Leader = item[3]?.ToString(),
                        Lineup = new List<string>()
                    };
                    if (item.Count > 4)
                    {
                        for (int i = 4; i < item.Count; i++)
                        {
                            lobby.Lineup.Add(item[i]?.ToString());
                        }
                    }
                    data.Add(lobby);
                }
            }

            return data;
        }
        public List<UserMigrationDto> GetUserMigrationData()
        {
            String range = "Crewmate Normalization!A2:D";
            var request = _sheetsService.Spreadsheets.Values.Get(_spreadsheetId, range);

            Google.Apis.Sheets.v4.Data.ValueRange response = request.Execute();
            // Data.BatchUpdateValuesResponse response = await request.ExecuteAsync(); // For async 

            List<UserMigrationDto> data = new List<UserMigrationDto>();
            int count = 0;
            if (response.Values != null)
            {
                foreach (var item in response.Values)
                {
                    UserMigrationDto user = new UserMigrationDto
                    {
                        //StartingCell = ++count,
                        OldName = item[0]?.ToString(),
                        ID = item[1]?.ToString(),
                        Username = item[2]?.ToString(),
                        DisplayName = item[3]?.ToString()
                    };
                    data.Add(user);
                }
            }

            return data;
        }
        public List<IList<object>> ConvertToSheet(List<LobbyEntryDto> list)
        {
            List<IList<object>> result = new List<IList<object>>();
            foreach (var item in list)
            {
                List<object> model =
                [
                    item.Id ?? "",
                    item.Date.ToString() ?? "",
                    item.Game ?? "",
                    item.Host ?? "",
                    item.Lineup ?? "",
                ];
                result.Add(model);
            }
            return result;
        }
        public List<IList<object>> ConvertToUserMigrationSheet(List<UserMigrationDto> list)
        {
            List<IList<object>> result = new List<IList<object>>();
            foreach (var item in list)
            {
                List<object> model =
                [
                    item.OldName ?? "",
                    item.ID ?? "",
                    item.Username ?? "",
                    item.DisplayName ?? ""
                ];
                result.Add(model);
            }
            return result;
        }
    }
}
