using Microsoft.Extensions.DependencyInjection;
using System.Net.WebSockets;
using System.Security;
using System.Timers;
using Timer = System.Timers.Timer;

namespace PlayerCountBot
{
    public class UpdateController : LoggableClass
    {
        private HostEnvironment HostEnvType = HostEnvironment.STANDARD;

        private Dictionary<HostEnvironment, IConfigurable> HostingEnvironments { get; set; } = new();
        private Dictionary<string, Bot> Bots = new();

        private int Time = 30;
        private Timer? Timer;

        public UpdateController(IServiceProvider services)
        {
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);

            HostingEnvironments = services.GetServices<IConfigurable>()
                                          .ToDictionary(value => value.GetRequiredEnvironment());

            try
            {
                var IsDocker = Environment.GetEnvironmentVariable("ISDOCKER") != null && bool.Parse(Environment.GetEnvironmentVariable("ISDOCKER") ?? "false");
                HostEnvType = IsDocker ? HostEnvironment.DOCKER : HostEnvironment.STANDARD;
            }
            catch (Exception e)
            {
                if (e is ArgumentException || e is FormatException)
                {
                    Error("Error while parsing ISDOCKER variable. Please check your docker file, and fix your changes.", null, e);
                }

                if (e is SecurityException)
                {
                    Error("A security error has happened when trying to fet ISDOCKER variable.", null, e);
                }

                HostEnvType = HostEnvironment.STANDARD;
            }
        }

        public async Task LoadConfig()
        {
            var configurationType = HostingEnvironments[HostEnvType];
            var config = await configurationType.Configure();

            Bots = config.Item1;
            Time = config.Item2;

            Info($"Created: {Bots.Count} bot(s) that update every {Time} seconds.");
        }

        public async Task UpdatePlayerCounts()
        {
            foreach (var bot in Bots.Values)
            {
                try
                {
                    await bot.UpdateAsync();
                }
                catch (Exception ex)
                {
                    if (ex is OperationCanceledException canceledException)
                    {
                        Warn($"Discord host connection was closed. Resetting connection.", bot.Information.Id.ToString());
                        return;
                    }

                    if (ex is WebSocketException socketException)
                    {
                        Warn($"Web socket was found to be in a invalid state.", bot.Information.Id.ToString());
                        return;
                    }

                    Error($"Please send crash log to https://discord.gg/FPXdPjcX27.", bot.Information.Id.ToString(), ex);
                }
            }
        }

        private async void OnTimerExecute(object? source, ElapsedEventArgs e)
        {
            await UpdatePlayerCounts();
        }

        public async Task MainAsync()
        {
            await LoadConfig();
            Start();
            await Task.Delay(-1);
        }

        private void Start()
        {
            Timer = new Timer(Time * 1000);
            Timer.Elapsed += OnTimerExecute;
            Timer.AutoReset = true;
            Timer.Enabled = true;
            Timer.Start();
            Info($"Update timer started");
            Info($"Timer set to go off every: {Time} second(s)");
        }

        public async void OnProcessExit(object? sender, EventArgs e)
        {
            foreach (var entry in Bots)
            {
                Warn($"Stoping bot: {entry.Key}");
                await entry.Value.StopAsync();
            }
        }
    }
}
