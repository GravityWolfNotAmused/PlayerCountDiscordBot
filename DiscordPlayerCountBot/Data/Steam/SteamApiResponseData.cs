namespace PlayerCountBot
{
    public class SteamApiResponseData
    {
        public string addr { get; set; }
        public int gameport { get; set; }
        public string steamid { get; set; }
        public string name { get; set; }
        public int appid { get; set; }
        public string gamedir { get; set; }
        public string? version { get; set; }
        public string product { get; set; }
        public int region { get; set; }
        public int players { get; set; }
        public int max_players { get; set; }
        public int bot { get; set; }
        public string map { get; set; }
        public bool secure { get; set; }
        public bool dedicated { get; set; }
        public string os { get; set; }
        public string gametype { get; set; }

        public int GetQueueCount()
        {
            if (gametype != null)
            {
                string[] splitData = gametype.Split(",");

                if (splitData.Length > 0)
                {
                    foreach (string str in splitData)
                    {
                        if (str.Contains("lqs"))
                        {
                            string queueCount = str.Replace("lqs", "");

                            return int.Parse(queueCount);
                        }
                    }
                }
            }
            return 0;
        }
    }
}
