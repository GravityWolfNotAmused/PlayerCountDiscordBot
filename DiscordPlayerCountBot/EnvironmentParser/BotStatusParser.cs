using DiscordPlayerCountBot.EnvironmentParser.Base;

namespace DiscordPlayerCountBot.EnvironmentParser;

public class BotStatusParser : EnvironmentParserBase<IEnumerable<int>>
{
    public override string GetKey() => "BOT_STATUSES";
    public override IEnumerable<int> ParseTyped(string? environmentVariable)
    {
        return environmentVariable?.Split(";")
            .Select(int.Parse) ?? [];
    }
}
