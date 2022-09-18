using Discord;
using Discord.WebSocket;
using DiscordPlayerCountBot;
using DiscordPlayerCountBot.Enum;
using DiscordPlayerCountBot.Http;
using DiscordPlayerCountBot.Providers;
using DiscordPlayerCountBot.Providers.Base;
using log4net;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlayerCountBot
{
    public class Bot
    {
        public BotInformation Information { get; set; }
        public DiscordSocketClient DiscordClient { get; set; }
        public Dictionary<int, IServerInformationProvider> DataProviders { get; set; } = new();
        public Dictionary<string, string> ApplicationTokens { get; set; } = new();

        private ILog Logger = LogManager.GetLogger(typeof(Bot));

        public Bot(BotInformation info, Dictionary<string, string> applicationTokens)
        {
            if (info is null) throw new ArgumentNullException(nameof(info));
            if (applicationTokens is null) throw new ArgumentException(nameof(applicationTokens));

            ApplicationTokens = applicationTokens;
            Information = info;

            DiscordClient = new DiscordSocketClient(new DiscordSocketConfig()
            {
                HandlerTimeout = null
            });

            InitDataProviders();
        }

        public void InitDataProviders()
        {
            DataProviders.Add((int)DataProvider.STEAM, new SteamProvider());
            DataProviders.Add((int)DataProvider.CFX, new CFXProvider());
            DataProviders.Add((int)DataProvider.MINECRAFT, new MinecraftProvider());
            DataProviders.Add((int)DataProvider.BATTLEMETRICS, new BattleMetricsProvider());
        }

        public async Task StartAsync(bool shouldStart)
        {
            if (Information.Address.Contains("hostname") || Information.Address.Contains("localhost"))
            {
                Information.Address = await AddressHelper.ResolveAddress(Information.Address);
            }

            Logger.Info($"[Bot] - Loaded {Information.Name} at address and port: {Information.Address}, {Information.ProviderType}");
            await DiscordClient.LoginAndStartAsync(Information.Token, Information.Address, shouldStart);
        }

        public async Task StopAsync()
        {
            await DiscordClient.StopAsync();
        }

        public async Task UpdateAsync()
        {
            var dataProviderType = EnumHelper.GetDataProvider(Information.ProviderType);

            if (dataProviderType != Information.ProviderType)
            {
                Logger.Warn($"[Bot] - Config for bot at address: {Information.Address} has an invalid provider type: {Information.ProviderType}");
            }

            var activityInteger = EnumHelper.GetActivityType(Information.Status);

            if (Information.Status != activityInteger)
            {
                Logger.Warn($"[Bot] - Config for bot at address: {Information.Address} has an invalid activity type: {Information.Status}");
            }

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
