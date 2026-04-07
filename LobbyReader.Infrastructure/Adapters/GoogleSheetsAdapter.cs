using LobbyReader.Common.Dtos;
using LobbyReader.Domain.Ports;
using LobbyReader.GoogleSheets.Connector;
using LobbyReader.GoogleSheets.Connector.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LobbyReader.Infrastructure.Adapters
{
    public class GoogleSheetsAdapter : IGoogleSheetsPort
    {
        private readonly ISpreadSheetConnector spreadSheetConnector;
        private readonly IMigrationConnector migrationConnector;
        public GoogleSheetsAdapter(ISpreadSheetConnector spreadSheetConnector, IMigrationConnector migrationConnector)
        {
            this.spreadSheetConnector = spreadSheetConnector;
            this.migrationConnector = migrationConnector;
        }
        public List<LobbyEntryDto> GetData()    
        {
            return spreadSheetConnector.GetData();
        }
        public List<DiscordUserDto> GetUserData()
        {
            return spreadSheetConnector.GetUserData();
        }
        public void SaveData(List<LobbyEntryDto> data) 
        {
            var rows = spreadSheetConnector.ConvertToSheet(data);
            spreadSheetConnector.UpdateData(rows);
        }
        public void SaveUserData(List<DiscordUserDto> data)
        {
            var rows = spreadSheetConnector.ConvertToUserSheet(data);
            spreadSheetConnector.UpdateUserData(rows);
        }
        #region MIGRATION
        public List<OldLobbyDto> GetMigrationData()
        {
            return migrationConnector.GetData();
        }
        public List<UserMigrationDto> GetUserMigrationData()
        {
            return migrationConnector.GetUserMigrationData();
        }
        public void SaveUserMigration(List<UserMigrationDto> data)
        {
            var rows = migrationConnector.ConvertToUserMigrationSheet(data);
            migrationConnector.UpdateUserMigrationData(rows);
        }
        #endregion
    }
}
