using DiscordPlayerCountBot.Providers.Base;
using DiscordPlayerCountBot.Services;
using log4net;
using PlayerCountBot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace DiscordPlayerCountBot.Providers
{
    public class ScumProvider : ServerInformationProvider
    {
        private ILog Logger = LogManager.GetLogger(typeof(ScumProvider));

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

                if (WasLastExecutionAFailure)
                {
                    Logger.Info($"[ScumProvider] - Bot for Address: {information.Address} successfully fetched data after failure.");
                    LastException = null;
                    WasLastExecutionAFailure = false;
                }

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
                if (e.Message == LastException?.Message)
                    return null;

                WasLastExecutionAFailure = true;
                LastException = e;

                if (e is HttpRequestException requestException)
                {
                    Logger.Error($"[ScumProvider] - The Scum provider has failed to respond.");
                    return null;
                }

                if (e is WebException webException)
                {
                    if (webException.Status == WebExceptionStatus.Timeout)
                    {
                        Logger.Error($"[ScumProvider] - Speaking with the Scum provider has timed out.");
                        return null;
                    }
                    else if (webException.Status == WebExceptionStatus.ConnectFailure)
                    {
                        Logger.Error($"[ScumProvider] - Could not connect to Scum provider.");
                        return null;
                    }
                    else
                    {
                        Logger.Error($"[ScumProvider] - There was an error speaking with the Scum data provider.", e);
                        return null;
                    }
                }

                if (e is ApplicationException applicationException)
                {
                    Logger.Error($"[ScumProvider] - {applicationException.Message}");
                    return null;
                }

                Logger.Error($"[ScumProvider] - There was an error speaking with Scum.", e);
                throw;
            }
        }
    }
}
