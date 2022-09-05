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

        public async Task<SteamApiResponseData?> GetSteamApiResponse(BotInformation information)
        {
            using var httpClient = new HttpExecuter(new HttpClient());
            var response = await httpClient.GET<object, SteamServerListResponse>("https://api.steampowered.com/IGameServersService/GetServerList/v1/", new SteamGetServerListQueryParams()
            {
                Key = information.SteamAPIToken,
                Filter = $"\\addr\\{information.Address}"
            });

            if (response == null) return null;

            var serverPortAndAddress = information.GetAddressAndPort();
            var data = response.GetServerDataByPort(serverPortAndAddress.Item2.ToString());

            if (data == null) return null;

            return data;
        }
    }
}
