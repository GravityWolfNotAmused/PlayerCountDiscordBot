using Microsoft.Extensions.DependencyInjection;
using EnvironmentHelper = DiscordPlayerCountBot.Tests.Environment.EnvironmentHelper;

namespace DiscordPlayerCountBot.Tests;

[Collection("Configuration Test Suite")]
public class DockerConfigurationTests
{

    [Fact(DisplayName = "Test Docker Configuration with all data", Timeout = 30)]
    public async Task DockerConfigurationTestWithAllData()
    {
        EnvironmentHelper.SetTestEnvironmentWithAllVariables();

        var bots = new Dictionary<string, Bot>();
        var time = -1;

        var serviceProvider = new ServiceCollection()
            .BuildServiceProvider();

        var dockerConfiguration = new DockerConfiguration(serviceProvider);
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

    [Fact(DisplayName = "Test Docker Configuration with duplicate addresses")]
    public async Task DockerConfigurationWithDuplicateAddresses()
    {
        EnvironmentHelper.SetTestEnvironmentWithDuplicateAddresses();

        var services = new ServiceCollection()
            .BuildServiceProvider();

        var dockerConfiguration = new DockerConfiguration(services);
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

    [Fact(DisplayName = "Test Docker Configuration without Battle Metrics", Timeout = 30)]
    public async Task DockerConfigurationTestWithoutBattleMetrics()
    {
        EnvironmentHelper.SetTestEnvironmentWithoutBattleMetrics();

        var bots = new Dictionary<string, Bot>();
        var time = -1;

        var services = new ServiceCollection()
            .BuildServiceProvider();

        var dockerConfiguration = new DockerConfiguration(services);
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

    [Fact(DisplayName = "Test Docker Configuration without Application Variables", Timeout = 30)]
    public async Task DockerConfigurationTestWithoutApplicationVariables()
    {
        EnvironmentHelper.SetTestEnvironmentWithoutApplicationVariables();

        var bots = new Dictionary<string, Bot>();
        var time = -1;

        var services = new ServiceCollection()
            .BuildServiceProvider();

        var dockerConfiguration = new DockerConfiguration(services);
        var configuration = await dockerConfiguration.Configure(false);

        bots = configuration.Item1;
        time = configuration.Item2;

        EnvironmentHelper.ClearTestEnvironmentVariables();

        Assert.True(time > 0, $"Time was returned from config and not zero. Actual: {time}");
        Assert.True(bots.Count > 0, $"More than one bot was created. Actual: {bots.Count}");
        Assert.True(bots.ToList()[0].Value.ApplicationTokens.Count == 0, $"Should be 0 variables in the dictionary. Actual: {bots.ToList()[0].Value.ApplicationTokens.Count}");
    }

    [Fact(DisplayName = "Test Docker Configuration without Steam variable", Timeout = 30)]
    public async Task DockerConfigurationTestWithoutSteam()
    {
        EnvironmentHelper.SetTestEnvironmentWithoutSteam();

        var bots = new Dictionary<string, Bot>();
        var time = -1;

        var services = new ServiceCollection()
            .BuildServiceProvider();

        var dockerConfiguration = new DockerConfiguration(services);
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