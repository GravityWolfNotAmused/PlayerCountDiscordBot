namespace PlayerCountBot
{

    [Name("Bot")]
    public class Bot : LoggableClass
    {
        public DiscordSocketClient DiscordClient { get; set; }
        public Dictionary<int, IServerInformationProvider> DataProviders { get; set; } = new();
        public Dictionary<string, string> ApplicationTokens { get; set; } = new();

        public Bot(BotInformation info, Dictionary<string, string> applicationTokens) : base(info)
        {
            if (info is null) throw new ArgumentNullException(nameof(info));
            if (applicationTokens is null) throw new ArgumentException(nameof(applicationTokens));

            ApplicationTokens = applicationTokens;

            DiscordClient = new DiscordSocketClient(new DiscordSocketConfig()
            {
                HandlerTimeout = null
            });

            InitDataProviders();
        }

        public void InitDataProviders()
        {
            DataProviders.Add((int)DataProvider.STEAM, new SteamProvider(Information!));
            DataProviders.Add((int)DataProvider.CFX, new CFXProvider(Information!));
            DataProviders.Add((int)DataProvider.MINECRAFT, new MinecraftProvider(Information!));
            DataProviders.Add((int)DataProvider.BATTLEMETRICS, new BattleMetricsProvider(Information!));
        }

        public async Task StartAsync(bool shouldStart)
        {
            if (Information!.Address.Contains("hostname") || Information.Address.Contains("localhost"))
            {
                Information.Address = await AddressHelper.ResolveAddress(Information.Address);
            }

            Info($"Loaded {Information.Name} at address and port: {Information.Address}, {Information.ProviderType}");
            await DiscordClient.LoginAndStartAsync(Information.Token, Information.Address, shouldStart);
        }

        public async Task StopAsync()
        {
            await DiscordClient.StopAsync();
        }

        public async Task UpdateAsync()
        {
            var dataProviderType = EnumHelper.GetDataProvider(Information!.ProviderType);

            if (dataProviderType != Information.ProviderType)
            {
                Warn($"Config for bot at address: {Information.Address} has an invalid provider type: {Information.ProviderType}");
            }

            var activityInteger = EnumHelper.GetActivityType(Information.Status);

            if (Information.Status != activityInteger)
            {
                Warn($"Config for bot at address: {Information.Address} has an invalid activity type: {Information.Status}");
            }

            var dataProvider = DataProviders[dataProviderType];
            var serverInformation = await dataProvider.GetServerInformation(Information, ApplicationTokens);

            if (serverInformation == null)
            {
                return;
            }

            var gameStatus = serverInformation.ReplaceTagsWithValues(Information.GetCurrentFormat(), Information.UseNameAsLabel, Information.Name);

            await DiscordClient.SetGameAsync(gameStatus, null, (ActivityType)activityInteger);
            await DiscordClient.SetChannelName(Information.ChannelID, gameStatus);
        }
    }
}
