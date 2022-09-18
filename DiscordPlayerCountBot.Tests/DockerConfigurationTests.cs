namespace DiscordPlayerCountBot.Tests;

[Collection("Configuration Test Suite")]
public class DockerConfigurationTests
{

    [Fact(DisplayName = "Test Docker Configuration with all data", Timeout = 30)]
    public async Task DockerConfigurationTestWithAllData()
    {
        EnivronmentHelper.SetTestEnvironmentWithAllVariables();

        var bots = new Dictionary<string, Bot>();
        var time = -1;

        var dockerConfiguration = new DockerConfiguration();
        var configuration = await dockerConfiguration.Configure(false);

        bots = configuration.Item1;
        time = configuration.Item2;

        EnivronmentHelper.ClearTestEnvironmentVariables();

        Assert.True(time > 0, $"Time was returned from config and not zero. Actual: {time}");
        Assert.True(bots.Count > 0, $"More than one bot was created. Actual: {bots.Count}");
        Assert.True(bots.ToList()[0].Value.ApplicationTokens.Count >= 2, $"More than 2 tokens in the dictionary for the bot. Actual: {bots.ToList()[0].Value.ApplicationTokens.Count}");
        Assert.True(bots.ToList()[0].Value.ApplicationTokens.ContainsKey("SteamAPIKey"), $"Steam Key should be present.");
        Assert.True(bots.ToList()[0].Value.ApplicationTokens.ContainsKey("BattleMetricsKey"), $"Battle Metrics Key should be present.");
    }

    [Fact(DisplayName = "Test Docker Configuration without Battle Metrics", Timeout = 30)]
    public async Task DockerConfigurationTestWithoutBattleMetrics()
    {
        EnivronmentHelper.SetTestEnvironmentWithoutBattleMetrics();

        var bots = new Dictionary<string, Bot>();
        var time = -1;

        var dockerConfiguration = new DockerConfiguration();
        var configuration = await dockerConfiguration.Configure(false);

        bots = configuration.Item1;
        time = configuration.Item2;

        EnivronmentHelper.ClearTestEnvironmentVariables();

        Assert.True(time > 0, $"Time was returned from config and not zero. Actual: {time}");
        Assert.True(bots.Count > 0, $"More than one bot was created. Actual: {bots.Count}");
        Assert.True(bots.ToList()[0].Value.ApplicationTokens.Count == 1, $"1 token in the dictionary for the bots. Actual: {bots.ToList()[0].Value.ApplicationTokens.Count}");
        Assert.True(bots.ToList()[0].Value.ApplicationTokens.ContainsKey("SteamAPIKey"), $"Steam Key should be present.");
        Assert.False(bots.ToList()[0].Value.ApplicationTokens.ContainsKey("BattleMetricsKey"), $"Battle Metrics Key should not be present.");
    }

    [Fact(DisplayName = "Test Docker Configuration without Application Variables", Timeout = 30)]
    public async Task DockerConfigurationTestWithoutApplicationVariables()
    {
        EnivronmentHelper.SetTestEnvironmentWithoutApplicationVariables();

        var bots = new Dictionary<string, Bot>();
        var time = -1;

        var dockerConfiguration = new DockerConfiguration();
        var configuration = await dockerConfiguration.Configure(false);

        bots = configuration.Item1;
        time = configuration.Item2;

        EnivronmentHelper.ClearTestEnvironmentVariables();

        Assert.True(time > 0, $"Time was returned from config and not zero. Actual: {time}");
        Assert.True(bots.Count > 0, $"More than one bot was created. Actual: {bots.Count}");
        Assert.True(bots.ToList()[0].Value.ApplicationTokens.Count == 0, $"Should be 0 variables in the dictionary. Actual: {bots.ToList()[0].Value.ApplicationTokens.Count}");
    }

    [Fact(DisplayName = "Test Docker Configuration without Steam variable", Timeout = 30)]
    public async Task DockerConfigurationTestWithoutSteam()
    {
        EnivronmentHelper.SetTestEnvironmentWithoutSteam();

        var bots = new Dictionary<string, Bot>();
        var time = -1;

        var dockerConfiguration = new DockerConfiguration();
        var configuration = await dockerConfiguration.Configure(false);

        bots = configuration.Item1;
        time = configuration.Item2;

        EnivronmentHelper.ClearTestEnvironmentVariables();

        Assert.True(time > 0, $"Time was returned from config and not zero. Actual: {time}");
        Assert.True(bots.Count > 0, $"More than one bot was created. Actual: {bots.Count}");
        Assert.True(bots.ToList()[0].Value.ApplicationTokens.Count == 1, $"1 token in the dictionary for the bots. Actual: {bots.ToList()[0].Value.ApplicationTokens.Count}");
        Assert.True(bots.ToList()[0].Value.ApplicationTokens.ContainsKey("BattleMetricsKey"), $"Battle Metrics Key should be present.");
    }
}