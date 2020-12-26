using Discord.WebSocket;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace VPPPlayerCount
{
    class BotConfig
    {
        [JsonProperty]
        public int _updateTime { get; set; }

        [JsonProperty]
        public string _steamAPIKey { get; set; }

        [JsonProperty]
        public List<DayZServerBot> _serverInformation;

        public BotConfig()
        {
            _serverInformation = new List<DayZServerBot>();
        }

        public void CreateDefaults()
        {
            _serverInformation.Add(new DayZServerBot("VPPTestBot", "127.0.0.1:2532", "DiscordKeyHere"));
            _updateTime = 30;
            _steamAPIKey = "SteamAPIKeyHere";
        }
        public List<string> GetAddresses()
        {
            List<string> addresses = new List<string>();

            foreach(DayZServerBot bot in _serverInformation)
            {
                string ipAddress = bot.botAddress.Split(":")[0];

                if (!addresses.Contains(ipAddress))
                {
                    addresses.Add(ipAddress);
                }
            }
            return addresses;
        }
    }

    class DayZServerBot
    {
        public string botName { get; set; }
        public string botAddress { get; set; }
        public string discordAPIKey { get; set; }

        public DayZServerBot(string name, string address, string discordKey)
        {
            botName = name;
            botAddress = address;
            discordAPIKey = discordKey;
        }
    }

    class SteamServerListResponse
    {
        [JsonProperty]
        public SteamServerListSubClass response { get; }

        public SteamServerListResponse()
        {
            response = new SteamServerListSubClass();
        }

        public SteamApiResponseData GetServerDataByPort(string port)
        {
            return response.GetAddressDataByPort(port);
        }
    }

    class SteamServerListSubClass
    {
        [JsonProperty]
        public List<SteamApiResponseData> servers { get; }

        public SteamServerListSubClass()
        {
            servers = new List<SteamApiResponseData>();
        }

        public SteamApiResponseData GetAddressDataByPort(string port)
        {
            foreach(SteamApiResponseData data in servers)
            {
                if(data.addr.Split(":")[1] == port)
                {
                    return data;
                }
            }

            return null;
        }
    }

    class SteamApiResponseData
    {
        [JsonProperty]
        public string addr { get; set; }

        [JsonProperty]
        public int gameport { get; set; }

        [JsonProperty]
        public string steamid { get; set; }

        [JsonProperty]
        public string name { get; set; }

        [JsonProperty]
        public int appid { get; set; }

        [JsonProperty]
        public string gamedir { get; set; }

        [JsonProperty]
        public string version { get; set; }

        [JsonProperty]
        public string product { get; set; }

        [JsonProperty]
        public int region { get; set; }

        [JsonProperty]
        public int players { get; set; }

        [JsonProperty]
        public int max_players{ get; set; }

        [JsonProperty]
        public int bot { get; set; }

        [JsonProperty]
        public string map { get; set; }

        [JsonProperty]
        public bool secure { get; set; }

        [JsonProperty]
        public bool dedicated { get; set; }

        [JsonProperty]
        public string os { get; set; }

        [JsonProperty]
        public string gametype{ get; set; }


        public string GetQueueCount()
        {
            string[] splitData = gametype.Split(",");

            if(splitData.Length > 0)
            {
                foreach(string str in splitData)
                {
                    if(str.Contains("lqs"))
                    {
                        string queueCount = str.Replace("lqs", "");

                        return queueCount;
                    }
                }
            }

            return "";
        }
}

    class VPPPlayerCountBots
    {
        BotConfig config;
        Dictionary<string, SteamServerListResponse> responseData;

        Dictionary<string, DiscordSocketClient> serverBots;

        System.Timers.Timer timer;
        
        static VPPPlayerCountBots bots;

        public VPPPlayerCountBots()
        {
            config = new BotConfig();
            serverBots = new Dictionary<string, DiscordSocketClient>();
            responseData = new Dictionary<string, SteamServerListResponse>();
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
                foreach(DayZServerBot bot in config._serverInformation)
                {
                    DiscordSocketClient discordBot = new DiscordSocketClient();
                    await discordBot.LoginAsync(Discord.TokenType.Bot, bot.discordAPIKey);
                    await discordBot.SetGameAsync("Starting Bots");
                    await discordBot.StartAsync();
                    serverBots.Add(bot.botAddress, discordBot);
                }

                await UpdatePlayerCounts();
            }
        }

        public async Task UpdatePlayerCounts()
        {
            List<string> addresses = config.GetAddresses();
            if (addresses.Count == 0)
            {
                Console.WriteLine("No Addresses to request data.");
            }

            foreach (string address in addresses)
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"https://api.steampowered.com/IGameServersService/GetServerList/v1/?key={config._steamAPIKey}&filter=\\addr\\{address}&limit=10");
                string responseDataStr = string.Empty;

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            responseDataStr = reader.ReadToEnd();
                        }
                    }
                }
                SteamServerListResponse responseObject = JsonConvert.DeserializeObject<SteamServerListResponse>(responseDataStr);

                if (responseObject != null)
                {
//                    Console.WriteLine($"Response Data for address at {address}: {responseObject.response.servers.Count}");

                    if (responseData.ContainsKey(address))
                        responseData.Remove(address);

                    responseData.Add(address, responseObject);
                }

                foreach (KeyValuePair<string, DiscordSocketClient> entry in serverBots)
                {
                    string serverAddress = entry.Key.Split(":")[0];
                    string serverPort = entry.Key.Split(":")[1];

                    SteamServerListResponse responseList = responseData[serverAddress];

                    if (responseList != null)
                    {
                        SteamApiResponseData data = responseList.GetServerDataByPort(serverPort);
                        string playersInQueue = data.GetQueueCount();

                        if (data != null)
                        {
                            DiscordSocketClient client = entry.Value;

                            if (client != null)
                            {
                                string gameStatus = $"{data.players}/{data.max_players}";
                                string queueCount = data.GetQueueCount();
                                if (queueCount != string.Empty && queueCount != "0")
                                {
                                    gameStatus += $" - {queueCount} In Queue";
                                }

                                await client.SetGameAsync(gameStatus);
                            }
                        }
                    }
                }
            }
        }

        private async void OnTimerExecute(Object source, ElapsedEventArgs e)
        {
            //Console.WriteLine("Timer executed! Updating");

            await UpdatePlayerCounts();
        }

        public async Task MainAsync()
        {
            await LoadConfig();

            //Console.WriteLine("Starting Bot Timer.");
            timer = new System.Timers.Timer(config._updateTime * 1000);
            timer.Elapsed += OnTimerExecute;
            timer.AutoReset = true;
            timer.Enabled = true;
            Console.WriteLine($"Timer set to go off every: {config._updateTime} second(s)");
            timer.Start();

            for(; ;)
            {
                Thread.Sleep(100);
            }
        }

        private async void OnProcessExit(object sender, EventArgs e)
        {
            foreach(KeyValuePair<string, DiscordSocketClient> entry in serverBots)
            {
                DiscordSocketClient bot = entry.Value;

                if(bot != null)
                {
                    await bot.StopAsync();
                }
            }
        }

        static void Main(string[] args)
        {
            bots = new VPPPlayerCountBots();
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(bots.OnProcessExit);

            bots.MainAsync().GetAwaiter().GetResult();
        }
    }
}
