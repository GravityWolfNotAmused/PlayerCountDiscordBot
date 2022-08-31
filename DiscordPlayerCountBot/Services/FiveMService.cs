using DiscordPlayerCountBot.Source.CFX;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace DiscordPlayerCountBot.Services
{
    public class CFXService : ICFXService
    {
        public async Task<List<CFXPlayerInformation>> GetPlayerInformationAsync(string address)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"http://{address}/Players.json");

            using HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
            using Stream stream = response.GetResponseStream();
            using StreamReader reader = new StreamReader(stream);
            var responseDataStr = await reader.ReadToEndAsync();
            var responseObject = JsonConvert.DeserializeObject<List<CFXPlayerInformation>>(responseDataStr);
            return responseObject;
        }

        public async Task<CFXServer> GetServerInformationAsync(string address)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"http://{address}/Info.json");

            using HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
            using Stream stream = response.GetResponseStream();
            using StreamReader reader = new StreamReader(stream);
            var responseDataStr = await reader.ReadToEndAsync();
            var responseObject = JsonConvert.DeserializeObject<CFXServer>(responseDataStr);
            return responseObject;
        }
    }
}
