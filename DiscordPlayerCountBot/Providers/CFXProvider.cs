using System;
using System.Threading.Tasks;
using DiscordPlayerCountBot.Services;
using log4net;
using PlayerCountBot;

namespace DiscordPlayerCountBot.Providers
{
    public class CFXProvider : IServerInformationProvider
    {
        public ILog Logger = LogManager.GetLogger(typeof(SteamProvider));

        public async Task<GenericServerInformation> GetServerInformation(BotInformation information)
        {
            var service = new CFXService();

            try
            {
                var playerInfo = await service.GetPlayerInformationAsync(information.Address);
                var serverInfo = await service.GetServerInformationAsync(information.Address);
                var addressAndPort = information.GetAddressAndPort();

                return new GenericServerInformation()
                {
                    Address = addressAndPort.Item1,
                    CurrentPlayers = playerInfo.Count,
                    MaxPlayers = serverInfo.GetMaxPlayers(),
                    Port = addressAndPort.Item2,
                };
            }
            catch(Exception e)
            {
                Logger.Error($"[CFXProvider] - There was an error speaking with CFX.", e);
                throw;
            }
        }
    }
}
