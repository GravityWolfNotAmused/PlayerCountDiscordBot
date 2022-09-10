using DiscordPlayerCountBot.Data;
using System.Threading.Tasks;

namespace DiscordPlayerCountBot.Services
{
    public interface IBattleMetricsService
    {
        public Task<BattleMetricsServerData?> GetPlayerInformationAsync(string address, string token);
    }
}
