using Newtonsoft.Json;
using System.Collections.Generic;

namespace DiscordPlayerCountBot.Data.Minecraft
{ 
    public class MinecraftMotd
    {
        [JsonProperty("raw")]
        public List<string> Raw { get; set; }

        [JsonProperty("clean")]
        public List<string> Clean { get; set; }

        [JsonProperty("html")]
        public List<string> Html { get; set; }
    }


}
