using Newtonsoft.Json;

namespace DiscordPlayerCountBot.Data.Minecraft
{
    public class MinecraftPlayerInfo
    {
        [JsonProperty("online")]
        public int Online { get; set; }

        [JsonProperty("max")]
        public int Max { get; set; }
    }


}
