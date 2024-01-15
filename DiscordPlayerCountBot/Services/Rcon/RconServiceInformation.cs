using PlayerCountBot.Enums;
using PlayerCountBot.Services.Praser;

namespace PlayerCountBot.Services
{
    public interface IRconServiceInformation
    {
        public abstract RconServiceType GetServiceType();
        public abstract string GetPlayerListCommand();
        public abstract IRconInformationParser GetParser();
    }
}