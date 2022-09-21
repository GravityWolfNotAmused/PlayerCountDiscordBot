using Newtonsoft.Json;
using System.Collections.Generic;

namespace PlayerCountBot
{
    public class SteamServerListSubClass
    {
        [JsonProperty]
        public List<SteamApiResponseData> servers { get; }

        public SteamServerListSubClass()
        {
            servers = new List<SteamApiResponseData>();
        }

        public SteamApiResponseData? GetAddressDataByPort(int port)
        {
            foreach (SteamApiResponseData data in servers)
            {
                if (int.Parse(data.addr.Split(":")[1]) == port || data.gameport == port)
                {
                    return data;
                }
            }

            return null;
        }
    }
}
