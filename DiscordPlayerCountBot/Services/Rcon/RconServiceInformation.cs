using DiscordPlayerCountBot.Enums;
using DiscordPlayerCountBot.Services.Praser;

namespace DiscordPlayerCountBot.Services
{
    public interface IRconServiceInformation
    {
        public abstract RconServiceType GetServiceType();
        public abstract string GetPlayerListCommand();
        public abstract IRconInformationParser GetParser();
    }
}
