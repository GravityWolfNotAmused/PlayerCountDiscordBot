namespace PlayerCountBot.Configuration
{
    public static class EnvironmentHelper
    {
        public static Tuple<bool, List<string>> ValidateVariables()
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

            return new(hasAllRequiredVariables, listOfMissingVariableNames);
        }
    }
}