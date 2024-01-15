using PlayerCountBot.Enums;
using PlayerCountBot.Services.Praser;

namespace PlayerCountBot.Services.Rcon.ServiceInformation
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
