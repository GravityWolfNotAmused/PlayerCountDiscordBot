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

        public BotInformation()
        {

        }

        public BotInformation(string name, string address, string token, int status = 1, bool useNameAsLabel = false)
        {
            Name = name;
            Address = address;
            Token = token;
            Status = status;
            UseNameAsLabel = UseNameAsLabel;
        }
    }
}
