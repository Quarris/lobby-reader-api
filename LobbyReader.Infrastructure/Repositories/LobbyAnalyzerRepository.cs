using LobbyReader.Common.Dtos;
using LobbyReader.Common.Extensions;
using LobbyReader.Common.Types;
using LobbyReader.Domain.Ports;
using System.Globalization;
using System.Text.RegularExpressions;

namespace LobbyReader.Infrastructure.Repositories
{
    public class LobbyAnalyzerRepository : ILobbyAnalyzerPort
    {
        private readonly IDiscordPort discordAdapter;
        public LobbyAnalyzerRepository(IDiscordPort discordAdapter)
        {
            this.discordAdapter = discordAdapter;    
        }
        public DateTime GetDate(string message)
        {
            Regex r = new Regex(@"<t:\d+:F>");
            var match = r.Match(message);
            if(match.Success)
            {
                var utcCode = long.Parse(match.ToString().Replace("<t:", "").Replace(":F>", ""));
                return DateTime.UnixEpoch.AddSeconds(utcCode);
            } else
            {
                r = new Regex(@"<t:\d+:f>");
                match = r.Match(message);
                if (match.Success)
                {
                    var utcCode = long.Parse(match.ToString().Replace("<t:", "").Replace(":f>", ""));
                    return DateTime.UnixEpoch.AddSeconds(utcCode);
                }
            }
            return DateTime.Now;
        }
        public string GetLobbyType(string message)
        {
            message = message.Replace(System.Environment.NewLine, "");
            var types = LobbyType.amongUsTypes;
            Regex r = new Regex(@"\*\*.+?\*\*");
            var match = r.Match(message);
            if (match != null)
            {
                var title = match.ToString().Replace("_","");
                foreach (KeyValuePair<string, string[]> type in types)
                {
                    int count = 0;
                    foreach (var item in type.Value)
                    {
                        if(Regex.IsMatch(title.ToLower(), @"\b" + item.ToLower() + @"\b"))
                        {
                            count++;
                        }
                    }
                    if (count > 0)
                    {
                        return type.Key;
                    }
                }
                types = LobbyType.otherGameTypes;
                foreach (KeyValuePair<string, string[]> type in types)
                {
                    int count = 0;
                    foreach (var item in type.Value)
                    {
                        if (Regex.IsMatch(title.ToLower(), @"\b" + item.ToLower() + @"\b"))
                        {
                            count++;
                        }
                    }
                    if (count > 0)
                    {
                        return type.Key;
                    }
                }
            }
            return "";
        }
        public async Task<Tuple<List<string>, List<DiscordUserDto>>> GetLineup(MentionDto mentions, List<DiscordUserDto> users)
        {
            List<string> lineup = new List<string>();
            foreach (var user in mentions.Users)
            {
                var username = "";
                var existingUsername = users.FirstOrDefault(u => u.Id == user);
                if (existingUsername != null)
                {
                    username = existingUsername.Username;
                    lineup.Add(username);
                } else
                {
                    var discordUser = await discordAdapter.GetUser(user, false);
                    if (discordUser != null)
                    {
                        users.Add(discordUser);
                        username = discordUser.Username;
                        lineup.Add(username);
                    }
                }
            }
            return new Tuple<List<string>, List<DiscordUserDto>>(lineup, users);
        }
        public async Task<Tuple<string, List<DiscordUserDto>>> GetAuthor(string author, List<DiscordUserDto> users)
        {
            var username = "";
            var existingUsername = users.FirstOrDefault(u => u.Id == author);
            if (existingUsername != null)
            {
                username = existingUsername.GlobalName;
                return new Tuple<string, List<DiscordUserDto>>(username, users);
            }
            else
            {
                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                var authorName = await discordAdapter.GetUser(author, true);
                users.Add(authorName);
                return new Tuple<string, List<DiscordUserDto>>(authorName.GlobalName.FirstCharToUpper(), users);
            }
            
        }
        public bool IsCancelledLobby(string message)
        {
            string str = message.Substring(0, 2);
            return str.Equals("~~", StringComparison.OrdinalIgnoreCase);
        }
    }
}
