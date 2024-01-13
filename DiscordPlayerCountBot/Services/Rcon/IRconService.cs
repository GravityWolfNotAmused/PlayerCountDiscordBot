using DiscordPlayerCountBot.Enums;

namespace DiscordPlayerCountBot.Services
{
    public interface IRconService
    {
        public Task<BaseViewModel> GetRconResponse(string address, int port, string authorizationToken, RconServiceType serviceType);
    }
}
