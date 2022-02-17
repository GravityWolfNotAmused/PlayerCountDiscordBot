using Discord;
using Discord.WebSocket;
using log4net;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
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

            Logger.Info($"[PlayerCountBot]:: Loaded {Information.Name} at address and port: {Information.Address}");

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
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"https://api.steampowered.com/IGameServersService/GetServerList/v1/?key={Token}&filter=\\addr\\{Information.Address}");
            string responseDataStr = string.Empty;

            try
            {
                using HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
                Logger.Debug("[PlayerCountBot]:: Response Received.");
                using Stream stream = response.GetResponseStream();
                Logger.Debug("[PlayerCountBot]:: Response Steam received.");
                using StreamReader reader = new StreamReader(stream);
                responseDataStr = await reader.ReadToEndAsync();
                Logger.Debug("[PlayerCountBot]:: Response Read.");
            }
            catch (WebException ex)
            {
                Logger.Error(ex);
                Logger.Error($"[Discord-PlayerCount-Bot]:: Update failed, steam could not be reached. Waiting 30 seconds before continuing.");
                await Task.Delay(TimeSpan.FromSeconds(30));
                return;
            }

            if (responseDataStr != string.Empty)
            {
                SteamServerListResponse responseObject = JsonConvert.DeserializeObject<SteamServerListResponse>(responseDataStr);
                var serverPortAndAddress = GetAddressAndPort();

                SteamApiResponseData data = responseObject.GetServerDataByPort(serverPortAndAddress.Item2.ToString());

                if (data == null)
                {
                    Logger.Warn($"[PlayerCountBot]:: No data for address: {Information.Address}");
                    return;
                }

                if (data != null)
                {
                    string gameStatus = "";

                    if (Information.UseNameAsLabel)
                    {
                        gameStatus += $"{Information.Name} ";
                    }

                    gameStatus += $"{data.players}/{data.max_players} ";

                    //This may change in the future when other games use this bot.
                    string queueCount = data.GetQueueCount();

                    if (queueCount != string.Empty && queueCount != "0")
                    {
                        gameStatus += $"Q: {queueCount}";
                    }

                    var activityInteger = Information.Status <= 3 && Information.Status > 0 ? Information.Status : 0;

                    if (Information.Status != activityInteger)
                    {
                        Logger.Warn($"[PlayerCountBot]:: Config for bot at address: {Information.Address} has an invalid activity type: {Information.Status}");
                    }

                    var activityType = (ActivityType)(activityInteger);
                    await DiscordClient.SetGameAsync(gameStatus, null, activityType);
                    Logger.Debug($"[PlayerCountBot]:: Changed Status of: {Information.Address}, Status: {gameStatus}, Activity: {activityInteger}");

                    if(Information.ChannelID.HasValue)
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

        public Tuple<string, ushort> GetAddressAndPort()
        {
            string[] splitData = Information.Address.Split(":");
            try
            {
                ushort port = ushort.Parse(splitData[1]);
                return new Tuple<string, ushort>(splitData[0], port);
            }
            catch(Exception e)
            {
                if(e is ArgumentNullException || e is FormatException || e is OverflowException)
                {
                    var exception = new ArgumentException();
                    Logger.Error($"[PlayerCountBot]:: Port {splitData[1]} is invalid for a port value.", exception);
                    throw exception;
                }
            }

            return null;
        }
    }
}
