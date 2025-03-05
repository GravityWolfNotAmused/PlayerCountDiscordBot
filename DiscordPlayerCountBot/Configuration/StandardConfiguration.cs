using DiscordPlayerCountBot.Attributes;
using DiscordPlayerCountBot.Bot;
using DiscordPlayerCountBot.Configuration.Base;
using DiscordPlayerCountBot.Enums;
using DiscordPlayerCountBot.Json;
using DiscordPlayerCountBot.Providers.Base;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordPlayerCountBot.Configuration;

[Name("Standard Configuration")]
public class StandardConfiguration(IServiceProvider services) : LoggableClass, IConfigurable
{
    public IServiceProvider Services { get; set; } = services;

    public async Task<Tuple<Dictionary<string, Bot.Bot>, int>> Configure(bool shouldStart = true)
    {
        var bots = new Dictionary<string, Bot.Bot>();
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

            Debug($"Config.json loaded:\n{fileContents}");

            var dataProviders = Services.GetServices<IServerInformationProvider>()
                .ToDictionary(value => value.GetRequiredProviderType());

            foreach (var info in config.ServerInformation)
            {
                var bot = new Bot.Bot(info, config.ApplicationTokens, dataProviders);
                await bot.StartAsync(shouldStart);
                bots.Add(bot.Information!.Id.ToString(), bot);
            }
        }

        return new Tuple<Dictionary<string, Bot.Bot>, int>(bots, config.UpdateTime);
    }

    public HostEnvironment GetRequiredEnvironment()
    {
        return HostEnvironment.STANDARD;
    }
}