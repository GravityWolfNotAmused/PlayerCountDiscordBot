using Newtonsoft.Json;

namespace PlayerCountBot
{
    class SteamApiResponseData
    {
        [JsonProperty]
        public string addr { get; set; }

        [JsonProperty]
        public int gameport { get; set; }

        [JsonProperty]
        public string steamid { get; set; }

        [JsonProperty]
        public string name { get; set; }

        [JsonProperty]
        public int appid { get; set; }

        [JsonProperty]
        public string gamedir { get; set; }

        [JsonProperty]
        public string version { get; set; }

        [JsonProperty]
        public string product { get; set; }

        [JsonProperty]
        public int region { get; set; }

        [JsonProperty]
        public int players { get; set; }

        [JsonProperty]
        public int max_players { get; set; }

        [JsonProperty]
        public int bot { get; set; }

        [JsonProperty]
        public string map { get; set; }

        [JsonProperty]
        public bool secure { get; set; }

        [JsonProperty]
        public bool dedicated { get; set; }

        [JsonProperty]
        public string os { get; set; }

        [JsonProperty]
        public string gametype { get; set; }


        public string GetQueueCount()
        {
            string[] splitData = gametype.Split(",");

            if (splitData.Length > 0)
            {
                foreach (string str in splitData)
                {
                    if (str.Contains("lqs"))
                    {
                        string queueCount = str.Replace("lqs", "");

                        return queueCount;
                    }
                }
            }

            return "";
        }
    }
}
