namespace PlayerCountBot.Data
{
    public class CFXServer : IViewModelConverter
    {
        [JsonProperty("clients")]
        public int PlayerCount { get; set; }
        [JsonProperty("gametype")]
        public string GameType { get; set; }
        [JsonProperty("hostname")]
        public string HostName { get; set; }
        [JsonProperty("iv")]
        public string IV { get; set; }
        [JsonProperty("mapname")]
        public string MapName { get; set; }
        [JsonProperty("sv_maxclients")]
        public string MaxClients { get; set; }

        public BaseViewModel? ToViewModel()
        {
            return new CFXViewModel
            {
                Address = HostName,
                Players = PlayerCount,
                MaxPlayers = int.Parse(MaxClients),
                QueuedPlayers = 0
            };
        }
    }
}
