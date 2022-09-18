using DiscordPlayerCountBot.Providers.Base;
using DiscordPlayerCountBot.Services;
using PlayerCountBot;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using DiscordPlayerCountBot.Attributes;
using DiscordPlayerCountBot.ViewModels;

namespace DiscordPlayerCountBot.Providers
{
    [Name("Minecraft")]
    public class MinecraftProvider : ServerInformationProvider
    {
        public async override Task<BaseViewModel?> GetServerInformation(BotInformation information, Dictionary<string, string> applicationVariables)
        {
            var service = new MinecraftService();
            var addressAndPort = information.GetAddressAndPort();

            try
            {
                var response = await service.GetMinecraftServerInformationAsync(addressAndPort.Item1, addressAndPort.Item2);

                if (response == null)
                    throw new ApplicationException("Response cannot be null.");

                if (!response.Online)
                {
                    Logger.Warn($"[MinecraftProvider] - The minecraft provider states the server is offline. Server counts may not be correct.");
                }

                HandleLastException(information);

                var playerInformation = response.Players;

                if (playerInformation == null)
                    throw new ApplicationException("Failed to fetch player information from provider.");

                return response.GetViewModel();
            }
            catch (Exception e)
            {
                HandleException(e);
                return null;
            }
        }
    }
}
