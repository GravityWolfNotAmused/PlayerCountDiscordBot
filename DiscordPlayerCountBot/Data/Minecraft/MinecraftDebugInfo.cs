namespace PlayerCountBot.Data
{
    public class MinecraftDebugInfo
    {
        [JsonProperty("ping")]
        public bool Ping { get; set; }

        [JsonProperty("query")]
        public bool Query { get; set; }

        [JsonProperty("srv")]
        public bool Srv { get; set; }

        [JsonProperty("querymismatch")]
        public bool Querymismatch { get; set; }

        [JsonProperty("ipinsrv")]
        public bool Ipinsrv { get; set; }

        [JsonProperty("cnameinsrv")]
        public bool Cnameinsrv { get; set; }

        [JsonProperty("animatedmotd")]
        public bool Animatedmotd { get; set; }

        [JsonProperty("cachetime")]
        public int Cachetime { get; set; }

        [JsonProperty("apiversion")]
        public int Apiversion { get; set; }

        [JsonProperty("error")]
        public MinecraftErrorInfo Error { get; set; }
    }


}