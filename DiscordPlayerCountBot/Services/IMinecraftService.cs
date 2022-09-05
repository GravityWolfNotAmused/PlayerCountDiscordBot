using DiscordPlayerCountBot.Data.Minecraft;
using System.Threading.Tasks;

namespace DiscordPlayerCountBot.Services
{
    public interface IMinecraftService
    {
        public Task<MinecraftServer?> GetMinecraftServerInformationAsync(string address, int port);
    }
}
