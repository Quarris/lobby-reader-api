using LobbyReader.Common.Dtos;
using LobbyReader.Domain.Ports;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace LobbyReader.Infrastructure.Adapters
{
    public class DiscordAdapter : IDiscordPort
    {
        private readonly HttpClient _httpClient;
        public DiscordAdapter(IConfiguration config)
        {
            var discordKey = config.GetSection("DiscordBotKey").Get<string>();
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bot {discordKey}");
        }
        public async Task<DiscordUserDto> GetUser(string id, bool isAuthor = false)
        {
            Thread.Sleep(1000);
            string URL = $"https://discord.com/api/v9/users/{id}";
            var response = await _httpClient.GetAsync(URL);
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                var user = JsonSerializer.Deserialize<DiscordUserDto>(result);
                if (isAuthor)
                {
                    var name = user.GlobalName;
                    Regex rgx = new Regex("[^a-zA-Z0-9 -]");
                    user.GlobalName = rgx.Replace(name, "").Replace(" ", "");
                }
                if(!string.IsNullOrEmpty(user.GlobalName))
                {
                    user.GlobalName = user.Username;
                }
                return user;
            }
            return new DiscordUserDto
            {
                Id = "0"
            };
        }
    }
}
