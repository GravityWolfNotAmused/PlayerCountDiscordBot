using PlayerCountBot.Enums;

namespace PlayerCountBot.Services
{
    public interface IRconService
    {
        public Task<BaseViewModel> GetRconResponse(string address, int port, string authorizationToken, RconServiceType serviceType);
    }
}
