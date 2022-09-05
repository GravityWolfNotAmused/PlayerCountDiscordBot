using DiscordPlayerCountBot.Providers.Base;
using DiscordPlayerCountBot.Services;
using log4net;
using PlayerCountBot;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace DiscordPlayerCountBot.Providers
{
    public class SteamProvider : ServerInformationProvider
    {
        private ILog Logger = LogManager.GetLogger(typeof(SteamProvider));

        public async override Task<GenericServerInformation?> GetServerInformation(BotInformation information)
        {
            var service = new SteamService();
            var serverPortAndAddress = information.GetAddressAndPort();

            try
            {
                var response = await service.GetSteamApiResponse(information);

                if (response == null)
                    throw new ApplicationException($"Server Address: {information.Address} was not found in Steam's directory.");

                if (WasLastExecutionAFailure)
                {
                    Logger.Info($"[CFXProvider] - Bot for Address: {information.Address} successfully fetched data after failure.");
                    LastException = null;
                    WasLastExecutionAFailure = false;
                }

                return new()
                {
                    Address = serverPortAndAddress.Item1,
                    Port = serverPortAndAddress.Item2,
                    CurrentPlayers = response.players,
                    MaxPlayers = response.max_players,
                    PlayersInQueue = response.GetQueueCount()
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
                    Logger.Error($"[SteamProvider] - The Steam has failed to respond.");
                    return null;
                }

                if (e is WebException webException)
                {
                    if (webException.Status == WebExceptionStatus.Timeout)
                    {
                        Logger.Error($"[SteamProvider] - Speaking with Steam has timed out.");
                        return null;
                    }
                    else if (webException.Status == WebExceptionStatus.ConnectFailure)
                    {
                        Logger.Error($"[SteamProvider] - Could not connect to Steam.");
                        return null;
                    }
                    else
                    {
                        Logger.Error($"[SteamProvider] - There was an error speaking with your CFX server.", e);
                        return null;
                    }
                }

                if (e is ApplicationException applicationException)
                {
                    Logger.Error($"[SteamProvider] - {e.Message}");
                    return null;
                }

                Logger.Error($"[SteamProvider] - There was an error speaking with Steam.", e);
                throw;
            }
        }
    }
}
