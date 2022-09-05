using DiscordPlayerCountBot.Configuration.Base;
using DiscordPlayerCountBot.Enum;
using log4net;
using PlayerCountBot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordPlayerCountBot.Configuration
{
    public class DockerConfiguration : IConfigurable
    {
        private ILog Logger = LogManager.GetLogger(typeof(DockerConfiguration));
        public async Task<Tuple<Dictionary<string, Bot>, int>> Configure()
        {
            var bots = new Dictionary<string, Bot>();
            Logger.Info("[Docker Configuration] - Loading Docker Config.");

            var botNames = Environment.GetEnvironmentVariable("BOT_NAMES")?.Split(";");
            var botAddresses = Environment.GetEnvironmentVariable("BOT_PUBADDRESSES")?.Split(";");
            var botPorts = Environment.GetEnvironmentVariable("BOT_PORTS")?.Split(";");
            var botTokens = Environment.GetEnvironmentVariable("BOT_DISCORD_TOKENS")?.Split(";");
            var botStatuses = Environment.GetEnvironmentVariable("BOT_STATUSES")?.Split(";");
            var botTags = Environment.GetEnvironmentVariable("BOT_USENAMETAGS")?.Split(";");
            var providerTypes = Environment.GetEnvironmentVariable("BOT_PROVIDERTYPES")?.Split(";");

            var channelIDs = new List<ulong?>();

            if (Environment.GetEnvironmentVariables().Contains("BOT_CHANNELIDS"))
            {
                var channelIDStrings = Environment.GetEnvironmentVariable("BOT_CHANNELIDS")?.Split(";").ToList() ?? new();

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
                            Logger.Error($"Could not parse Channel ID: {channelIDString}", e);
                        }
                    }
                }
            }

            for (int i = 0; i < botNames?.Length; i++)
            {
                var activity = 0;
                var useNameAsLabel = false;

                try
                {
                    activity = int.Parse(botStatuses?[i] ?? "0");
                    useNameAsLabel = bool.Parse(botTags?[i] ?? "false");
                }
                catch (Exception e)
                {
                    if (e is FormatException || e is ArgumentNullException || e is IndexOutOfRangeException)
                    {
                        Logger.Error(e);
                        activity = 0;
                        useNameAsLabel = false;
                    }
                }

                ulong? channelID = null;

                if (i < channelIDs.Count)
                    channelID = channelIDs[i];

                var info = new BotInformation()
                {
                    Name = botNames[i],
                    Address = botAddresses?[i] + ":" + botPorts?[i],
                    Token = botTokens?[i],
                    Status = activity,
                    UseNameAsLabel = useNameAsLabel,
                    ChannelID = channelID ?? null,
                    ProviderType = (int)EnumHelper.GetDataProvider(int.Parse(providerTypes?[i] ?? "0")),
                    SteamAPIToken = Environment.GetEnvironmentVariable("STEAM_API_KEY") ?? "",
                };

                var bot = new Bot(info);
                await bot.StartAsync();
                bots.Add(bot.Information.Address, bot);
            }
            return new Tuple<Dictionary<string, Bot>, int>(bots, int.Parse(Environment.GetEnvironmentVariable("BOT_UPDATE_TIME") ?? "30"));
        }
    }
}
