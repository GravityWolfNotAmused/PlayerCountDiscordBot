using DiscordPlayerCountBot.Data.Steam;

namespace DiscordPlayerCountBot.Services;

public interface ISteamService
{
    public Task<SteamApiResponseData?> GetSteamApiResponse(string address, int port, string token);
}