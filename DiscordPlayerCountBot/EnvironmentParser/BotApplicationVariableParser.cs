using DiscordPlayerCountBot.EnvironmentParser.Base;

namespace DiscordPlayerCountBot.EnvironmentParser
{
    public class BotApplicationVariableParser : EnvironmentParserBase<Dictionary<string, string>>
    {
        public override string GetKey() => "BOT_APPLICATION_VARIABLES";
        public override Dictionary<string, string> ParseTyped(string? environmentVariable)
        {
            return environmentVariable?.Split(";")
                .Select(pair => pair.Split(','))
                .ToDictionary(kv => kv[0], kv => kv[1]) ?? new Dictionary<string, string>();
        }
    }
}
