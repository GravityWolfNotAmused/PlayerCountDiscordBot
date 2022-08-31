using log4net;
using Newtonsoft.Json;
using PlayerCountBot;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace DiscordPlayerCountBot.Services
{
    public class SteamService : ISteamService
    {
        public ILog Logger = LogManager.GetLogger(typeof(SteamService));

        public async Task<SteamApiResponseData?> GetSteamApiResponse(BotInformation information)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"https://api.steampowered.com/IGameServersService/GetServerList/v1/?key={information.SteamAPIToken}&filter=\\addr\\{information.Address}");

            using HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
            Logger.Debug("[Steam Provider]:: Response Received.");
            using Stream stream = response.GetResponseStream();
            Logger.Debug("[Steam Provider]:: Response Steam received.");
            using StreamReader reader = new StreamReader(stream);
            var responseDataStr = await reader.ReadToEndAsync();
            Logger.Debug("[Steam Provider]:: Response Read.");

            var responseObject = JsonConvert.DeserializeObject<SteamServerListResponse>(responseDataStr);

            if (responseObject == null) return null;

            var serverPortAndAddress = information.GetAddressAndPort();
            SteamApiResponseData data = responseObject.GetServerDataByPort(serverPortAndAddress.Item2.ToString());

            if (data == null) return null;
            return data;
        }
    }
}
