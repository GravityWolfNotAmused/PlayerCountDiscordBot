using DiscordPlayerCountBot.Data.Minecraft;
using DiscordPlayerCountBot.Http;
using System.Net.Http;
using System.Threading.Tasks;

namespace DiscordPlayerCountBot.Services
{
    public class MinecraftService : IMinecraftService
    {
        public async Task<MinecraftServer?> GetMinecraftServerInformationAsync(string address, int port)
        {
            using var httpClient = new HttpExecuter(new HttpClient());
            return await httpClient.GET<object, MinecraftServer>($"https://api.mcsrvstat.us/2/{address}:{port}");
        }
    }
}
