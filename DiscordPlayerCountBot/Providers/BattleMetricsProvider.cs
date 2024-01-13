using SteamQueryNet.Enums;

namespace PlayerCountBot.Providers
{
    [Name("BattleMetrics")]
    public class BattleMetricsProvider : ServerInformationProvider
    {
        public BattleMetricsProvider(BotInformation info) : base(info)
        {
        }

        public async override Task<BaseViewModel?> GetServerInformation(BotInformation information, Dictionary<string, string> applicationVariables)
        {
            var service = new BattleMetricsService();

            try
            {
                var addressAndPort = information.GetAddressAndPort();
                var server = await service.GetPlayerInformationAsync(addressAndPort.Item1, applicationVariables["BattleMetricsKey"]);

                if (server == null)
                    throw new ApplicationException("Server cannot be null. Is your server offline?");

                HandleLastException(information);

                var model = server.GetViewModel();

                if (!string.IsNullOrEmpty(model.Time) && TimeOnly.TryParse(model.Time, out var time))
                {
                    if (information.SunriseHour.HasValue && information.SunsetHour.HasValue)
                        model.SunMoon = time.Hour > information.SunriseHour && time.Hour < information.SunsetHour ? "☀️" : "🌙";

                    if (!information.SunriseHour.HasValue || !information.SunsetHour.HasValue)
                        model.SunMoon = time.Hour > 6 && time.Hour < 20 ? "☀️" : "🌙";
                }

                return model;
            }
            catch (Exception e)
            {
                HandleException(e);
                return null;
            }
        }
    }
}
