using Discord;
using Discord.WebSocket;
using DiscordPlayerCountBot.Enum;
using DiscordPlayerCountBot.Http;
using DiscordPlayerCountBot.Providers;
using DiscordPlayerCountBot.Providers.Base;
using log4net;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
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
            DataProviders.Add((int)DataProvider.SCUM, new ScumProvider());
            DataProviders.Add((int)DataProvider.MINECRAFT, new MinecraftProvider());
            DataProviders.Add((int)DataProvider.BATTLEMETRICS, new BattleMetricsProvider());
        }

        public async Task StartAsync()
        {
            if (Information.Address.Contains("hostname") || Information.Address.Contains("localhost"))
            {
                string[] splitAddr = Information.Address.Split(":");
                string address = await GetHostAddress();
                string port = splitAddr[1].ToLower();
                Information.Address = address + ":" + port;
            }

            Logger.Info($"[Bot] - Loaded {Information.Name} at address and port: {Information.Address}, {Information.ProviderType}");

            await DiscordClient.LoginAsync(TokenType.Bot, Information.Token);
            await DiscordClient.SetGameAsync($"Starting: {Information.Address}");
            await DiscordClient.StartAsync();
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

            var dataProvider = DataProviders[dataProviderType];
            var serverInformation = await dataProvider.GetServerInformation(Information, ApplicationTokens);

            if (serverInformation == null)
            {
                return;
            }

            var gameStatus = serverInformation.GetStatusString(Information.Name, Information.UseNameAsLabel);
            var activityInteger = EnumHelper.GetActivityType(Information.Status);

            if (Information.Status != activityInteger)
            {
                Logger.Warn($"[Bot] - Config for bot at address: {Information.Address} has an invalid activity type: {Information.Status}");
            }

            var activityType = (ActivityType)(activityInteger);
            await DiscordClient.SetGameAsync(gameStatus, null, activityType);

            if (Information.ChannelID.HasValue)
            {
                IDiscordClient socket = DiscordClient;
                IGuildChannel channel = (IGuildChannel)await socket.GetChannelAsync(Information.ChannelID.Value);

                if (channel is null)
                {
                    var exception = new ArgumentException();
                    Logger.Error($"[Bot] - Invalid Channel Id: {Information.ChannelID}, Channel was not found.", exception);
                    throw exception;
                }

                if (channel != null)
                {
                    if (channel is ITextChannel)
                    {
                        gameStatus = gameStatus.Replace('/', '-').Replace(' ', '-').Replace(':', '-');
                    }

                    //Keep in mind there is a massive rate limit on this call that is specific to discord, and not Discord.Net
                    //2x per 10 minutes
                    //https://discord.com/developers/docs/topics/rate-limits
                    //https://www.reddit.com/r/Discord_Bots/comments/qzrl5h/channel_name_edit_rate_limit/
                    await channel.ModifyAsync(prop => prop.Name = gameStatus);
                }
            }
        }

        public async Task<string> GetHostAddress()
        {
            string publicIPAddress = string.Empty;
            var httpClient = new HttpExecuter(new HttpClient());


            try
            {
                var ipAddress = await httpClient.GET<object, string>("http://ifconfig.me");

                if (string.IsNullOrEmpty(ipAddress))
                    throw new ApplicationException("IP Address cannot be null. Host failed to resolve address.");

                return ipAddress;
            }
            catch (WebException ex)
            {
                Logger.Error($"[Bot] - Error Reaching ifconfig.me: {ex.Status}", ex);
                throw ex;
            }
        }
    }
}
