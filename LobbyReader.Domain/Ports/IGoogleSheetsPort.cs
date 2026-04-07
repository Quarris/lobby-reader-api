using LobbyReader.Common.Dtos;
using LobbyReader.GoogleSheets.Connector.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LobbyReader.Domain.Ports
{
    public interface IGoogleSheetsPort
    {
        List<LobbyEntryDto> GetData();
        List<DiscordUserDto> GetUserData();
        void SaveData(List<LobbyEntryDto> data);
        void SaveUserData(List<DiscordUserDto> data);
        List<OldLobbyDto> GetMigrationData();
        void SaveUserMigration(List<UserMigrationDto> data);
        List<UserMigrationDto> GetUserMigrationData();
    }
}
