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

        public SteamApiResponseData GetAddressDataByPort(string port)
        {
            foreach (SteamApiResponseData data in servers)
            {
                if (data.addr.Split(":")[1] == port)
                {
                    return data;
                }
            }

            return null;
        }
    }
}
