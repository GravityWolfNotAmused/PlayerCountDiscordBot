using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DiscordPlayerCountBot.Attributes;
using DiscordPlayerCountBot.Providers.Base;
using DiscordPlayerCountBot.Services;
using PlayerCountBot;

namespace DiscordPlayerCountBot.Providers
{
    [Name("BattleMetrics")]
    public class BattleMetricsProvider : ServerInformationProvider
    {
        public async override Task<GenericServerInformation?> GetServerInformation(BotInformation information, Dictionary<string, string> applicationVariables)
        {
            var service = new BattleMetricsService();

            try
            {
                var addressAndPort = information.GetAddressAndPort();
                var server = await service.GetPlayerInformationAsync(addressAndPort.Item1, applicationVariables["BattleMetricsKey"]);

                if (server == null)
                    throw new ApplicationException("Server cannot be null. Is your server offline?");

                HandleLastException(information);

                return new GenericServerInformation()
                {
                    Address = addressAndPort.Item1,
                    CurrentPlayers = server.attributes?.players ?? 0,
                    MaxPlayers = server.attributes?.maxPlayers ?? 0,
                    Port = addressAndPort.Item2,
                    PlayersInQueue = 0
                };
            }
            catch (Exception e)
            {
                HandleException(e);
                return null;
            }
        }
    }
}
