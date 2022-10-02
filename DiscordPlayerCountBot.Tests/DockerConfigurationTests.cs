using EnvironmentHelper = PlayerCountBot.Tests.Environment.EnvironmentHelper;

namespace PlayerCountBot.Tests;

[Collection("Configuration Test Suite")]
public class DockerConfigurationTests
{

    [Fact(DisplayName = "All Data", Timeout = 30)]
    public async Task DockerConfigurationTestWithAllData()
    {
        EnvironmentHelper.SetTestEnvironmentWithAllVariables();

        var bots = new Dictionary<string, Bot>();
        var time = -1;

        var dockerConfiguration = new DockerConfiguration();
        var configuration = await dockerConfiguration.Configure(false);

        bots = configuration.Item1;
        time = configuration.Item2;

        EnvironmentHelper.ClearTestEnvironmentVariables();

        Assert.True(time > 0, $"Time was returned from config and not zero. Actual: {time}");
        Assert.True(bots.Count > 0, $"More than one bot was created. Actual: {bots.Count}");
        Assert.True(bots.ToList()[0].Value.ApplicationTokens.Count >= 2, $"More than 2 tokens in the dictionary for the bot. Actual: {bots.ToList()[0].Value.ApplicationTokens.Count}");
        Assert.True(bots.ToList()[0].Value.ApplicationTokens.ContainsKey("SteamAPIKey"), $"Steam Key should be present.");
        Assert.True(bots.ToList()[0].Value.ApplicationTokens.ContainsKey("BattleMetricsKey"), $"Battle Metrics Key should be present.");
    }

    [Fact(DisplayName = "Duplicate addresses")]
    public async Task DockerConfigurationWithDuplicateAddresses()
    {
        EnvironmentHelper.SetTestEnvironmentWithDuplicateAddresses();

        var dockerConfiguration = new DockerConfiguration();
        var configuration = await dockerConfiguration.Configure(false);

        EnvironmentHelper.ClearTestEnvironmentVariables();
        var duplicateAddressCount = configuration.Item1.GroupBy(x => x.Value.Information!.Address)
              .Where(g => g.Count() > 1)
              .Select(y => new { Element = y.Key, Counter = y.Count() })
              .ToList();

        Assert.NotNull(duplicateAddressCount);
        Assert.True(configuration.Item1.Values.Count > 0, "Should have created more than 0 bots.");
        Assert.True(duplicateAddressCount.Any(grouping => grouping.Counter > 1), $"Should be two addresses that are the same.");
    }

    [Fact(DisplayName = "Multiple Status Formats Contains Nulls", Timeout = 30)]
    public async Task DockerConfigurationTestWithOneStatusRestAreNull()
    {
        EnvironmentHelper.SetTestEnvironmentWithOneStatusRestAreNull();

        var bots = new Dictionary<string, Bot>();
        var time = -1;

        var dockerConfiguration = new DockerConfiguration();
        var configuration = await dockerConfiguration.Configure(false);

        bots = configuration.Item1;
        time = configuration.Item2;

        foreach (var bot in bots)
        {
            var botInformation = bot.Value.Information;

            if (botInformation!.StatusFormat == null) continue;

            var statuses = botInformation!.GetFormats();

            Assert.True(statuses.Count == 0 || statuses.Count == 3, "This test should only contain bots with three statuses, or none.");

            for (int i = 0; i < 10000; i++)
            {
                var format = botInformation.GetCurrentFormat();

                Assert.True(format == null || format != "", $"Format should contain characters, or null. Value: {format}");
                Assert.False(botInformation.CurrentFormat < 0, $"Current Format cannot be a negative number.");
            }
        }

        EnvironmentHelper.ClearTestEnvironmentVariables();
    }

    [Fact(DisplayName = "Without Battle Metrics", Timeout = 30)]
    public async Task DockerConfigurationTestWithoutBattleMetrics()
    {
        EnvironmentHelper.SetTestEnvironmentWithoutBattleMetrics();

        var bots = new Dictionary<string, Bot>();
        var time = -1;

        var dockerConfiguration = new DockerConfiguration();
        var configuration = await dockerConfiguration.Configure(false);

        bots = configuration.Item1;
        time = configuration.Item2;

        EnvironmentHelper.ClearTestEnvironmentVariables();

        Assert.True(time > 0, $"Time was returned from config and not zero. Actual: {time}");
        Assert.True(bots.Count > 0, $"More than one bot was created. Actual: {bots.Count}");
        Assert.True(bots.ToList()[0].Value.ApplicationTokens.Count == 1, $"1 token in the dictionary for the bots. Actual: {bots.ToList()[0].Value.ApplicationTokens.Count}");
        Assert.True(bots.ToList()[0].Value.ApplicationTokens.ContainsKey("SteamAPIKey"), $"Steam Key should be present.");
        Assert.False(bots.ToList()[0].Value.ApplicationTokens.ContainsKey("BattleMetricsKey"), $"Battle Metrics Key should not be present.");
    }

    [Fact(DisplayName = "Without Application Variables", Timeout = 30)]
    public async Task DockerConfigurationTestWithoutApplicationVariables()
    {
        EnvironmentHelper.SetTestEnvironmentWithoutApplicationVariables();

        var bots = new Dictionary<string, Bot>();
        var time = -1;

        var dockerConfiguration = new DockerConfiguration();
        var configuration = await dockerConfiguration.Configure(false);

        bots = configuration.Item1;
        time = configuration.Item2;

        EnvironmentHelper.ClearTestEnvironmentVariables();

        Assert.True(time > 0, $"Time was returned from config and not zero. Actual: {time}");
        Assert.True(bots.Count > 0, $"More than one bot was created. Actual: {bots.Count}");
        Assert.True(bots.ToList()[0].Value.ApplicationTokens.Count == 0, $"Should be 0 variables in the dictionary. Actual: {bots.ToList()[0].Value.ApplicationTokens.Count}");
    }

    [Fact(DisplayName = "Without Steam Variable", Timeout = 30)]
    public async Task DockerConfigurationTestWithoutSteam()
    {
        EnvironmentHelper.SetTestEnvironmentWithoutSteam();

        var bots = new Dictionary<string, Bot>();
        var time = -1;

        var dockerConfiguration = new DockerConfiguration();
        var configuration = await dockerConfiguration.Configure(false);

        bots = configuration.Item1;
        time = configuration.Item2;

        EnvironmentHelper.ClearTestEnvironmentVariables();

        Assert.True(time > 0, $"Time was returned from config and not zero. Actual: {time}");
        Assert.True(bots.Count > 0, $"More than one bot was created. Actual: {bots.Count}");
        Assert.True(bots.ToList()[0].Value.ApplicationTokens.Count == 1, $"1 token in the dictionary for the bots. Actual: {bots.ToList()[0].Value.ApplicationTokens.Count}");
        Assert.True(bots.ToList()[0].Value.ApplicationTokens.ContainsKey("BattleMetricsKey"), $"Battle Metrics Key should be present.");
    }
}