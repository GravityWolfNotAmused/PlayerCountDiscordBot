using DiscordPlayerCountBot.Enums;

namespace DiscordPlayerCountBot.Services.SteamQuery
{
    internal interface ISteamQueryService
    {
        public Task<BaseViewModel> GetQueryResponse(string address, int port);
    }
}
