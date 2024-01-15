using DiscordPlayerCountBot.Enums;
using DiscordPlayerCountBot.Services.Praser;

namespace DiscordPlayerCountBot.Services.Rcon.ServiceInformation
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
