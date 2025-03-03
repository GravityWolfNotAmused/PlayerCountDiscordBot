using DiscordPlayerCountBot.Bot;
using DiscordPlayerCountBot.Attributes;
using DiscordPlayerCountBot.EnvironmentParser;
using DiscordPlayerCountBot.Configuration.Base;

using Microsoft.Extensions.DependencyInjection;
using DiscordPlayerCountBot.Enums;
using DiscordPlayerCountBot.Providers.Base;

namespace DiscordPlayerCountBot.Configuration;

[Name("Docker Configuration")]
public class DockerConfiguration(IServiceProvider services) : LoggableClass, IConfigurable
{
    public async Task<Tuple<Dictionary<string, Bot.Bot>, int>> Configure(bool shouldStart = true)
    {
        EnvironmentHelper.ValidateVariables();

        var parser = services.GetRequiredService<EnvironmentParserResolver>();
        var dataProviders = services.GetServices<IServerInformationProvider>()
            .ToDictionary(value => value.GetRequiredProviderType());

        var bots = new Dictionary<string, Bot.Bot>();
        Info("[Docker Configuration] - Loading Docker Config.");

        var botNames = parser.ParseVariable<IEnumerable<string>>("BOT_NAMES");
        var botAddresses = parser.ParseVariable<IEnumerable<string>>("BOT_PUBADDRESSES");
        var botPorts = parser.ParseVariable<IEnumerable<int>>("BOT_PORTS");
        var botTokens = parser.ParseVariable<IEnumerable<string>>("BOT_DISCORD_TOKENS");
        var botStatuses = parser.ParseVariable<IEnumerable<int>>("BOT_STATUSES");
        var botTags = parser.ParseVariable<IEnumerable<bool>>("BOT_USENAMETAGS");
        var providerTypes = parser.ParseVariable<IEnumerable<int>>("BOT_PROVIDERTYPES");
        var statusFormats = parser.ParseVariable<IEnumerable<string?>>("BOT_STATUSFORMATS");
        var applicationTokens = parser.ParseVariable<Dictionary<string, string>>("BOT_APPLICATION_VARIABLES");
        var channelIds = parser.ParseVariable<IEnumerable<ulong?>>("BOT_CHANNELIDS");
        var updateTime = parser.ParseVariable<int>("BOT_UPDATE_TIME");

        var index = 0;

        foreach (var botName in botNames)
        {
            var address = botAddresses.ElementAtOrDefault(index);
            var port = botPorts.ElementAtOrDefault(index);
            var token = botTokens.ElementAtOrDefault(index) ?? throw new ApplicationException("Missing bot token.");
            var status = botStatuses.ElementAtOrDefault(index);
            var statusFormat = statusFormats.ElementAtOrDefault(index);
            var nameAsLabel = botTags.ElementAtOrDefault(index);
            var channelID = channelIds.ElementAtOrDefault(index);
            var provider = EnumHelper.GetDataProvider(providerTypes.ElementAtOrDefault(index));

            var info = new BotInformation()
            {
                Name = botName,
                Address = string.Format("{0}:{1}", address, port),
                Token = token,
                Status = status,
                StatusFormat = statusFormat,
                UseNameAsLabel = nameAsLabel,
                ChannelID = channelID,
                ProviderType = provider
            };

            var bot = new Bot.Bot(info, applicationTokens, dataProviders);
            await bot.StartAsync(shouldStart);
            bots.Add(bot.Information.Id.ToString(), bot);
            index++;
        }

        return new Tuple<Dictionary<string, Bot.Bot>, int>(bots, updateTime);
    }

    public HostEnvironment GetRequiredEnvironment()
    {
        return HostEnvironment.DOCKER;
    }
}