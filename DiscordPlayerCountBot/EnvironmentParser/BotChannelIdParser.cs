using DiscordPlayerCountBot.EnvironmentParser.Base;

namespace DiscordPlayerCountBot.EnvironmentParser;

public class BotChannelIdParser : EnvironmentParserBase<IEnumerable<ulong?>>
{
    public override string GetKey() => "BOT_CHANNELIDS";
    public override IEnumerable<ulong?> ParseTyped(string? environmentVariable)
    {
        return environmentVariable?.Split(";").Select(id => ulong.TryParse(id, out var value) ? value : (ulong?)null) ?? [];
    }
}