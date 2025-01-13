using DiscordPlayerCountBot.EnvironmentParser.Base;

namespace DiscordPlayerCountBot.EnvironmentParser
{
    public class BotStatusFormatParser : EnvironmentParserBase<IEnumerable<string?>>
    {
        public override string GetKey() => "BOT_STATUSFORMATS";
        public override IEnumerable<string?> ParseTyped(string? environmentVariable)
        {
            return environmentVariable?.Split(";").Select(s => s == "null" ? null : s) ?? Enumerable.Empty<string?>();
        }
    }
}
