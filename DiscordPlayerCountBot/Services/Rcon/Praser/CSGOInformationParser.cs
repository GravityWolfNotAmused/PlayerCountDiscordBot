using PlayerCountBot.Exceptions;
using System.Text.RegularExpressions;

namespace PlayerCountBot.Services.Praser
{
    public class CSGOInformationParser : IRconInformationParser
    {
        public BaseViewModel Parse(string message)
        {
            var playersMatch = Regex.Match(message, @"players\s+:\s+(\d+)\s+humans");
            var maxPlayersMatch = Regex.Match(message, @"\((\d+)/\d+\s+max\)");
            var queuedPlayersMatch = Regex.Match(message, @"queue\s+:\s+(\d+)\s+players waiting");

            if (!playersMatch.Success || !maxPlayersMatch.Success)
            {
                throw new ParsingException("Could not find players or max players from a CSGO Rcon Response");
            }

            var players = int.Parse(playersMatch.Groups[0].Value);
            var maxPlayers = int.Parse(maxPlayersMatch.Groups[0].Value);
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