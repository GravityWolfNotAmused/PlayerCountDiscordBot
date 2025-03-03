using DiscordPlayerCountBot.EnvironmentParser.Base;

namespace DiscordPlayerCountBot.EnvironmentParser;

public class BotNameParser : EnvironmentParserBase<IEnumerable<string>>
{
    public override string GetKey() => "BOT_NAMES";
    public override IEnumerable<string> ParseTyped(string? environmentVariable)
    {
        return environmentVariable?.Split(";") ?? [];
    }
}