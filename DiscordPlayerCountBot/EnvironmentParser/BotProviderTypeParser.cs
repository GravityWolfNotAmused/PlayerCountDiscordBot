using DiscordPlayerCountBot.EnvironmentParser.Base;

namespace DiscordPlayerCountBot.EnvironmentParser;

public class BotProviderTypeParser : EnvironmentParserBase<IEnumerable<int>>
{
    public override string GetKey() => "BOT_PROVIDERTYPES";
    public override IEnumerable<int> ParseTyped(string? environmentVariable)
    {
        return environmentVariable?.Split(";")
            .Select(int.Parse) ?? [];
    }
}
