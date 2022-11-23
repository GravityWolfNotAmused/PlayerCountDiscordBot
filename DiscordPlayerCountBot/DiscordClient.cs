using System.Net.Sockets;
using System.Threading.Channels;

namespace PlayerCountBot
{
    public static class DiscordClient
    {
        public static async Task LoginAndStartAsync(this DiscordSocketClient client, string token, string address, bool shouldStart = true)
        {
            if (shouldStart)
            {
                await client.LoginAsync(TokenType.Bot, token);
                await client.SetGameAsync($"Starting: {address}");
                await client.StartAsync();
            }
        }

        public static async Task ChannelNameWarning(this IDiscordClient socket, ulong? channelId)
        {
            if (channelId == null) return;

            IGuildChannel channel = (IGuildChannel)await socket.GetChannelAsync(channelId.Value);

            if (channel is null)
            {
                throw new ArgumentException($"[Bot] - Invalid Channel Id: {channelId}, Channel was not found.");
            }

            if (channel != null)
            {
                if (channel is IVoiceChannel)
                {
                    return;
                }
                else
                {
                    var chnl = channel as IMessageChannel;
                    await chnl.SendMessageAsync("WARNING: Some characters you chose in the channel name are not compatible with a text channel name so have been replaced!");
                }
            }
        }

        public static async Task SetChannelName(this IDiscordClient socket, ulong? channelId, string gameStatus)
        {
            if (channelId == null) return;

            IGuildChannel channel = (IGuildChannel)await socket.GetChannelAsync(channelId.Value);

            if (channel is null)
            {
                throw new ArgumentException($"[Bot] - Invalid Channel Id: {channelId}, Channel was not found.");
            }

            if (channel != null)
            {
                if (channel is IVoiceChannel)
                {
                    gameStatus = gameStatus;
                }
                else
                {
                    gameStatus = gameStatus.Replace('/', '-').Replace(' ', '-').Replace(':', '-');
                }


                //Keep in mind there is a massive rate limit on this call that is specific to discord, and not Discord.Net
                //2x per 10 minutes
                //https://discord.com/developers/docs/topics/rate-limits
                //https://www.reddit.com/r/Discord_Bots/comments/qzrl5h/channel_name_edit_rate_limit/
                await channel.ModifyAsync(prop => prop.Name = gameStatus);
            }
        }
    }
}
