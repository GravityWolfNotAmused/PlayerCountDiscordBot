using System.Collections.Generic;
using System.Linq;

namespace DiscordPlayerCountBot.Data.Scum
{
    public class ScumProviderResponse
    {
        public bool Success { get; set; }
        public string Error { get; set; }
        public int Servers { get; set; }
        public List<ScumServerData> Data { get; set; } = new();

        public ScumServerData? GetScumServerDataByQueryPort(int queryPort)
        {
            return Data.Where(data => data.QueryPort == queryPort).FirstOrDefault();
        }
    }
}
