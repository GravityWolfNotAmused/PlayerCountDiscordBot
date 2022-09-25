namespace PlayerCountBot.Data
{
    public class ScumProviderResponse
    {
        public bool Success { get; set; }
        public string Error { get; set; }
        public int Servers { get; set; }
        public List<ScumServerData> Data { get; set; } = new();

        public ScumServerData? GetScumServerData(int port)
        {
            return Data.Where(data => data.QueryPort == port || data.Port == port).FirstOrDefault();
        }
    }
}
