using DiscordPlayerCountBot.EnvironmentParser.Base;

namespace DiscordPlayerCountBot.EnvironmentParser;

public class BotApplicationVariableParser : EnvironmentParserBase<Dictionary<string, string>>
{
    public override string GetKey() => "BOT_APPLICATION_VARIABLES";
    public override Dictionary<string, string> ParseTyped(string? environmentVariable)
    {
        if (string.IsNullOrEmpty(environmentVariable) || string.IsNullOrWhiteSpace(environmentVariable))
            ArgumentException.ThrowIfNullOrEmpty(nameof(environmentVariable));

        if (!environmentVariable!.Contains(';') && !environmentVariable.Contains(','))
            throw new FormatException("The environment variable doesn't contain ';' and ','.");

        if (!environmentVariable.Contains("SteamAPIKey", StringComparison.OrdinalIgnoreCase) && !environmentVariable.Contains("BattleMetricsKey", StringComparison.OrdinalIgnoreCase))
            throw new FormatException("The input must contain either 'SteamAPIKey' or 'BattleMetricsKey'.");

        if (!environmentVariable.Contains(','))
            throw new FormatException("The environment variable must contain ','.");

        return environmentVariable
            .Split(';', StringSplitOptions.RemoveEmptyEntries)
            .Select(pair => pair.Split(',', 2))
            .Where(kv => kv.Length == 2)
            .ToDictionary(kv => kv[0].Trim(), kv => kv[1].Trim());
    }
}
