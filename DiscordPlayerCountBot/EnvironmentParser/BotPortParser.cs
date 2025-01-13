using DiscordPlayerCountBot.EnvironmentParser.Base;

namespace DiscordPlayerCountBot.EnvironmentParser;

public class BotPortParser : EnvironmentParserBase<IEnumerable<int>>
{
    public override string GetKey() => "BOT_PORTS";
    public override IEnumerable<int> ParseTyped(string? environmentVariable)
    {
        return environmentVariable?.Split(";")
            .Select(int.Parse) ?? [];
    }
}
