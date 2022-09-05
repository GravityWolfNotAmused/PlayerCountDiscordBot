using Newtonsoft.Json;

namespace DiscordPlayerCountBot.Json
{
    public static class JsonHelper
    {
        public static T? DeserializeObject<T>(string content)
        {
            return JsonConvert.DeserializeObject<T>(content, new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore
            });
        }
    }
}
