namespace DiscordPlayerCountBot.ViewModels
{
    public class BattleMetricsViewModel : BaseViewModel
    {
        public string Map { get; set; }
        public string Time { get; set; }
        public string GameMode { get; set; }
        public int Rank { get; set; }
    }
}