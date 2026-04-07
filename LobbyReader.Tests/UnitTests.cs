using LobbyReader.Infrastructure.Adapters;
using LobbyReader.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;

namespace LobbyReader.Tests
{
    public class Tests
    {
        DiscordAdapter discordAdapter;
        LobbyAnalyzerRepository lobbyAnalyzerRepository;
        [SetUp]
        public void Setup()
        {
            var configuration = new ConfigurationBuilder()
                .Build();
            discordAdapter = new DiscordAdapter(configuration);
            lobbyAnalyzerRepository = new LobbyAnalyzerRepository(discordAdapter);
        }

        [Test]
        public void Test1()
        {
            var result = lobbyAnalyzerRepository.GetLobbyType("__**Debut QOTC Lobby <t:1766271600:F> to <t:1766278800:t>**__\r\nBOTC is a Social Deduction game similar to Town of Salem, Werewolf, Mafia etc.\r\nClassic tale of good vs evil.\r\n*Newcomers welcome! If you have any question feel free to message me.*\r\n\r\n**Game is FREE and on BROWSER. All you need is a mic and an account at https://botc.app/**\r\n\r\nReact with 🕰️  to sign up \r\n\r\nLooking for 8+ players\r\n\r\n<@&851148677871304735>");
            //var result = lobbyAnalyzerRepository.GetLobbyType("**__ChocoLobby - Modded Lobby (TOU 5.1.2)__**\r\n**__<t:1737158400:F> to <t:1737165600:t>__**\r\nReact with <:thesickestplays:947665005816451112> to sign up.\r\n\r\nDa Rules: <#960898514039222312> \r\nGet the mod: <#845376972593823744> \r\nAny questions or suggestions: DMs are open.\r\n\r\n<@&845660349431545857>");
            Assert.Pass();
        }
    }
}
