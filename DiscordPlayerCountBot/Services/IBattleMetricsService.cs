namespace PlayerCountBot.Services
{
    public interface IBattleMetricsService
    {
        public Task<BattleMetricsServerData?> GetPlayerInformationAsync(string address, string token);
    }
}