using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DiscordPlayerCountBot.Attributes;
using DiscordPlayerCountBot.Providers.Base;
using DiscordPlayerCountBot.Services;
using PlayerCountBot;

namespace DiscordPlayerCountBot.Providers
{
    [Name("CFX")]
    public class CFXProvider : ServerInformationProvider
    {
        public async override Task<GenericServerInformation?> GetServerInformation(BotInformation information, Dictionary<string, string> applicationVariables)
        {
            var service = new CFXService();

            try
            {
                var playerInfo = await service.GetPlayerInformationAsync(information.Address);
                var serverInfo = await service.GetServerInformationAsync(information.Address);
                var addressAndPort = information.GetAddressAndPort();

                if (playerInfo == null)
                    throw new ApplicationException("Player Information cannot be null. Is your server offline?");

                if (serverInfo == null)
                    throw new ApplicationException("Server Information cannot be null. Is your server offline?");

                HandleLastException(information);

                return new GenericServerInformation()
                {
                    Address = addressAndPort.Item1,
                    CurrentPlayers = playerInfo.Count,
                    MaxPlayers = serverInfo.GetMaxPlayers(),
                    Port = addressAndPort.Item2,
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
