using LobbyReader.Common.Dtos;

namespace LobbyReader.GoogleSheets.Connector.Interfaces
{
    public interface ISpreadSheetConnector
    {
        void ConnectToGoogle();
        List<LobbyEntryDto> GetData();
        List<DiscordUserDto> GetUserData();
        string UpdateData(List<IList<object>> data);
        string UpdateUserData(List<IList<object>> data);
        List<IList<object>> ConvertToSheet(List<LobbyEntryDto> list);
        List<IList<object>> ConvertToUserSheet(List<DiscordUserDto> list);
    }
}