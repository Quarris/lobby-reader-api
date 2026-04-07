using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LobbyReader.Common.Types
{
    public static class LobbyType
    {
        public static readonly Dictionary<string, string[]> amongUsTypes = new Dictionary<string, string[]>
        {
            {"Modded Among Us", ["Modded", "Town of Us", "The Other Roles", "of Us", "ToU", "Limited Roles", "Proximity"] },
            {"Among Us", ["Unmodded", "Vanilla", "Among Us", "Hide and Seek"]}
        };
        public static readonly Dictionary<string, string[]> otherGameTypes = new Dictionary<string, string[]>
        {
            {"Pummel Party", ["Pummel"]},
            {"Secret Hitler", ["Hitler"]},
            {"Crab Game", ["Crab"]},
            {"Garry's Mod", ["Garry's Mod", "Garrys Mod", "Prop Hunt"]},
            {"Avalon", ["Avalon"]},
            {"Goose Goose Duck", ["Goose", "Duck", "Geese"]},
            {"Overwatch", ["Overwatch"]},
            {"Town of Salem 2", ["Town of Salem 2"]},
            {"Town of Salem", ["Town of Salem"]},
            {"Just Act Natural", ["Just Act Natural"]},
            {"First Class Trouble", ["First Class Trouble"]},
            {"Fall Guys", ["Fall Guys"]},
            {"Lethal Company", ["Lethal Company"]},
            { "Unfortunate Spacemen", ["Unfortunate Spacemen"]},
            { "Dale & Dawson Stationery Supplies", ["Dawson"]},
            { "R.E.P.O.", ["R.E.P"]},
            { "Dead by Daylight", ["Dead by Daylight"]},
            { "Codenames", ["Codenames"]},
            { "Blood on the Clocktower", ["Blood on", "Clocktower", "Botc", "QOTC"]}

        };
    }
}
