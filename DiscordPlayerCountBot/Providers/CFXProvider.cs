﻿namespace PlayerCountBot.Providers
{
    [Name("CFX")]
    public class CFXProvider : ServerInformationProvider
    {
        public readonly CFXService Service;

        public CFXProvider(CFXService service)
        {
            Service = service;
        }

        public override DataProvider GetRequiredProviderType()
        {
            return DataProvider.CFX;
        }

        public async override Task<BaseViewModel?> GetServerInformation(BotInformation information, Dictionary<string, string> applicationVariables)
        {
            try
            {
                var playerInfo = await Service.GetPlayerInformationAsync(information.Address);
                var serverInfo = await Service.GetServerInformationAsync(information.Address);
                var addressAndPort = information.GetAddressAndPort();

                if (playerInfo == null)
                    throw new ApplicationException("Player Information cannot be null. Is your server offline?");

                if (serverInfo == null)
                    throw new ApplicationException("Server Information cannot be null. Is your server offline?");

                HandleLastException(information);

                return new CFXViewModel()
                {
                    Address = addressAndPort.Item1,
                    Players = playerInfo.Count,
                    MaxPlayers = serverInfo.GetMaxPlayers(),
                    Port = addressAndPort.Item2,
                    QueuedPlayers = 0
                };
            }
            catch (Exception e)
            {
                HandleException(e, information.Id.ToString());
                return null;
            }
        }
    }
}
