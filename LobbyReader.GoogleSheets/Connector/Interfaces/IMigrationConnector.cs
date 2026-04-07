using LobbyReader.Common.Dtos;

namespace LobbyReader.GoogleSheets.Connector.Interfaces
{
    public interface IMigrationConnector
    {
        void ConnectToGoogle();
        List<IList<object>> ConvertToSheet(List<LobbyEntryDto> list);
        List<IList<object>> ConvertToUserMigrationSheet(List<UserMigrationDto> list);
        List<OldLobbyDto> GetData();
        List<UserMigrationDto> GetUserMigrationData();
        string UpdateData(List<IList<object>> data);
        string UpdateUserMigrationData(List<IList<object>> data);
    }
}