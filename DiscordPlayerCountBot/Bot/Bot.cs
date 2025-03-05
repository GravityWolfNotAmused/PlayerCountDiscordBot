using DiscordPlayerCountBot.Attributes;
using DiscordPlayerCountBot.Enums;
using DiscordPlayerCountBot.Extensions;
using DiscordPlayerCountBot.Http;
using DiscordPlayerCountBot.Providers.Base;

namespace DiscordPlayerCountBot.Bot;

[Name("Bot")]
public class Bot : LoggableClass
{
    public readonly DiscordSocketClient DiscordClient;
    public readonly BotInformation Information;
    public readonly Dictionary<DataProvider, IServerInformationProvider> DataProviders = [];
    public readonly Dictionary<string, string> ApplicationTokens = [];
    public string LastKnownStatus = string.Empty;

    public Bot(BotInformation info, Dictionary<string, string> applicationTokens, Dictionary<DataProvider, IServerInformationProvider> dataProviders)
    {
        ArgumentNullException.ThrowIfNull(info);
        ArgumentNullException.ThrowIfNull(applicationTokens);

        ApplicationTokens = applicationTokens;
        Information = info;

        DiscordClient = new DiscordSocketClient(new DiscordSocketConfig()
        {
            HandlerTimeout = null
        });

        DiscordClient.Ready += DiscordClient_Ready;
        DiscordClient.SlashCommandExecuted += DiscordClient_SlashCommandExecuted;
        DataProviders = dataProviders;
    }

    private async Task DiscordClient_SlashCommandExecuted(SocketSlashCommand command)
    {
        // NOTE: I only have logic for one slash command, if I am going to add more functionality.
        // I will want to create a way to register commands, so people can expand it.

        if (string.IsNullOrEmpty(LastKnownStatus))
        {
            await command.RespondAsync("Bot does not have a status to display.");
            return;
        }

        var embed = new EmbedBuilder
        {
            Title = $"Server: {Information.Name}",
            Fields =
            [
                new()
                {
                    Name = "Players",
                    Value = LastKnownStatus
                }
            ]
        };

        await command.RespondAsync(embeds: [embed.Build()], ephemeral: true);
    }

    private async Task DiscordClient_Ready()
    {
        var globalCommand = new SlashCommandBuilder()
        {
            Name = "players",
            Description = $"This will show the player count for the server: {Information.Name}"
        };

        try
        {
            await DiscordClient.Rest.DeleteAllGlobalCommandsAsync();
        }
        catch (Exception ex)
        {
            Error("Failed to delete global commands.", Information.Id.ToString(), ex);
        }

        try
        {
            await DiscordClient.CreateGlobalApplicationCommandAsync(globalCommand.Build());
        }
        catch (Exception ex)
        {
            Error("Failed to apply global commands.", Information.Id.ToString(), ex);
        }
    }

    public async Task StartAsync(bool shouldStart)
    {
        if (Information!.Address.Contains("hostname") || Information.Address.Contains("localhost"))
        {
            Information.Address = await AddressHelper.ResolveAddress(Information.Address);
        }

        Info($"Loaded {Information.Name} ({Information.Id}) at address and port: {Information.Address}, {(DataProvider)Information.ProviderType}");
        await DiscordClient.LoginAndStartAsync(Information.Token, Information.Address, shouldStart);
    }

    public async Task StopAsync()
    {
        await DiscordClient.StopAsync();
    }

    public async Task UpdateAsync()
    {
        var dataProviderInt = EnumHelper.GetDataProvider(Information!.ProviderType);

        if (dataProviderInt != Information.ProviderType)
        {
            Warn($"Config for bot at address: {Information.Address} has an invalid provider type: {Information.ProviderType}", Information.Id.ToString());
        }

        var activityInteger = EnumHelper.GetActivityType(Information.Status);

        if (Information.Status != activityInteger)
        {
            Warn($"Config for bot at address: {Information.Address} has an invalid activity type: {Information.Status}", Information.Id.ToString());
        }

        var dataProviderType = (DataProvider)dataProviderInt;

        if (!DataProviders.TryGetValue(dataProviderType, out IServerInformationProvider? dataProvider))
            throw new Exception($"Missing Data Provider for Type: {dataProviderType}");

        var serverInformation = await dataProvider.GetServerInformation(Information, ApplicationTokens);

        if (serverInformation == null)
        {
            return;
        }

        LastKnownStatus = serverInformation.ReplaceTagsWithValues(Information.StatusFormat, Information.UseNameAsLabel, Information.Name);

        await DiscordClient.SetGameAsync(LastKnownStatus, null, (ActivityType)activityInteger);
        await DiscordClient.SetChannelName(Information.ChannelID, LastKnownStatus);
    }
}