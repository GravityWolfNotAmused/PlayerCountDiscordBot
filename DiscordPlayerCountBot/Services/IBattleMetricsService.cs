using DiscordPlayerCountBot.Data.BattleMetrics;

namespace DiscordPlayerCountBot.Services;

public interface IBattleMetricsService
{
    public Task<BattleMetricsServerData?> GetPlayerInformationAsync(string address, string token);
}