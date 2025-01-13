namespace DiscordPlayerCountBot.Configuration;

public static class EnvironmentHelper
{
    public static void ValidateVariables()
    {
        var variables = Environment.GetEnvironmentVariables();
        var requiredVariableNames = new List<string>() { "BOT_NAMES", "BOT_PUBADDRESSES", "BOT_PORTS", "BOT_DISCORD_TOKENS", "BOT_STATUSES", "BOT_USENAMETAGS", "BOT_PROVIDERTYPES" };
        var listOfMissingVariableNames = new List<string>();

        var hasAllRequiredVariables = requiredVariableNames.All(name =>
        {
            var containsVariable = variables.Contains(name);

            if (!containsVariable)
            {
                listOfMissingVariableNames.Add(name);
            }

            return containsVariable;
        });

        if (!hasAllRequiredVariables)
            throw new ApplicationException($"Missing required variable(s) from docker configuration. {string.Join(',', listOfMissingVariableNames)}");

    }
}