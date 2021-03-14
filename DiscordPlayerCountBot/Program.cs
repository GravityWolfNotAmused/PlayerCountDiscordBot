using Discord.WebSocket;
using Newtonsoft.Json;
using PlayerCountBot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace PlayerCountBots
{
    class PlayerCountBots
    {
        BotConfig config;
        Dictionary<string, SteamServerListResponse> responseData;
        Dictionary<string, DiscordSocketClient> serverBots;

        System.Timers.Timer timer;

        static PlayerCountBots bots;

        public PlayerCountBots()
        {
            config = new BotConfig();
            serverBots = new Dictionary<string, DiscordSocketClient>();
            responseData = new Dictionary<string, SteamServerListResponse>();
            Console.ForegroundColor = ConsoleColor.White;
        }

        async Task LoadConfig()
        {
            if (!File.Exists("./Config.json"))
            {
                Console.WriteLine("Creating new config file. Please configure the Config.json file, and restart the program.");
                config.CreateDefaults();
                File.WriteAllText("./Config.json", JsonConvert.SerializeObject(config, Formatting.Indented));
                Console.ReadLine();
                Environment.Exit(-1);
            }

            if (File.Exists("./Config.json"))
            {
                Console.WriteLine("Loading config file.");
                string fileContents = File.ReadAllText("./Config.json");
                config = JsonConvert.DeserializeObject<BotConfig>(fileContents);
                Console.WriteLine($"Config.json loaded: {fileContents}");
                foreach (DayZServerBot bot in config._serverInformation)
                {
                    DiscordSocketClient discordBot = new DiscordSocketClient();
                    await discordBot.LoginAsync(Discord.TokenType.Bot, bot.discordBotToken);
                    await discordBot.SetGameAsync("Starting Bot watching: " + bot.botAddress);
                    await discordBot.StartAsync();
                    serverBots.Add(bot.botAddress, discordBot);
                }

                Console.WriteLine("Calling first update");
                await UpdatePlayerCounts();
            }
        }

        public async Task UpdatePlayerCounts()
        {
            List<string> addresses = config.GetAddresses();
            if (addresses.Count == 0)
            {
                if (config._isDebug)
                {
                    Console.WriteLine("No Addresses to request data.");
                }
            }

            if (config._isDebug)
            {
                Console.WriteLine("Printing addresses:");
                addresses.ToList().ForEach(i => Console.WriteLine(i));
            }

            foreach (string address in addresses)
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"https://api.steampowered.com/IGameServersService/GetServerList/v1/?key={config._steamAPIKey}&filter=\\addr\\{address}");

                if (config._isDebug)
                    Console.WriteLine("Response Received");

                string responseDataStr = string.Empty;

                try
                {
                    using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
                    {
                        using (Stream stream = response.GetResponseStream())
                        {
                            using (StreamReader reader = new StreamReader(stream))
                            {
                                responseDataStr = await reader.ReadToEndAsync();
                            }
                        }
                    }
                }catch(WebException ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[WebException]:: {ex.Message}");
                    Console.WriteLine($"[WebException]:: {ex.Status.ToString()}");
                    Console.WriteLine($"[WebException]:: {ex.Response.ToString()}");
                    Console.WriteLine($"[Discord-PlayerCount-Bot]:: Update failed, steam could not be reached. Waiting 30 seconds before continuing.");
                    Console.ForegroundColor = ConsoleColor.White;


                    await Task.Delay(TimeSpan.FromSeconds(30));
                    return;
                }

                if (responseDataStr != string.Empty)
                {
                    SteamServerListResponse responseObject = JsonConvert.DeserializeObject<SteamServerListResponse>(responseDataStr);

                    if (responseObject != null)
                    {
                        if (responseData.ContainsKey(address))
                            responseData.Remove(address);

                        if (config._isDebug)
                            Console.WriteLine("Adding Address Response Data for Address: " + address);

                        responseData.Add(address, responseObject);
                    }
                }
            }

            if(config._isDebug)
            {
                Console.WriteLine("Response Data Size: " + responseData.Count);
            }

            foreach (KeyValuePair<string, DiscordSocketClient> entry in serverBots)
            {
                string serverAddress = entry.Key.Split(":")[0];
                string serverPort = entry.Key.Split(":")[1];


                if (config._isDebug)
                    Console.WriteLine("Updating bot that is watching address: " + serverAddress + ":" + serverPort);

                if (responseData.ContainsKey(serverAddress))
                {
                    SteamServerListResponse responseList = responseData[serverAddress];

                    if (responseList != null)
                    {
                        SteamApiResponseData data = responseList.GetServerDataByPort(serverPort);

                        if (data != null)
                        {
                            string playersInQueue = data.GetQueueCount();

                            DiscordSocketClient client = entry.Value;

                            if (client != null)
                            {
                                string gameStatus = $"{data.players}/{data.max_players}";
                                string queueCount = data.GetQueueCount();
                                if (queueCount != string.Empty && queueCount != "0")
                                {
                                    gameStatus += $" - {queueCount} In Queue";
                                }


                                if (config._isDebug)
                                    Console.WriteLine("Changed Status of : " + serverAddress + ", Status: " + gameStatus);

                                await client.SetGameAsync(gameStatus);
                            }
                        }
                    }
                }

            }
        }

        private async void OnTimerExecute(Object source, ElapsedEventArgs e)
        {
            try
            {
                await UpdatePlayerCounts();
            }catch(Exception ex)
            {
                using (StreamWriter writer = File.CreateText($"crash_{DateTime.Now.ToLongDateString()}.txt"))
                {
                    Console.WriteLine(ex.StackTrace);
                    Console.WriteLine(ex.Message);

                    await writer.WriteLineAsync(ex.StackTrace);
                    await writer.WriteLineAsync(ex.Message);
                    writer.Close();
                }

                Console.Error.WriteLine("Restarting due to error, Please send crash log to GravityWolf#6981 on Discord.");
                responseData.Clear();
                serverBots.Clear();
                config = null;

                await LoadConfig();
                await Start();  
            }
        }

        public async Task MainAsync()
        {
            await LoadConfig();
            await Start();

            for (; ; )
            {
                Thread.Sleep(100);
            }
        }

        private async Task Start()
        {
            //Console.WriteLine("Starting Bot Timer.");
            timer = new System.Timers.Timer(config._updateTime * 1000);
            timer.Elapsed += OnTimerExecute;
            timer.AutoReset = true;
            timer.Enabled = true;
            Console.WriteLine($"Timer set to go off every: {config._updateTime} second(s)");
            timer.Start();
        }

        private async void OnProcessExit(object sender, EventArgs e)
        {
            foreach (KeyValuePair<string, DiscordSocketClient> entry in serverBots)
            {
                DiscordSocketClient bot = entry.Value;

                if (bot != null)
                {
                    Console.WriteLine($"Stoping bot: {entry.Key}");
                    await bot.StopAsync();
                }
            }
        }

        static void Main(string[] args)
        {
            bots = new PlayerCountBots();
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(bots.OnProcessExit);

            bots.MainAsync().GetAwaiter().GetResult();
        }
    }
}
