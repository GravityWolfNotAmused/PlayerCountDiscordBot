using DiscordPlayerCountBot.Enums;
using DiscordPlayerCountBot.Services.Praser;

namespace DiscordPlayerCountBot.Services.Rcon.ServiceInformation
{
    public class ArkRconServiceInformation : IRconServiceInformation
    {
        public IRconInformationParser GetParser()
        {
            return new ArkInformationParser();
        }

        public RconServiceType GetServiceType()
        {
            return RconServiceType.Ark;
        }

        public string GetPlayerListCommand()
        {
            return "listplayers";
        }
    }
}
