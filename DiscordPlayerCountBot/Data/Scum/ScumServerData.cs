using Newtonsoft.Json;

namespace DiscordPlayerCountBot.Data.Scum
{
    public class ScumServerData
    {
        public string Address { get; set; }
        public int Port { get; set; }
        [JsonProperty("q_port")]
        public int QueryPort { get; set; }
        public string Name { get; set; }
        public int Players { get; set; }
        [JsonProperty("players_max")]
        public int MaxPlayers { get; set; }
        public string? Version { get; set; }
        public string? Time { get; set; }
        public bool Password { get; set; }
    }
}
