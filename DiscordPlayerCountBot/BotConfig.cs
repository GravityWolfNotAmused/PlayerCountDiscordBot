using Newtonsoft.Json;
using System.Collections.Generic;

namespace PlayerCountBot
{
    class BotConfig
    {
        [JsonProperty]
        public int _updateTime { get; set; }

        [JsonProperty]
        public string _steamAPIKey { get; set; }


        [JsonProperty]
        public bool _isDebug { get; set; }

        [JsonProperty]
        public List<DayZServerBot> _serverInformation;

        [JsonProperty]
        public bool _userConfigNameAsLabel { get; set; }

        public BotConfig()
        {
            _serverInformation = new List<DayZServerBot>();
        }

        public void CreateDefaults()
        {
            _serverInformation.Add(new DayZServerBot("VPPTestBot", "127.0.0.1:27014", "DiscordTokenHere"));
            _updateTime = 30;
            _steamAPIKey = "SteamAPIKeyHere";
            _userConfigNameAsLabel = false;
        }
        public List<string> GetAddresses()
        {
            List<string> addresses = new List<string>();

            foreach (DayZServerBot bot in _serverInformation)
            {
                string ipAddress = bot.botAddress.Split(":")[0];

                if (!addresses.Contains(ipAddress))
                {
                    addresses.Add(ipAddress);
                }
            }
            return addresses;
        }
    }
}
