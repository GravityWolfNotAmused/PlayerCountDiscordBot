using Newtonsoft.Json;

namespace PlayerCountBot
{
    public class BotInformation
    {
        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public string Address { get; set; }

        [JsonProperty]
        public string Token { get; set; }

        [JsonProperty]
        public int Status { get; set; }

        [JsonProperty]
        public bool UseNameAsLabel { get; set; }

        [JsonProperty]
        public ulong? ChannelID { get; set; }
    }
}
