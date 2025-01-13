using DiscordPlayerCountBot.EnvironmentParser.Base;
using Microsoft.Extensions.DependencyInjection;

namespace PlayerCountBot.Configuration
{
    [Name("Docker Configuration")]
    public class DockerConfiguration : LoggableClass, IConfigurable
    {
        public IServiceProvider Services { get; set; }

        public DockerConfiguration(IServiceProvider services)
        {
            Services = services;
        }

        private static T ParseVariable<T>(Dictionary<string, IEnvironmentParser> parsers, string key)
        {
            if (!parsers.ContainsKey(key))
                throw new KeyNotFoundException($"Missing environment parser for: {key}");

            EnvironmentParserBase<T> parser = (EnvironmentParserBase<T>) parsers[key];

            return parser.ParseTyped(Environment.GetEnvironmentVariable(key));
        }

        public async Task<Tuple<Dictionary<string, Bot>, int>> Configure(bool shouldStart = true)
        {
            EnvironmentHelper.ValidateVariables();
            var bots = new Dictionary<string, Bot>();
            Info("[Docker Configuration] - Loading Docker Config.");

            var parsers = Services.GetServices<IEnvironmentParser>()
                .ToDictionary(parser => parser.GetKey());

            if (!parsers.Any()) throw new Exception("No parsers found for Docker Configuration.");

            var botNames = ParseVariable<IEnumerable<string>>(parsers, "BOT_NAMES");
            var botAddresses = ParseVariable<IEnumerable<string>>(parsers, "BOT_PUBADDRESSES");
            var botPorts = ParseVariable<IEnumerable<int>>(parsers, "BOT_PORTS");
            var botTokens = ParseVariable<IEnumerable<string>>(parsers, "BOT_DISCORD_TOKENS");
            var botStatuses = ParseVariable<IEnumerable<int>>(parsers, "BOT_STATUSES");
            var botTags = ParseVariable<IEnumerable<bool>>(parsers, "BOT_USENAMETAGS");
            var providerTypes = ParseVariable<IEnumerable<int>>(parsers, "BOT_PROVIDERTYPES");
            var statusFormats = ParseVariable<IEnumerable<string?>>(parsers, "BOT_STATUSFORMATS");
            var applicationTokens = ParseVariable<Dictionary<string, string>>(parsers, "BOT_APPLICATION_VARIABLES");
            var channelIds = ParseVariable<IEnumerable<ulong?>>(parsers, "BOT_CHANNELIDS");
            var updateTime = ParseVariable<int>(parsers, "BOT_UPDATE_TIME");

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

                var bot = new Bot(info, applicationTokens, Services);
                await bot.StartAsync(shouldStart);
                bots.Add(bot.Information.Id.ToString(), bot);
                index++;
            }

            return new Tuple<Dictionary<string, Bot>, int>(bots, updateTime);
        }

        public HostEnvironment GetRequiredEnvironment()
        {
            return HostEnvironment.DOCKER;
        }
    }
}
