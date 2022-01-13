using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Timers;

using log4net;
using Newtonsoft.Json;

namespace PlayerCountBot
{
    public class UpdateController
    {
        readonly ILog Logger = LogManager.GetLogger(typeof(UpdateController));
        readonly Dictionary<string, Bot> Bots;
        readonly bool IsDocker;

        BotConfig Config;
        Timer timer;

        public UpdateController()
        {
            IsDocker = Environment.GetEnvironmentVariable("ISDOCKER") == "true";
            Config = new BotConfig(IsDocker);
            Bots = new Dictionary<string, Bot>();
        }

        async Task LoadConfig()
        {
            if (IsDocker)
            {
                var botNames = Environment.GetEnvironmentVariable("BOT_NAMES").Split(";");
                var botAddresses = Environment.GetEnvironmentVariable("BOT_PUBADDRESSES").Split(";");
                var botPorts = Environment.GetEnvironmentVariable("BOT_PORTS").Split(";");
                var botTokens = Environment.GetEnvironmentVariable("BOT_DISCORD_TOKENS").Split(";");
                var botStatuses = Environment.GetEnvironmentVariable("BOT_STATUSES").Split(";");
                var botTags = Environment.GetEnvironmentVariable("BOT_USENAMETAGS").Split(";");
                Logger.Debug("[PlayerCountBot]:: Loading config.");

                for(int i = 0; i < botNames.Length; i++)
                {
                    var info = new BotInformation(botNames[i], botAddresses[i] + ":" + botPorts[i], botTokens[i], Int32.Parse(botStatuses[i]), bool.Parse(botTags[i]));
                    var bot = new Bot(info, Environment.GetEnvironmentVariable("STEAM_API_KEY"), IsDocker);
                    await bot.StartAsync();
                    Bots.Add(bot.Information.Address, bot);
                }
            }
            else
            {
                if (!File.Exists("./Config.json"))
                {
                    Logger.Error("[PlayerCountBot]:: Creating new config file. Please configure the Config.json file, and restart the program.");
                    Config.CreateDefaults();
                    File.WriteAllText("./Config.json", JsonConvert.SerializeObject(Config, Formatting.Indented));
                    Console.ReadLine();
                    Environment.Exit(-1);
                }

                if (File.Exists("./Config.json"))
                {
                    Logger.Info("[PlayerCountBot]:: Loading config file.");
                    string fileContents = await File.ReadAllTextAsync("./Config.json");
                    Config = JsonConvert.DeserializeObject<BotConfig>(fileContents);
                    Logger.Debug($"[PlayerCountBot]:: Config.json loaded: {fileContents}");

                    foreach (var info in Config.ServerInformation)
                    {
                        var bot = new Bot(info, Config.SteamAPIKey);
                        await bot.StartAsync();
                        Bots.Add(bot.Information.Address, bot);
                    }
                }
            }

            Logger.Info($"[PlayerCountBot]:: Created: {Bots.Count} bot(s).");
        }

        public async Task UpdatePlayerCounts()
        {
            foreach(var bot in Bots.Values)
            {
                await bot.UpdateAsync();
            }
        }

        private async void OnTimerExecute(Object source, ElapsedEventArgs e)
        {
            try
            {
                await UpdatePlayerCounts();
            }
            catch (Exception ex)
            {
                Logger.Error(ex.StackTrace);
                Logger.Error(ex.Message);
                Logger.Error($"[PlayerCountBot]:: Restarting due to error, Please send crash log to https://discord.gg/FPXdPjcX27.");

                Process.Start(AppDomain.CurrentDomain.FriendlyName);
                Environment.Exit(0);
            }
        }

        public async Task MainAsync()
        {
            await LoadConfig();
            Start();

            for (; ; )
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void Start()
        {
            int time = 30;

            try
            {
                var timeValue = Environment.GetEnvironmentVariable("BOT_UPDATE_TIME");

                if (IsDocker)
                {
                    time = Int32.Parse(timeValue) * 1000;
                }
            }
            catch (Exception ex)
            {
                if (ex is FormatException || ex is OverflowException || ex is ArgumentNullException)
                {
                    Logger.Error($"[PlayerCountBot]:: The time specified is not valid or is missing from the configuration file. Using 30 seconds.");
                }
                else
                {
                    throw;
                }
            }

            if (!IsDocker)
            {
                time = Config.UpdateTime * 1000;
            }

            timer = new Timer(time);
            timer.Elapsed += OnTimerExecute;
            timer.AutoReset = true;
            timer.Enabled = true;
            timer.Start();
            Logger.Info($"[PlayerCountBot]:: Update timer started");
            Logger.Info($"[PlayerCountBot]:: Timer set to go off every: {time/1000} second(s)");
        }

        public async void OnProcessExit(object sender, EventArgs e)
        {
            foreach (KeyValuePair<string, Bot> entry in Bots)
            {
                Logger.Warn($"[PlayerCountBot]:: Stoping bot: {entry.Key}");
                await entry.Value.StopAsync();
            }
        }
    }
}
