using PlayerCountBot.Enums;
using PlayerCountBot.Services.Praser;

namespace PlayerCountBot.Services.Rcon.ServiceInformation
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
