using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace DiscordPlayerCountBot
{
    public static class DiscordClient
    {
        public static async Task LoginAndStartAsync(this DiscordSocketClient client, string token, string address)
        {
            await client.LoginAsync(TokenType.Bot, token);
            await client.SetGameAsync($"Starting: {address}");
            await client.StartAsync();
        }
    }
}
