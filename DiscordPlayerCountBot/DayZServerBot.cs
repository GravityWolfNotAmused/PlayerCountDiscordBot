using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

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

        public async Task<string> GetHostAddress()
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
