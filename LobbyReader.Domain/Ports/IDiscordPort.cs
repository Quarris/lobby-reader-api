using LobbyReader.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LobbyReader.Domain.Ports
{
    public interface IDiscordPort
    {
        Task<DiscordUserDto> GetUser(string id, bool isAuthor = false);
    }
}
