using DiscordPlayerCountBot.Configuration.Base;
using DiscordPlayerCountBot.Json;
using log4net;
using Newtonsoft.Json;
using PlayerCountBot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DiscordPlayerCountBot.Configuration
{
    public class StandardConfiguration : IConfigurable
    {
        private ILog Logger = LogManager.GetLogger(typeof(StandardConfiguration));
        public async Task<Tuple<Dictionary<string, Bot>, int>> Configure()
        {
            var bots = new Dictionary<string, Bot>();
            var config = new BotConfig();

            if (!File.Exists("./Config.json"))
            {
                Logger.Warn("[Standard Configuration] - Creating new config file. Please configure the Config.json file, and restart the program.");
                config.CreateDefaults();
                File.WriteAllText("./Config.json", JsonConvert.SerializeObject(config, Formatting.Indented));
                Console.ReadLine();
            }

            if (File.Exists("./Config.json"))
            {
                Logger.Info("[Standard Configuration] - Loading Config.json.");
                string fileContents = await File.ReadAllTextAsync("./Config.json");
                config = JsonHelper.DeserializeObject<BotConfig>(fileContents);

                if (config == null) throw new ApplicationException("[Standard Configuration] - You have broken the syntax of your config file.");

                Logger.Debug($"[Standard Configuration] - Config.json loaded:\n{fileContents}");

                foreach (var info in config.ServerInformation)
                {
                    var bot = new Bot(info, config.ApplicationTokens);
                    await bot.StartAsync();
                    bots.Add(bot.Information.Address, bot);
                }
            }
            return new Tuple<Dictionary<string, Bot>, int>(bots, config.UpdateTime);
        }
    }
}
