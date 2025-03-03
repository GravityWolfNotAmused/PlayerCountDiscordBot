using DiscordPlayerCountBot.EnvironmentParser.Base;

namespace DiscordPlayerCountBot.EnvironmentParser;

public class BotAddressParser : EnvironmentParserBase<IEnumerable<string>>
{
    public override string GetKey() => "BOT_PUBADDRESSES";
    public override IEnumerable<string> ParseTyped(string? environmentVariable)
    {
        return environmentVariable?.Split(";") ?? Enumerable.Empty<string>();
    }
}