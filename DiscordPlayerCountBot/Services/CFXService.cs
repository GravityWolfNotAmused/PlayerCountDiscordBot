using DiscordPlayerCountBot.Http;
using DiscordPlayerCountBot.Source.CFX;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace DiscordPlayerCountBot.Services
{
    public class CFXService : ICFXService
    {
        public async Task<List<CFXPlayerInformation>?> GetPlayerInformationAsync(string address)
        {
            using var httpClient = new HttpExecuter(new HttpClient());
            return await httpClient.GET<object, List<CFXPlayerInformation>>($"http://{address}/Players.json");
        }

        public async Task<CFXServer?> GetServerInformationAsync(string address)
        {
            using var httpClient = new HttpExecuter(new HttpClient());
            return await httpClient.GET<object, CFXServer>($"http://{address}/Info.json");
        }
    }
}
