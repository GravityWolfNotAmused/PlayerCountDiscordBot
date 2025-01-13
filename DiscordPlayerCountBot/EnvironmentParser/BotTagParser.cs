using DiscordPlayerCountBot.EnvironmentParser.Base;

namespace DiscordPlayerCountBot.EnvironmentParser
{
    public class BotTagParser : EnvironmentParserBase<IEnumerable<bool>>
    {
        public override string GetKey() => "BOT_USENAMETAGS";
        public override IEnumerable<bool> ParseTyped(string? environmentVariable)
        {
            return environmentVariable?.Split(";").Select(bool.Parse) ?? Enumerable.Empty<bool>();
        }
    }
}
