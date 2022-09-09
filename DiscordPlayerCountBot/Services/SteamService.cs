using DiscordPlayerCountBot.Http;
using log4net;
using PlayerCountBot;
using System.Net.Http;
using System.Threading.Tasks;

namespace DiscordPlayerCountBot.Services
{

    public class SteamService : ISteamService
    {
        public ILog Logger = LogManager.GetLogger(typeof(SteamService));

        public async Task<SteamApiResponseData?> GetSteamApiResponse(string address, int port, string token)
        {
            using var httpClient = new HttpExecuter();
            var response = await httpClient.GET<object, SteamServerListResponse>("https://api.steampowered.com/IGameServersService/GetServerList/v1/", new SteamGetServerListQueryParams()
            {
                Key = token,
                Filter = $"\\addr\\{address}:{port}"
            });

            if (response == null) return null;

            var data = response.GetServerDataByPort(port);

            if (data == null) return null;

            return data;
        }
    }
}
