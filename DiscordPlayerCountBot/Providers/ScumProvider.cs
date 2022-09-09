using DiscordPlayerCountBot.Attributes;
using DiscordPlayerCountBot.Providers.Base;
using DiscordPlayerCountBot.Services;
using PlayerCountBot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordPlayerCountBot.Providers
{
    [Name("Scum")]
    public class ScumProvider : ServerInformationProvider
    {
        public async override Task<GenericServerInformation?> GetServerInformation(BotInformation information, Dictionary<string, string> applicationVariables)
        {
            var service = new ScumService();
            var addressAndPort = information.GetAddressAndPort();

            try
            {
                var apiResponse = await service.GetPlayerInformationAsync(addressAndPort.Item1, addressAndPort.Item2);

                if (apiResponse == null)
                    throw new ApplicationException("Response cannot be null.");

                if (apiResponse.Servers == 0)
                    throw new ApplicationException("Response contained no valid servers.");

                var server = apiResponse.GetScumServerData(addressAndPort.Item2);

                if (server == null) throw new ApplicationException("Could not find Server in Scum Provider.");

                HandleLastException(information);

                return new()
                {
                    Address = addressAndPort.Item1,
                    Port = addressAndPort.Item2,
                    CurrentPlayers = server.Players,
                    MaxPlayers = server.MaxPlayers,
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
