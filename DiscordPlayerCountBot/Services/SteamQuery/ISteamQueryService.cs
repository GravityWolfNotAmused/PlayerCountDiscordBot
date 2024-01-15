using PlayerCountBot.Enums;

namespace PlayerCountBot.Services.SteamQuery
{
    internal interface ISteamQueryService
    {
        public Task<BaseViewModel> GetQueryResponse(string address, int port);
    }
}
