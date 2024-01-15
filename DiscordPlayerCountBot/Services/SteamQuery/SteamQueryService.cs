using SteamQueryNet;

namespace PlayerCountBot.Services.SteamQuery
{
    public class SteamQueryService : ISteamQueryService
    {
        public async Task<BaseViewModel> GetQueryResponse(string address, int port)
        {
            var model = new BaseViewModel()
            {
                Address = address,
                Port = port
            };

            var serverQuery = await new ServerQuery()
                .Connect($"{address}:{port}")
                .GetServerInfoAsync();

            if (serverQuery == null)
                throw new Exception("Failed to fetch server information from Steam Query");

            model.MaxPlayers = serverQuery.MaxPlayers;
            model.Players = serverQuery.Players;
            model.QueuedPlayers = 0;

            return model;
        }
    }
}