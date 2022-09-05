using DiscordPlayerCountBot.Providers.Base;
using DiscordPlayerCountBot.Services;
using log4net;
using PlayerCountBot;
using System;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;

namespace DiscordPlayerCountBot.Providers
{
    public class MinecraftProvider : ServerInformationProvider
    {
        private ILog Logger = LogManager.GetLogger(typeof(MinecraftProvider));
        public async override Task<GenericServerInformation?> GetServerInformation(BotInformation information)
        {
            var service = new MinecraftService();
            var addressAndPort = information.GetAddressAndPort();

            try
            {
                var response = await service.GetMinecraftServerInformationAsync(addressAndPort.Item1, addressAndPort.Item2);

                if (response == null)
                    throw new ApplicationException("Response cannot be null.");

                if(!response.Online)
                {
                    Logger.Warn($"[MinecraftProvider] - The minecraft provider states the server is offline. Server counts may not be correct.");
                }

                if (WasLastExecutionAFailure)
                {
                    Logger.Info($"[MinecraftProvider] - Bot for Address: {information.Address} successfully fetched data after failure.");
                    WasLastExecutionAFailure = false;
                }

                return new()
                {
                    Address = addressAndPort.Item1,
                    Port = addressAndPort.Item2,
                    CurrentPlayers = response.Players.Online,
                    MaxPlayers = response.Players.Max,
                    PlayersInQueue = 0
                };
            }
            catch(Exception e)
            {
                if (e.Message == LastException?.Message)
                    return null;

                WasLastExecutionAFailure = true;
                LastException = e;

                if (e is HttpRequestException requestException)
                {
                    Logger.Error($"[MinecraftProvider] - The Minecraft provider has failed to respond.");
                    return null;
                }

                if (e is WebException webException)
                {
                    if (webException.Status == WebExceptionStatus.Timeout)
                    {
                        Logger.Error($"[MinecraftProvider] - Speaking with Minecraft has timed out.");
                        return null;
                    }
                    else if (webException.Status == WebExceptionStatus.ConnectFailure)
                    {
                        Logger.Error($"[MinecraftProvider] - Could not connect to Minecraft.");
                        return null;
                    }
                    else
                    {
                        Logger.Error($"[MinecraftProvider] - There was an error speaking with your Minecraft server.", e);
                        return null;
                    }
                }

                if (e is ApplicationException applicationException)
                {
                    Logger.Error($"[MinecraftProvider] - {e.Message}");
                    return null;
                }

                Logger.Error($"[MinecraftProvider] - There was an error speaking with Minecraft.", e);
                throw;
            }
        }
    }
}
