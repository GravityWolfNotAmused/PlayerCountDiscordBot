namespace PlayerCountBot.Providers
{
    [Name("CFX")]
    public class CFXProvider : ServerInformationProvider
    {
        public CFXProvider(BotInformation info) : base(info)
        {
        }

        public async override Task<BaseViewModel?> GetServerInformation(BotInformation information, Dictionary<string, string> applicationVariables)
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
                HandleException(e);
                return null;
            }
        }
    }
}
