using LobbyReader.Common.Dtos;
using LobbyReader.Common.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LobbyReader.Domain.Ports
{
    public interface ILobbyAnalyzerPort
    {
        DateTime GetDate(string message);
        string GetLobbyType(string message);
        Task<Tuple<List<string>, List<DiscordUserDto>>> GetLineup(MentionDto mentions, List<DiscordUserDto> users);
        Task<Tuple<string, List<DiscordUserDto>>> GetAuthor(string author, List<DiscordUserDto> users);
        bool IsCancelledLobby(string message);
    }
}
