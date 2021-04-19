using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

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
        public async Task<List<string>> GetAddresses()
        {
            List<string> addresses = new List<string>();

            foreach (DayZServerBot bot in _serverInformation)
            {
                string ipAddress = bot.botAddress.Split(":")[0];

                if (ipAddress.ToLower() == "hostname")
                {
                    ipAddress = await GetPublicIpAddress();

                    if (ipAddress == string.Empty)
                    {
                        Console.WriteLine("IP Address could not be resolved. Please contact Gravity Wolf on discord. GravityWolf#6981");
                    }
                }

                if (!addresses.Contains(ipAddress))
                {
                    addresses.Add(ipAddress);
                }
            }
            return addresses;
        }
        private async Task<string> GetPublicIpAddress()
        {
            string publicIPAddress = string.Empty;

            try
            {
                var request = (HttpWebRequest)WebRequest.Create("http://ifconfig.me");

                request.UserAgent = "curl"; // this will tell the server to return the information as if the request was made by the linux "curl" command

                request.Method = "GET";

                using (WebResponse response = await request.GetResponseAsync())
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        publicIPAddress = await reader.ReadToEndAsync();
                    }
                }
            }
            catch (WebException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error Reaching ifconfig.me");
                Console.ForegroundColor = ConsoleColor.White;
                return publicIPAddress;
            }
            
            return publicIPAddress.Replace("\n", "");
        }
    }
}
