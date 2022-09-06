using System.Threading.Tasks;
using PlayerCountBot;

namespace DiscordPlayerCountBot.Services
{
    public interface ISteamService
    {
        public Task<SteamApiResponseData?> GetSteamApiResponse(string address, int port, string token);
    }
}
