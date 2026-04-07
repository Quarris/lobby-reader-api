using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LobbyReader.Common.Dtos
{
    public class UserMigrationDto
    {
        public string OldName { get; set; }
        public string ID { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public int Count { get; set; }
        public UserMigrationDto()
        {
            Count = 1;
        }
    }
}
