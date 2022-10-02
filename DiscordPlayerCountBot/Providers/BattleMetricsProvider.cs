namespace PlayerCountBot.Providers
{
    [Name("BattleMetrics")]
    public class BattleMetricsProvider : ServerInformationProvider<BattleMetricsService, BattleMetricsServerData>
    {
        public BattleMetricsProvider(BotInformation info) : base(info)
        {
        }
    }
}
