using Newtonsoft.Json;
using System;

namespace DiscordPlayerCountBot.Json
{
    public static class JsonHelper
    {
        public static T? DeserializeObject<T>(string content)
        {
            if (typeof(T).IsPrimitive || typeof(T) == typeof(string)) return (T)Convert.ChangeType(content, typeof(T));

            return JsonConvert.DeserializeObject<T>(content, new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore
            });
        }
    }
}
