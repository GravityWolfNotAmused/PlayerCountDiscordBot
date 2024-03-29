﻿namespace PlayerCountBot.Providers
{
    [Name("Steam")]
    public class SteamProvider : ServerInformationProvider
    {
        public SteamProvider(BotInformation info) : base(info)
        {
        }

        public async override Task<BaseViewModel?> GetServerInformation(BotInformation information, Dictionary<string, string> applicationVariables)
        {
            var service = new SteamService();

            try
            {
                var addressAndPort = information.GetAddressAndPort();
                var response = await service.GetSteamApiResponse(addressAndPort.Item1, addressAndPort.Item2, applicationVariables["SteamAPIKey"]);

                if (response == null)
                    throw new ApplicationException($"Server Address: {information.Address} was not found in Steam's directory.");

                HandleLastException(information);

                return new SteamViewModel()
                {
                    Address = addressAndPort.Item1,
                    Port = addressAndPort.Item2,
                    Players = response.players,
                    MaxPlayers = response.max_players,
                    QueuedPlayers = response.GetQueueCount(),
                    Gametype = response.gametype,
                    Map = response.map
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
