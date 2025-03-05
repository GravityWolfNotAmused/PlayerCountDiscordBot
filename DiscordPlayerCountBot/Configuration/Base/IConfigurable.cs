using DiscordPlayerCountBot.Enums;

namespace DiscordPlayerCountBot.Configuration.Base;

public interface IConfigurable
{
    public HostEnvironment GetRequiredEnvironment();
    Task<Tuple<Dictionary<string, Bot.Bot>, int>> Configure(bool shouldStart = true);
}