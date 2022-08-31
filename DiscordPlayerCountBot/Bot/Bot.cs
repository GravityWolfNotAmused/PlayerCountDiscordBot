using Discord;
using Discord.WebSocket;
using DiscordPlayerCountBot.Providers;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace PlayerCountBot
{
    public class Bot
    {
        public BotInformation Information { get; set; }
        public string Token { get; set; }
        public DiscordSocketClient DiscordClient { get; set; }

        public bool IsDocker { get; set; }

        public ILog Logger = LogManager.GetLogger(typeof(Bot));
        public Dictionary<int, IServerInformationProvider> DataProviders { get; set; } = new Dictionary<int, IServerInformationProvider>();

        public Bot(BotInformation info, string steamAPIToken, bool isDocker = false)
        {
            if (info is null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            Information = info;
            IsDocker = isDocker;
            Token = steamAPIToken;

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

            Logger.Info($"[PlayerCountBot]:: Loaded {Information.Name} at address and port: {Information.Address}, {Information.ProviderType}");

            await DiscordClient.LoginAsync(TokenType.Bot, Information.Token);
            await DiscordClient.SetGameAsync($"[PlayerCountBot]:: Starting Bot watching: {Information.Address}");
            await DiscordClient.StartAsync();
        }

        public async Task StopAsync()
        {
            await DiscordClient.StopAsync();
        }

        public async Task UpdateAsync()
        {
            var dataProviderType = Enum.IsDefined(typeof(DataProvider), Information.ProviderType) ? Information.ProviderType : (int)DataProvider.STEAM;

            if(dataProviderType != Information.ProviderType)
            {
                Logger.Warn($"[PlayerCountBot]:: Config for bot at address: {Information.Address} has an invalid provider type: {Information.ProviderType}");
            }

            var dataProvider = DataProviders[dataProviderType];
            var serverInformation = await dataProvider.GetServerInformation(Information);

            if(serverInformation == null)
            {
                return;
            }

            var gameStatus = serverInformation.GetStatusString(Information.Name, Information.UseNameAsLabel);
            var activityInteger = Information.Status <= 3 && Information.Status > 0 ? Information.Status : 0;

            if (Information.Status != activityInteger)
            {
                Logger.Warn($"[PlayerCountBot]:: Config for bot at address: {Information.Address} has an invalid activity type: {Information.Status}");
            }

            var activityType = (ActivityType)(activityInteger);
            await DiscordClient.SetGameAsync(gameStatus, null, activityType);
            Logger.Debug($"[PlayerCountBot]:: Changed Status of: {Information.Address}, Status: {gameStatus}, Activity: {activityInteger}");

            if (Information.ChannelID.HasValue)
            {
                IDiscordClient socket = DiscordClient;
                IGuildChannel channel = (IGuildChannel)await socket.GetChannelAsync(Information.ChannelID.Value);

                if (channel is null)
                {
                    var exception = new ArgumentException();
                    Logger.Error($"Invalid Channel Id: {Information.ChannelID}, Channel was not found.", exception);
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

            try
            {
                var request = (HttpWebRequest)WebRequest.Create("http://ifconfig.me");
                request.UserAgent = "curl";
                request.Method = "GET";

                using WebResponse response = await request.GetResponseAsync();
                using var reader = new StreamReader(response.GetResponseStream());
                publicIPAddress = await reader.ReadToEndAsync();
            }
            catch (WebException ex)
            {
                Logger.Error(ex);
                Logger.Error($"[PlayerCountBot]:: Error Reaching ifconfig.me: {ex.Status}");
                return publicIPAddress;
            }

            return publicIPAddress.Replace("\n", "");
        }
    }
}
