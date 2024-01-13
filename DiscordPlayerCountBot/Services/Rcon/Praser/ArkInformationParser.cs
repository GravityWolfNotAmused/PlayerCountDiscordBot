using DiscordPlayerCountBot.Exceptions;
using System.Text.RegularExpressions;

namespace DiscordPlayerCountBot.Services.Praser
{
    public class ArkInformationParser : IRconInformationParser
    {
        public BaseViewModel Parse(string message)
        {
            var match = Regex.Match(message, @"There are (\d+)/(\d+) players online(?:.*\nQueue:\s+(\d+)\s+players waiting)?");

            if (!match.Success)
            {
                throw new ParsingException("Could not find players, max players, or queue information from an ARK RCON Response");
            }

            var players = int.Parse(match.Groups[1].Value);
            var maxPlayers = int.Parse(match.Groups[2].Value);
            int queuedPlayers = match.Groups.Count < 3 || string.IsNullOrEmpty(match.Groups[3].Value) ? 0 : int.Parse(match.Groups[3].Value);

            return new BaseViewModel()
            {
                Players = players,
                MaxPlayers = maxPlayers,
                QueuedPlayers = queuedPlayers
            };
        }
    }
}
