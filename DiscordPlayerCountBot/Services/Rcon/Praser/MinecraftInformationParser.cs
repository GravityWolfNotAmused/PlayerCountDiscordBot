using DiscordPlayerCountBot.Exceptions;
using System.Text.RegularExpressions;

namespace DiscordPlayerCountBot.Services.Praser
{
    public class MinecraftInformationParser : IRconInformationParser
    {
        public BaseViewModel Parse(string message)
        {
            var playerInfoMatch = Regex.Match(message, @"There are (\d+)/(\d+) players online");
            var queuedPlayersMatch = Regex.Match(message, @"Queue:\s+(\d+)\s+players waiting");

            if (!playerInfoMatch.Success || playerInfoMatch.Groups.Count != 2)
            {
                throw new ParsingException("Could not find players or max players from a CSGO Rcon Response");
            }

            var players = int.Parse(playerInfoMatch.Groups[0].Value);
            var maxPlayers = int.Parse(playerInfoMatch.Groups[1].Value);
            int queuedPlayers = !queuedPlayersMatch.Success ? 0 : int.Parse(queuedPlayersMatch.Groups[0].Value);

            return new BaseViewModel()
            {
                Players = players,
                MaxPlayers = maxPlayers,
                QueuedPlayers = queuedPlayers
            };
        }
    }
}
