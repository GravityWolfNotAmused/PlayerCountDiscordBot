using DiscordPlayerCountBot.Data.Minecraft;

namespace DiscordPlayerCountBot.Services
{
    public interface IMinecraftService
    {
        public Task<MinecraftServer?> GetMinecraftServerInformationAsync(string address, int port);
    }
}