using System.Collections.Generic;
using Newtonsoft.Json;

namespace PlayerCountBot
{
    public class BotConfig
    {
        [JsonProperty]
        public int UpdateTime { get; set; }

        [JsonProperty]
        public string SteamAPIKey { get; set; }

        [JsonProperty]
        public List<BotInformation> ServerInformation;

        [JsonIgnore]
        public bool IsDocker { get; set; }

        public BotConfig(bool isDocker)
        {
            ServerInformation = new List<BotInformation>();
            IsDocker = isDocker;
        }

        public void CreateDefaults()
        {

            ServerInformation.Add(new BotInformation("VPPTestBot", "127.0.0.1:27014", "DiscordTokenHere"));
            UpdateTime = 30;
            SteamAPIKey = "SteamAPIKeyHere";
        }
    }
}
