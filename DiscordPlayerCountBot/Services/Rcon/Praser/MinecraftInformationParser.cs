using PlayerCountBot.Exceptions;
using System.Text.RegularExpressions;

namespace PlayerCountBot.Services.Praser
{
    public class MinecraftInformationParser : IRconInformationParser
    {
        public BaseViewModel Parse(string message)
        {
            var playerInfoMatch = Regex.Match(message, @"(\d+)\s+of a max of\s+(\d+)");
            var queuedPlayersMatch = Regex.Match(message, @"Queue:\s+(\d+)\s+players waiting");

            if (!playerInfoMatch.Success || playerInfoMatch.Groups.Count < 3)
            {
                throw new ParsingException("Could not find players or max players from a Minecraft Rcon Response");
            }

            var players = int.Parse(playerInfoMatch.Groups[1].Value);
            var maxPlayers = int.Parse(playerInfoMatch.Groups[2].Value);
            int queuedPlayers = !queuedPlayersMatch.Success ? 0 : int.Parse(queuedPlayersMatch.Groups[1].Value);

            return new BaseViewModel()
            {
                Players = players,
                MaxPlayers = maxPlayers,
                QueuedPlayers = queuedPlayers
            };
        }
    }
}