using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LobbyReader.Common.Dtos
{
    public class LobbyEntryDto
    {
        public string Id { get; set; }
        public DateTime? Date { get; set; }
        public string Game { get; set; }
        public string Host { get; set; }
        public string Lineup { get; set; }
    }
}
