using SteamServerQuery;

namespace DiscordPlayerCountBot.Services.SteamQuery
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

            try
            {
                var serverInformation = await SteamServer.QueryServerAsync(address, port = 0);
                model.MaxPlayers = serverInformation.MaxPlayers;
                model.Players = serverInformation.Players;
                model.QueuedPlayers = 0;
            } catch
            {
                throw;
            }

            return model;
        }
    }
}
