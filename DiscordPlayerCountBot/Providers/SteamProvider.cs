using PlayerCountBot.Extensions;

namespace PlayerCountBot.Providers
{
    [Name("Steam")]
    public class SteamProvider : ServerInformationProvider
    {
        private readonly SteamService Service;

        public SteamProvider(SteamService service)
        {
            Service = service;
        }

        public override DataProvider GetRequiredProviderType()
        {
            return DataProvider.STEAM;
        }

        public async override Task<BaseViewModel?> GetServerInformation(BotInformation information, Dictionary<string, string> applicationVariables)
        {
            try
            {
                var addressAndPort = information.GetAddressAndPort();
                var response = await Service.GetSteamApiResponse(addressAndPort.Item1, addressAndPort.Item2, applicationVariables["SteamAPIKey"]);

                if (response == null)
                    throw new ApplicationException($"Server Address: {information.Address} was not found in Steam's directory.");

                HandleLastException(information);

                var model = new SteamViewModel()
                {
                    Address = addressAndPort.Item1,
                    Port = addressAndPort.Item2,
                    Players = response.players,
                    MaxPlayers = response.max_players,
                    QueuedPlayers = response.GetQueueCount(),
                    Gametype = response.gametype,
                    Map = response.map
                };

                var serverTime = model.Gametype.Split(",")
                                    .Where(entry => entry.Contains(':') && entry.Length == 5)
                                    .FirstOrDefault();

                if (!string.IsNullOrEmpty(serverTime))
                {
                    model.Time = serverTime;

                    if (model.Time.TryGetSunMoonPhase(information.SunriseHour, information.SunsetHour, out var sunMoon))
                    {
                        model.SunMoon = sunMoon;
                    }
                }

                return model;
            }
            catch (Exception e)
            {
                HandleException(e, information.Id.ToString());
                return null;
            }
        }
    }
}
