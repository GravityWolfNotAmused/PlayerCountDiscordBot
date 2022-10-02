namespace PlayerCountBot.Services
{
    public class BattleMetricsService : IProviderService<BattleMetricsServerData>
    {
        public async Task<BattleMetricsServerData?> GetInformation(string address, string? token = null)
        {
            using var httpClient = new HttpExecuter();

            var response = await httpClient.GET<object, BattleMetricsServerGame>($"https://api.battlemetrics.com/servers/{address.Split(':')[0]}", authToken: new Tuple<string, string>("Authorization", token));
            return response?.data;
        }
    }
}
