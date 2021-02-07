using Newtonsoft.Json;

namespace PlayerCountBot
{
    class DayZServerBot
    {
        [JsonProperty]
        public string botName { get; set; }

        [JsonProperty]
        public string botAddress { get; set; }

        [JsonProperty]
        public string discordBotToken { get; set; }

        public DayZServerBot(string name, string address, string discordKey)
        {
            botName = name;
            botAddress = address;
            discordBotToken = discordKey;
        }
    }
}
