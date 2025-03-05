using DiscordPlayerCountBot.Attributes;
using DiscordPlayerCountBot.Bot;
using DiscordPlayerCountBot.Enums;
using DiscordPlayerCountBot.Providers.Base;
using DiscordPlayerCountBot.Services.SteamQuery;
using DiscordPlayerCountBot.ViewModels;

namespace DiscordPlayerCountBot.Providers;

[Name("Steam Query")]
public class SteamQueryProvider(SteamQueryService service) : ServerInformationProvider
{
    public override DataProvider GetRequiredProviderType()
    {
        return DataProvider.STEAMQUERY;
    }

    public async override Task<BaseViewModel?> GetServerInformation(BotInformation information, Dictionary<string, string> applicationVariables)
    {
        try
        {
            var addressAndPort = information.GetAddressAndPort();
            var response = await service.GetQueryResponse(addressAndPort.Item1, addressAndPort.Item2);

            if (response == null)
            {
                throw new ApplicationException($" Failed to get a Server Information response from Steam Query.");
            }

            HandleLastException(information);

            return response;
        }
        catch (Exception e)
        {
            HandleException(e);
            return null;
        }
    }
}