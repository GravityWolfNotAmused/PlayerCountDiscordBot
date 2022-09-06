using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DiscordPlayerCountBot.Providers.Base;
using DiscordPlayerCountBot.Services;
using log4net;
using PlayerCountBot;

namespace DiscordPlayerCountBot.Providers
{
    public class BattleMetricsProvider : ServerInformationProvider
    {
        private ILog Logger = LogManager.GetLogger(typeof(BattleMetricsProvider));

        public async override Task<GenericServerInformation?> GetServerInformation(BotInformation information, Dictionary<string, string> applicationVariables)
        {
            var service = new BattleMetricsService();

            try
            {
                var addressAndPort = information.GetAddressAndPort();
                var server = await service.GetPlayerInformationAsync(addressAndPort.Item1, applicationVariables["BattleMetricsKey"]);

                if (server == null)
                    throw new ApplicationException("Server cannot be null. Is your server offline?");


                if (WasLastExecutionAFailure)
                {
                    Logger.Info($"[Battle Metrics Provider] - Bot for Address: {information.Address} successfully fetched data after failure.");
                    LastException = null;
                    WasLastExecutionAFailure = false;
                }

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
                if (e.Message == LastException?.Message)
                    return null;

                WasLastExecutionAFailure = true;
                LastException = e;

                if (e is KeyNotFoundException keyNotFoundException)
                {
                    Logger.Error($"[Battle Metrics Provider] - BattleMetricKey is missing from Application variable configuration.");
                    return null;
                }

                if (e is HttpRequestException requestException)
                {
                    Logger.Error($"[Battle Metrics Provider] - The BattleMetric host has failed to respond.");
                    return null;
                }

                if (e is WebException webException)
                {
                    if (webException.Status == WebExceptionStatus.Timeout)
                    {
                        Logger.Error($"[Battle Metrics Provider] - Speaking with BattleMetric has timed out.");
                        return null;
                    }
                    else if (webException.Status == WebExceptionStatus.ConnectFailure)
                    {
                        Logger.Error($"[Battle Metrics Provider] - Could not connect to BattleMetric.");
                        return null;
                    }
                    else
                    {
                        Logger.Error($"[Battle Metrics Provider] - There was an error speaking with your BattleMetric server.", e);
                        return null;
                    }
                }

                if (e is ApplicationException applicationException)
                {
                    Logger.Error($"[Battle Metrics Provider] - {e.Message}");
                    return null;
                }

                Logger.Error($"[Battle Metrics Provider] - There was an error speaking with BattleMetric.", e);
                throw;
            }
        }
    }
}
