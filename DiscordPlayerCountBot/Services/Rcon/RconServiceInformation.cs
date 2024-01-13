using DiscordPlayerCountBot.Services.Praser;

namespace DiscordPlayerCountBot.Services
{
    public class RconServiceInformation
    {
        public string PlayersCommand { get; private set; }
        public IRconInformationParser Parser { get; private set; }

        public RconServiceInformation(string playersCommand, IRconInformationParser parser)
        {
            PlayersCommand = playersCommand;
            Parser = parser;
        }
    }
}
