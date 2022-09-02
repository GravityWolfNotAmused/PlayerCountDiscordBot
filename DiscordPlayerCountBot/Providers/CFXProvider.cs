using System;
using System.Net;
using System.Threading.Tasks;
using DiscordPlayerCountBot.Services;
using log4net;
using PlayerCountBot;

namespace DiscordPlayerCountBot.Providers
{
    public class CFXProvider : IServerInformationProvider
    {
        public ILog Logger = LogManager.GetLogger(typeof(SteamProvider));

        public async Task<GenericServerInformation?> GetServerInformation(BotInformation information)
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
                if(e is WebException webException)
                {
                    if (webException.Status == WebExceptionStatus.Timeout)
                    {
                        Logger.Error($"[CFXProvider] - Speaking with CFX has timed out.", e);
                        return null;
                    }
                    else if (webException.Status == WebExceptionStatus.ConnectFailure)
                    {
                        Logger.Error($"[CFXProvider] - Could not connect to CFX.");
                        return null;
                    }
                    else
                    {
                        Logger.Error($"[CFXProvider] - There was an error speaking with your CFX server.", e);
                        return null;
                    }
                }

                if (e is ApplicationException applicationException)
                {
                    Logger.Error($"[CFXProvider] - {e.Message}");
                    return null;
                }

                Logger.Error($"[CFXProvider] - There was an error speaking with CFX.", e);
                throw;
            }
        }
    }
}
