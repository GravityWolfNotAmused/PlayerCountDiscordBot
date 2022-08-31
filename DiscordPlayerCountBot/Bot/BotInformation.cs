using log4net.Repository.Hierarchy;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System;

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
        public int ProviderType { get; set; } = 0;

        [JsonProperty]
        public ulong? ChannelID { get; set; }

        [JsonIgnore]
        public string SteamAPIToken { get; set; }

        public Tuple<string, ushort> GetAddressAndPort()
        {
            string[] splitData = Address.Split(":");
            try
            {
                ushort port = ushort.Parse(splitData[1]);
                return new Tuple<string, ushort>(splitData[0], port);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
