using DiscordPlayerCountBot.EnvironmentParser.Base;

namespace DiscordPlayerCountBot.EnvironmentParser;

public class BotTokenParser : EnvironmentParserBase<IEnumerable<string>>
{
    public override string GetKey() => "BOT_DISCORD_TOKENS";
    public override IEnumerable<string> ParseTyped(string? environmentVariable)
    {
        return environmentVariable?.Split(";") ?? [];
    }
}
