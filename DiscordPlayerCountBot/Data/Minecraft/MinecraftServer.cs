using Newtonsoft.Json;

namespace DiscordPlayerCountBot.Data.Minecraft
{ 
    public class MinecraftServer
    {
        [JsonProperty("ip")]
        public string Ip { get; set; }

        [JsonProperty("port")]
        public int Port { get; set; }

        [JsonProperty("debug")]
        public MinecraftDebugInfo Debug { get; set; }

        [JsonProperty("motd")]
        public MinecraftMotd Motd { get; set; }

        [JsonProperty("players")]
        public MinecraftPlayerInfo Players { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("online")]
        public bool Online { get; set; }

        [JsonProperty("protocol")]
        public int Protocol { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("software")]
        public string Software { get; set; }

        [JsonProperty("info")]
        public MinecraftInfo Info { get; set; }
    }


}
