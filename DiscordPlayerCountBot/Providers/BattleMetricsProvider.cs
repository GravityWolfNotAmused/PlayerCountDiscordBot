using PlayerCountBot.Extensions;

namespace PlayerCountBot.Providers
{
    [Name("BattleMetrics")]
    public class BattleMetricsProvider : ServerInformationProvider
    {
        private readonly BattleMetricsService Service;

        public BattleMetricsProvider(BattleMetricsService service)
        {
            Service = service;
        }

        public override DataProvider GetRequiredProviderType()
        {
            return DataProvider.BATTLEMETRICS;
        }

        public async override Task<BaseViewModel?> GetServerInformation(BotInformation information, Dictionary<string, string> applicationVariables)
        {
            try
            {
                var addressAndPort = information.GetAddressAndPort();
                var server = await Service.GetPlayerInformationAsync(addressAndPort.Item1, applicationVariables["BattleMetricsKey"]);

                if (server == null)
                    throw new ApplicationException("Server cannot be null. Is your server offline?");

                HandleLastException(information);

                var model = server.GetViewModel();

                if (model.Time.TryGetSunMoonPhase(information.SunriseHour, information.SunsetHour, out var sunMoon))
                {
                    model.SunMoon = sunMoon;
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