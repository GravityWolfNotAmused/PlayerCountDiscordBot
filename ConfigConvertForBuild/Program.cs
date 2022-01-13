using Newtonsoft.Json;
using PlayerCountBot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConfigConvertForBuild
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

        [JsonProperty]
        public int _activityStatus { get; set; }

        public BotConfig()
        {
            _serverInformation = new List<DayZServerBot>();
        }
    }

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
    }

    class Program
    {
        static void Main(string[] args)
        {
            string fileContents = File.ReadAllText("./Config.json");
            var oldConfig = JsonConvert.DeserializeObject<BotConfig>(fileContents);
            var serverList = oldConfig._serverInformation.Select(info => new BotInformation()
            {
                Name = info.botName,
                Address = info.botAddress,
                Status = oldConfig._activityStatus,
                Token = info.discordBotToken,
                UseNameAsLabel = oldConfig._userConfigNameAsLabel
            }).ToList();

            PlayerCountBot.BotConfig NewConfig = new PlayerCountBot.BotConfig(false)
            {
                ServerInformation = serverList,
                SteamAPIKey = oldConfig._steamAPIKey,
                UpdateTime = oldConfig._updateTime
            };

            try
            {
                File.Delete("./Config.json");
            }
            catch (Exception e)
            {
                if (e is UnauthorizedAccessException || e is IOException || e is DirectoryNotFoundException)
                {
                    Console.WriteLine("Failed to delete old config file.");
                    Console.WriteLine(e);
                }
            }


            try
            {
                using StreamWriter file = File.CreateText(@"./Config.json");
                JsonSerializer serializer = new JsonSerializer()
                {
                    Formatting = Formatting.Indented,
                };

                serializer.Serialize(file, NewConfig);
            }
            catch (Exception e)
            {
                if (e is UnauthorizedAccessException || e is IOException || e is DirectoryNotFoundException)
                {
                    Console.WriteLine("Failed to delete old config file.");
                    Console.WriteLine(e);
                }
            }
        }
    }
}
