using DiscordPlayerCountBot.Enums;
using DiscordPlayerCountBot.Services.Praser;

namespace DiscordPlayerCountBot.Services.Rcon.ServiceInformation
{
    public class CSGORconServiceInformation : IRconServiceInformation
    {
        public IRconInformationParser GetParser()
        {
            return new CSGOInformationParser();
        }

        public RconServiceType GetServiceType()
        {
            return RconServiceType.CSGO;
        }

        public string GetPlayerListCommand()
        {
            return "status";
        }
    }
}
