using Microsoft.Extensions.DependencyInjection;

namespace PlayerCountBot
{

    [Name("Bot")]
    public class Bot : LoggableClass
    {
        public readonly DiscordSocketClient DiscordClient;
        public readonly BotInformation Information;
        public readonly Dictionary<DataProvider, IServerInformationProvider> DataProviders = new();
        public readonly Dictionary<string, string> ApplicationTokens = new();

        public Bot(BotInformation info, Dictionary<string, string> applicationTokens, IServiceProvider services)
        {
            if (info is null) throw new ArgumentNullException(nameof(info));
            if (applicationTokens is null) throw new ArgumentException(nameof(applicationTokens));

            ApplicationTokens = applicationTokens;
            Information = info;

            DiscordClient = new DiscordSocketClient(new DiscordSocketConfig()
            {
                HandlerTimeout = null
            });

            DataProviders = services.GetServices<IServerInformationProvider>()
                .ToDictionary(value => value.GetRequiredProviderType());
        }

        public async Task StartAsync(bool shouldStart)
        {
            if (Information!.Address.Contains("hostname") || Information.Address.Contains("localhost"))
            {
                Information.Address = await AddressHelper.ResolveAddress(Information.Address);
            }

            Info($"Loaded {Information.Name} at address and port: {Information.Address}, {(DataProvider)Information.ProviderType}");
            await DiscordClient.LoginAndStartAsync(Information.Token, Information.Address, shouldStart);
        }

        public async Task StopAsync()
        {
            await DiscordClient.StopAsync();
        }

        public async Task UpdateAsync()
        {
            var dataProviderInt = EnumHelper.GetDataProvider(Information!.ProviderType);

            if (dataProviderInt != Information.ProviderType)
            {
                Warn($"Config for bot at address: {Information.Address} has an invalid provider type: {Information.ProviderType}", Information.Id.ToString());
            }

            var activityInteger = EnumHelper.GetActivityType(Information.Status);

            if (Information.Status != activityInteger)
            {
                Warn($"Config for bot at address: {Information.Address} has an invalid activity type: {Information.Status}", Information.Id.ToString());
            }

            var dataProviderType = (DataProvider)dataProviderInt;

            if (!DataProviders.ContainsKey(dataProviderType))
                throw new Exception($"Missing Data Provider for Type: {dataProviderType}");

            var dataProvider = DataProviders[dataProviderType];
            var serverInformation = await dataProvider.GetServerInformation(Information, ApplicationTokens);

            if (serverInformation == null)
            {
                return;
            }

            var gameStatus = serverInformation.ReplaceTagsWithValues(Information.StatusFormat, Information.UseNameAsLabel, Information.Name);

            await DiscordClient.SetGameAsync(gameStatus, null, (ActivityType)activityInteger);
            await DiscordClient.SetChannelName(Information.ChannelID, gameStatus);
        }
    }
}