using DiscordPlayerCountBot.Data;
using DiscordPlayerCountBot.Http;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DiscordPlayerCountBot.Services
{

    public class BattleMetricsService : IBattleMetricsService
    {
        public async Task<BattleMetricsServerData?> GetPlayerInformationAsync(string address, string token)
        {
            using var httpClient = new HttpExecuter();

            var response = await httpClient.GET<object, BattleMetricsServerGame>($"https://api.battlemetrics.com/servers/{address}", authToken: new Tuple<string, string>("Authorization", token));
            return response?.data;
        }
    }
}
