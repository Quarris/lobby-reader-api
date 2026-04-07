using LobbyReader.Common.Dtos;
using LobbyReader.Domain.Ports;
using LobbyReader.Domain.Services.Interfaces;
using System.Text.RegularExpressions;

namespace LobbyReader.Domain.Services
{
    public class MigrationService : IMigrationService
    {
        private readonly IGoogleSheetsPort googleSheets;
        public MigrationService(IGoogleSheetsPort googleSheets)
        {
            this.googleSheets = googleSheets;
        }
        [Obsolete]
        public void Migrate_OLD()
        {
            var oldLobbies = googleSheets.GetMigrationData();
            var users = googleSheets.GetUserData();
            var newLobbies = new List<LobbyEntryDto>();
            var oldUsers = new List<UserMigrationDto>();
            foreach (var lobby in oldLobbies) 
            {
                newLobbies.Add(ConvertOldLobby(lobby));
                if (!oldUsers.Any(u => u.OldName == lobby.Leader))
                {
                    oldUsers.Add(new UserMigrationDto
                    {
                        OldName = lobby.Leader
                    });
                } else
                {
                    var existingUserId = oldUsers.FindIndex(u => u.OldName == lobby.Leader);
                    oldUsers[existingUserId].Count++;
                }
                foreach (var user in lobby.Lineup)
                {
                    if(!oldUsers.Any(u => u.OldName == user))
                    {
                        oldUsers.Add(new UserMigrationDto
                        {
                            OldName = user
                        });
                    } else
                    {
                        var existingUserId = oldUsers.FindIndex(u => u.OldName == user);
                        oldUsers[existingUserId].Count++;
                    }
                }
            }
            foreach (var oldUser in oldUsers)
            {
                foreach (var user in users)
                {
                    if (oldUser.OldName.ToLower() == user.Username.ToLower() || oldUser.OldName.ToLower() == user.GlobalName.ToLower())
                    {
                        oldUser.ID = user.Id;
                        oldUser.Username = user.Username;
                        oldUser.DisplayName = user.GlobalName;
                    }
                }
                if (oldUser.ID == null)
                {
                    oldUser.ID = Guid.NewGuid().ToString();
                    oldUser.Username = oldUser.OldName;
                    oldUser.DisplayName = oldUser.OldName;
                }
            }
            oldUsers = oldUsers.OrderByDescending(u => u.Count).ToList();
            googleSheets.SaveUserMigration(oldUsers);
        }
        public void Migrate()
        {
            var oldLobbies = googleSheets.GetMigrationData();
            var users = googleSheets.GetUserData();
            var lobbies = googleSheets.GetData();
            var newLobbies = new List<LobbyEntryDto>();
            var userMigrations = googleSheets.GetUserMigrationData();
            var notFound = new List<string>();
            foreach (var user in userMigrations)
            {
                var username = user.Username.ToLower().Replace(" ", "");
                Guid guidOutput;
                bool isValid = Guid.TryParse(user.ID, out guidOutput);
                if (isValid)
                {
                    username += "_old";
                }
                var existingUser = users.Any(u => u.Username == username);
                if(!existingUser)
                {
                    users.Add(new DiscordUserDto
                    {
                        Id = user.ID,
                        Username = username,
                        GlobalName = user.DisplayName
                    });
                }
            }
            foreach (var lobby in oldLobbies)
            {
                var newLobby = ConvertOldLobby(lobby);

                var lineup = new List<string>();
                var author = userMigrations.FirstOrDefault(u => u.OldName == lobby.Leader);
                if(author != null)
                {
                    newLobby.Host = author.DisplayName;
                    var username = author.Username.ToLower().Replace(" ", "");
                    Guid guidOutput;
                    bool isValid = Guid.TryParse(author.ID, out guidOutput);
                    if (isValid)
                    {
                        username += "_old";
                    }
                    lineup.Add(username);
                } else
                {
                    notFound.Add(lobby.Leader);
                }
                foreach (var item in lobby.Lineup)
                {
                    var userMatch = userMigrations.FirstOrDefault(u => u.OldName == item);
                    if (userMatch != null)
                    {
                        var username = userMatch.Username.ToLower().Replace(" ", "");
                        Guid guidOutput;
                        bool isValid = Guid.TryParse(userMatch.ID, out guidOutput);
                        if (isValid)
                        {
                            username += "_old";
                        }
                        lineup.Add(username);
                    } else
                    {
                        notFound.Add(item);
                    }
                }
                newLobby.Lineup = string.Join(", ", lineup);
                newLobbies.Add(newLobby);  
            }
            lobbies.AddRange(newLobbies);
            lobbies = lobbies.OrderByDescending(l => l.Date).ToList();
            googleSheets.SaveData(lobbies);
            googleSheets.SaveUserData(users);
        }
        public LobbyEntryDto ConvertOldLobby(OldLobbyDto oldLobby)
        {
            var newLobby = new LobbyEntryDto();
            newLobby.Date = oldLobby.Date.Date;
            oldLobby.Time = oldLobby.Time.Replace(" UTC", "");
            var timeValue = int.Parse(Regex.Match(oldLobby.Time, @"\d+").Value);
            if(timeValue == 12)
            {
                timeValue = 0;
            }
            var amOrPm = new string(oldLobby.Time.Where(c => !char.IsDigit(c)).ToArray());
            if(amOrPm.ToLower() == "pm")
            {
                timeValue += 12;
            }
            newLobby.Date = newLobby.Date.Value.AddHours(timeValue);
            long epochTicks = new DateTime(1970, 1, 1).Ticks;
            long unixTime = ((newLobby.Date.Value.Ticks - epochTicks) / TimeSpan.TicksPerSecond);
            newLobby.Id = unixTime.ToString();
            if(oldLobby.LobbyType == "AU Normal")
            {
                newLobby.Game = "Among Us";
            } else
            {
                newLobby.Game = "Modded Among Us";
            }
            return newLobby;
        }
    }
}
