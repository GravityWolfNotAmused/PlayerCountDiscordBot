using DiscordPlayerCountBot.EnvironmentParser.Base;

namespace DiscordPlayerCountBot.EnvironmentParser;

public class BotUpdateTimeParser : EnvironmentParserBase<int>
{
    public override string GetKey()
    {
        return "BOT_UPDATE_TIME";
    }

    public override int ParseTyped(string? environmentVariable)
    {
        return int.Parse(Environment.GetEnvironmentVariable("BOT_UPDATE_TIME") ?? "30");
    }
}