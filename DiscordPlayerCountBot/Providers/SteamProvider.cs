using DiscordPlayerCountBot.Services;
using log4net;
using PlayerCountBot;
using System;
using System.Threading.Tasks;

namespace DiscordPlayerCountBot.Providers
{
    public class SteamProvider : IServerInformationProvider
    {
        public ILog Logger = LogManager.GetLogger(typeof(SteamProvider));

        public async Task<GenericServerInformation> GetServerInformation(BotInformation information)
        {
            var service = new SteamService();
            var serverPortAndAddress = information.GetAddressAndPort();

            try
            {
                var response = await service.GetSteamApiResponse(information);

                if (response == null)
                    throw new ApplicationException($"Server Address: {information.Address} was not found in Steam's directory.");

                var serverInformation = new GenericServerInformation()
                {
                    Address = serverPortAndAddress.Item1,
                    Port = serverPortAndAddress.Item2,
                    CurrentPlayers = response.players,
                    MaxPlayers = response.max_players,
                    PlayersInQueue = response.GetQueueCount()
                };

                return serverInformation;
            }
            catch(Exception e)
            {
                Logger.Error($"[SteamProvider] - There was an error speaking with Steam.", e);
                throw;
            }
        }
    }
}
