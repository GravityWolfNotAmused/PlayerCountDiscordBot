namespace PlayerCountBot.Configuration
{
    [Name("Standard Configuration")]
    public class StandardConfiguration : LoggableClass, IConfigurable
    {
        public StandardConfiguration() : base() { }
        public async Task<Tuple<Dictionary<string, Bot>, int>> Configure(bool shouldStart = true)
        {
            var bots = new Dictionary<string, Bot>();
            var config = new BotConfig();

            if (!File.Exists("./Config.json"))
            {
                Warn("[Standard Configuration] - Creating new config file. Please configure the Config.json file, and restart the program.");
                config.CreateDefaults();
                File.WriteAllText("./Config.json", JsonConvert.SerializeObject(config, Formatting.Indented));
                Console.ReadLine();
            }

            if (File.Exists("./Config.json"))
            {
                Info("[Standard Configuration] - Loading Config.json.");
                string fileContents = await File.ReadAllTextAsync("./Config.json");
                config = JsonHelper.DeserializeObject<BotConfig>(fileContents);

                if (config == null) throw new ApplicationException("[Standard Configuration] - You have broken the syntax of your config file.");

                Debug($"[Standard Configuration] - Config.json loaded:\n{fileContents}");

                foreach (var info in config.ServerInformation)
                {
                    var bot = new Bot(info, config.ApplicationTokens);
                    await bot.StartAsync(shouldStart);
                    bots.Add(bot.Information!.Address, bot);
                }
            }

            return new Tuple<Dictionary<string, Bot>, int>(bots, config.UpdateTime);
        }
    }
}
