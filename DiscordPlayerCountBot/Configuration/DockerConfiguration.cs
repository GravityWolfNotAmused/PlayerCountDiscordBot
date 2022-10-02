namespace PlayerCountBot.Configuration
{

    [Name("Docker Configuration")]
    public class DockerConfiguration : LoggableClass, IConfigurable
    {
        public DockerConfiguration() : base() { }
        public async Task<Tuple<Dictionary<string, Bot>, int>> Configure(bool shouldStart = true)
        {
            var bots = new Dictionary<string, Bot>();
            Info("[Docker Configuration] - Loading Docker Config.");

            var variables = Environment.GetEnvironmentVariables();
            var hasRequiredVariablesTuple = EnvironmentHelper.ValidateVariables();

            if (!hasRequiredVariablesTuple.Item1)
                throw new ApplicationException($"Missing required variable(s) from docker configuration. {string.Join(',', hasRequiredVariablesTuple.Item2)}");

            var botNames = variables["BOT_NAMES"]?.ToString()?.Split(";");
            var botAddresses = variables["BOT_PUBADDRESSES"]?.ToString()?.Split(";");
            var botPorts = variables["BOT_PORTS"]?.ToString()?.Split(";");
            var botTokens = variables["BOT_DISCORD_TOKENS"]?.ToString()?.Split(";");
            var botStatuses = variables["BOT_STATUSES"]?.ToString()?.Split(";");
            var providerTypes = variables["BOT_PROVIDERTYPES"]?.ToString()?.Split(";");

            List<string?> statusFormats = new();

            if (variables.Contains("BOT_STATUSFORMATS"))
            {
                List<string> formats = variables["BOT_STATUSFORMATS"]!.ToString()!.Split(";")!.ToList();

                foreach (string format in formats)
                {
                    if (format == "null")
                    {
                        statusFormats.Add(null);
                    }
                    else
                    {
                        statusFormats.Add(format);
                    }
                }
            }

            var applicationTokens = new Dictionary<string, string>();

            if (variables.Contains("BOT_APPLICATION_VARIABLES"))
            {
                var applicationTokensPairs = variables["BOT_APPLICATION_VARIABLES"]!.ToString()!.Split(";");

                foreach (var token in applicationTokensPairs)
                {
                    var keyValueSplit = token.Split(',');
                    var key = keyValueSplit[0];
                    var value = keyValueSplit[1];

                    applicationTokens.Add(key, value);
                }
            }

            var channelIDs = new List<ulong?>();

            if (variables.Contains("BOT_CHANNELIDS"))
            {
                var channelIDStrings = variables["BOT_CHANNELIDS"]!.ToString()!.Split(";");

                foreach (var channelIDString in channelIDStrings)
                {
                    try
                    {
                        channelIDs.Add(Convert.ToUInt64(channelIDString));
                    }
                    catch (Exception e)
                    {
                        if (e is FormatException || e is OverflowException)
                        {
                            Error($"Could not parse Channel ID: {channelIDString}", e);
                        }

                        throw new Exception(e.Message, e);
                    }
                }
            }

            for (int i = 0; i < botNames?.Length; i++)
            {
                var activity = int.Parse(botStatuses?[i] ?? "0");
                ulong? channelID = null;

                if (i < channelIDs.Count)
                    channelID = channelIDs[i];

                var info = new BotInformation()
                {
                    Name = botNames[i],
                    Address = botAddresses?[i] + ":" + botPorts?[i],
                    Token = botTokens?[i] ?? throw new ApplicationException("Missing bot token."),
                    Status = activity,
                    StatusFormat = i < statusFormats.Count ? statusFormats[i] : null,
                    ChannelID = channelID ?? null,
                    ProviderType = EnumHelper.GetDataProvider(int.Parse(providerTypes?[i] ?? "0"))
                };

                var bot = new Bot(info, applicationTokens);
                await bot.StartAsync(shouldStart);
                bots.Add(bot.Information!.Id.ToString(), bot);
            }

            return new Tuple<Dictionary<string, Bot>, int>(bots, int.Parse(Environment.GetEnvironmentVariable("BOT_UPDATE_TIME") ?? "30"));
        }
    }
}
