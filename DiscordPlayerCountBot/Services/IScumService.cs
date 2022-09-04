using System.Threading.Tasks;
using DiscordPlayerCountBot.Data.Scum;

namespace DiscordPlayerCountBot.Services
{
    public interface IScumService
    {
        public Task<ScumProviderResponse?> GetPlayerInformationAsync(string address, int port);
    }
}
