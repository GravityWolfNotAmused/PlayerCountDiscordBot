using DiscordPlayerCountBot.Attributes;
using DiscordPlayerCountBot.Bot;
using DiscordPlayerCountBot.Enums;
using DiscordPlayerCountBot.Extensions;
using DiscordPlayerCountBot.Providers.Base;
using DiscordPlayerCountBot.Services;
using DiscordPlayerCountBot.ViewModels;

namespace DiscordPlayerCountBot.Providers;

[Name("BattleMetrics")]
public class BattleMetricsProvider(BattleMetricsService service) : ServerInformationProvider
{
    public override DataProvider GetRequiredProviderType()
    {
        return DataProvider.BATTLEMETRICS;
    }

    public async override Task<BaseViewModel?> GetServerInformation(BotInformation information, Dictionary<string, string> applicationVariables)
    {
        try
        {
            var addressAndPort = information.GetAddressAndPort();

            var server = await service.GetPlayerInformationAsync(addressAndPort.Item1, applicationVariables["BattleMetricsKey"])
                ?? throw new ApplicationException("Server cannot be null. Is your server offline?");

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