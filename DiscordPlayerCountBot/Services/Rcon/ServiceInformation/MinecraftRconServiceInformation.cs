using PlayerCountBot.Enums;
using PlayerCountBot.Services.Praser;

namespace PlayerCountBot.Services.Rcon.ServiceInformation
{
    public class MinecraftRconServiceInformation : IRconServiceInformation
    {
        public IRconInformationParser GetParser()
        {
            return new MinecraftInformationParser();
        }

        public RconServiceType GetServiceType()
        {
            return RconServiceType.Minecraft;
        }

        public string GetPlayerListCommand()
        {
            return "list";
        }
    }
}