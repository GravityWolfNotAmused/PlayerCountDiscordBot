using Newtonsoft.Json;

namespace DiscordPlayerCountBot.Data.Minecraft
{
    public class MinecraftErrorInfo
    {
        [JsonProperty("query")]
        public string Query { get; set; }
    }


}
