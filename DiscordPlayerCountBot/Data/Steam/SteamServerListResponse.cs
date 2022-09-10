using Newtonsoft.Json;

namespace PlayerCountBot
{
    public class SteamServerListResponse
    {
        [JsonProperty]
        public SteamServerListSubClass response { get; }

        public SteamServerListResponse()
        {
            response = new SteamServerListSubClass();
        }

        public SteamApiResponseData? GetServerDataByPort(int port)
        {
            return response.GetAddressDataByPort(port);
        }
    }
}
