using LobbyReader.Common.Dtos;

namespace LobbyReader.Domain.Services.Interfaces
{
    public interface ISyncService
    {
        void SyncLineups(List<MessageDto> messages);
        void SyncLobbies(List<MessageDto> messages);
    }
}