using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DiscordPlayerCountBot.Providers.Base;
using DiscordPlayerCountBot.Services;
using PlayerCountBot;

namespace DiscordPlayerCountBot.Providers
{
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
                if (e.Message == LastException?.Message)
                    return null;

                WasLastExecutionAFailure = true;
                LastException = e;

                if (e is HttpRequestException requestException)
                {
                    Logger.Error($"[CFXProvider] - The CFX host has failed to respond. {requestException.StatusCode}");
                    return null;
                }

                if (e is WebException webException)
                {
                    if (webException.Status == WebExceptionStatus.Timeout)
                    {
                        Logger.Error($"[CFXProvider] - Speaking with CFX has timed out.");
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
                    Logger.Error($"[CFXProvider] - {applicationException.Message}");
                    return null;
                }

                Logger.Error($"[CFXProvider] - There was an error speaking with CFX.", e);
                throw;
            }
        }
    }
}
