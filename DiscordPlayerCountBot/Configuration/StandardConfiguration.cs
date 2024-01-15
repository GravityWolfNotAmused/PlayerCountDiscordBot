namespace PlayerCountBot.Configuration
{
    [Name("Standard Configuration")]
    public class StandardConfiguration : LoggableClass, IConfigurable
    {
        public IServiceProvider Services { get; set; }

        public StandardConfiguration(IServiceProvider services)
        {
            Services = services;
        }

        public async Task<Tuple<Dictionary<string, Bot>, int>> Configure(bool shouldStart = true)
        {
            var bots = new Dictionary<string, Bot>();
            var config = new BotConfig();

            if (!File.Exists("./Config.json"))
            {
                Warn("Creating new config file. Please configure the Config.json file, and restart the program.");
                config.CreateDefaults();
                File.WriteAllText("./Config.json", JsonConvert.SerializeObject(config, Formatting.Indented));
                Console.ReadLine();
                Environment.Exit(0);
            }

            if (File.Exists("./Config.json"))
            {
                Info("Loading Config.json.");
                var fileContents = await File.ReadAllTextAsync("./Config.json");
                config = JsonHelper.DeserializeObject<BotConfig>(fileContents);

                if (config == null) throw new ApplicationException("You have broken the syntax of your config file.");

                Info($"Config.json loaded:\n{fileContents}");

                foreach (var info in config.ServerInformation)
                {
                    var bot = new Bot(info, config.ApplicationTokens, Services);
                    await bot.StartAsync(shouldStart);
                    bots.Add(bot.Information!.Id.ToString(), bot);
                }
            }

            return new Tuple<Dictionary<string, Bot>, int>(bots, config.UpdateTime);
        }

        public HostEnvironment GetRequiredEnvironment()
        {
            return HostEnvironment.STANDARD;
        }
    }
}
