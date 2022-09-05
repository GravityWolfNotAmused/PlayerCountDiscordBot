using System;
using System.Collections.Generic;
using System.Security;
using System.Threading.Tasks;
using System.Timers;
using DiscordPlayerCountBot.Configuration;
using DiscordPlayerCountBot.Configuration.Base;
using DiscordPlayerCountBot.Enum;
using log4net;
using PlayerCountBot;

namespace DiscordPlayerCountBot
{
    public class UpdateController
    {
        private ILog Logger = LogManager.GetLogger(typeof(UpdateController));
        private HostEnvironment HostEnvType = HostEnvironment.STANDARD;

        private Dictionary<HostEnvironment, IConfigurable> HostingEnvironments { get; set; } = new();
        private Dictionary<string, Bot> Bots = new();

        private int Time = 30;
        private Timer? Timer;

        public UpdateController()
        {
            InitHostingConfigurations();

            try
            {
                var IsDocker = Environment.GetEnvironmentVariable("ISDOCKER") != null && bool.Parse(Environment.GetEnvironmentVariable("ISDOCKER") ?? "false");
                HostEnvType = IsDocker ? HostEnvironment.DOCKER : HostEnvironment.STANDARD;
            }
            catch (Exception e)
            {
                if (e is ArgumentException || e is FormatException)
                {
                    Logger.Error("[Bot Updater] - Error while parsing ISDOCKER variable. Please check your docker file, and fix your changes.", e);
                }

                if (e is SecurityException)
                {
                    Logger.Error("[Bot Updater] - A security error has happened when trying to fet ISDOCKER variable.", e);
                }

                HostEnvType = HostEnvironment.STANDARD;
            }
        }

        public void InitHostingConfigurations()
        {
            HostingEnvironments.Add(HostEnvironment.STANDARD, new StandardConfiguration());
            HostingEnvironments.Add(HostEnvironment.DOCKER, new DockerConfiguration());
        }

        public async Task LoadConfig()
        {
            var configurationType = HostingEnvironments[HostEnvType];
            var config = await configurationType.Configure();

            Bots = config.Item1;
            Time = config.Item2;
            Logger.Info($"[Bot Updater] - Created: {Bots.Count} bot(s) that update every {Time} seconds.");
        }

        public async Task UpdatePlayerCounts()
        {
            foreach (var bot in Bots.Values)
            {
                await bot.UpdateAsync();
            }
        }

        private async void OnTimerExecute(object? source, ElapsedEventArgs e)
        {
            try
            {
                await UpdatePlayerCounts();
            }
            catch(Exception ex)
            {
                Logger.Error($"[Bot Updater] - Please send crash log to https://discord.gg/FPXdPjcX27.", ex);
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
            Timer = new Timer(Time * 1000);
            Timer.Elapsed += OnTimerExecute;
            Timer.AutoReset = true;
            Timer.Enabled = true;
            Timer.Start();
            Logger.Info($"[Bot Updater] - Update timer started");
            Logger.Info($"[Bot Updater] - Timer set to go off every: {Time} second(s)");
        }

        public async void OnProcessExit(object? sender, EventArgs e)
        {
            foreach (KeyValuePair<string, Bot> entry in Bots)
            {
                Logger.Warn($"[Bot Updater] - Stoping bot: {entry.Key}");
                await entry.Value.StopAsync();
            }
        }
    }
}
