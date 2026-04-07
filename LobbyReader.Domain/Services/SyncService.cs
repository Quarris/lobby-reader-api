using LobbyReader.Common.Dtos;
using LobbyReader.Domain.Ports;
using LobbyReader.Domain.Services.Interfaces;

namespace LobbyReader.Domain.Services
{
    public class SyncService(IGoogleSheetsPort googleSheets, ILobbyAnalyzerPort lobbyAnalyzer)
        : ISyncService
    {
        public async void SyncLobbies(List<MessageDto> messages)
        {
            try
            {
                var lobbies = googleSheets.GetData();
                var users = googleSheets.GetUserData();
                var newLobbies = new List<LobbyEntryDto>();
                foreach (var message in messages)
                {
                    try
                    {
                        var existingLobbyId = lobbies.FindIndex(l => l.Id == message.Id);
                        if (existingLobbyId != -1)
                        {
                            var existingLobby = lobbies[existingLobbyId];
                            if (lobbyAnalyzer.IsCancelledLobby(message.Content))
                            {
                                existingLobby.Lineup = "CANCELLED";
                            }
                            existingLobby.Date = lobbyAnalyzer.GetDate(message.Content);
                            existingLobby.Game = lobbyAnalyzer.GetLobbyType(message.Content);
                            var author = await lobbyAnalyzer.GetAuthor(message.AuthorId, users);
                            existingLobby.Host = author.Item1;
                            if(existingLobby.Host == "Oceana")
                            {
                                existingLobby.Game = existingLobby.Game switch
                                {
                                    "Among Us" => "ABG",
                                    "Modded Among Us" => "Modded ABG",
                                    _ => existingLobby.Game
                                };
                            }
                            users = author.Item2;
                            lobbies[existingLobbyId] = existingLobby;
                        }
                        else
                        {
                            LobbyEntryDto lobby = new()
                            {
                                Id = message.Id,
                                Date = lobbyAnalyzer.GetDate(message.Content),
                                Game = lobbyAnalyzer.GetLobbyType(message.Content)
                            };
                            var author = await lobbyAnalyzer.GetAuthor(message.AuthorId, users);
                            lobby.Host = author.Item1;
                            if (lobby.Host == "Oceana")
                            {
                                lobby.Game = lobby.Game switch
                                {
                                    "Among Us" => "ABG",
                                    "Modded Among Us" => "Modded ABG",
                                    _ => lobby.Game
                                };
                            }
                            users = author.Item2;
                            if (lobbyAnalyzer.IsCancelledLobby(message.Content))
                            {
                                lobby.Lineup = "CANCELLED";
                            }
                            if(!string.IsNullOrEmpty(lobby.Game))
                            {
                                lobbies.Add(lobby);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // ignored
                    }
                }
                lobbies = lobbies.OrderByDescending(l => l.Date).ToList();
                googleSheets.SaveData(lobbies);
                googleSheets.SaveUserData(users);
            }
            catch (Exception e)
            {
                // ignored
            }
        }
        public async void SyncLineups(List<MessageDto> messages)
        {
            try
            {
                var lobbies = googleSheets.GetData();
                var users = googleSheets.GetUserData();
                var newLobbies = new List<LobbyEntryDto>();
                foreach (var message in messages)
                {
                    try
                    {
                        var lobbyDate = lobbyAnalyzer.GetDate(message.Content);
                        var existingLobbyId = lobbies.FindIndex(l => l.Date == lobbyDate);
                        if (existingLobbyId == -1) continue;
                        var existingLobby = lobbies[existingLobbyId];
                        var (lineup, userDtos) = await lobbyAnalyzer.GetLineup(message.Mentions, users);
                        users = userDtos;
                        existingLobby.Lineup = string.Join(", ", lineup);
                        lobbies[existingLobbyId] = existingLobby;
                    }
                    catch (Exception ex)
                    {
                        // ignored
                    }
                }
                lobbies = lobbies.OrderByDescending(l => l.Date).ToList();
                googleSheets.SaveData(lobbies);
                googleSheets.SaveUserData(users);
            }
            catch (Exception e)
            {
                // ignored
            }
        }
    }
}
