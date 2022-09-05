using DiscordPlayerCountBot.Attributes;

namespace DiscordPlayerCountBot.Http
{
    public class SteamGetServerListQueryParams : QueryParameterBuilder
    {
        [Name("key")]
        public string Key { get; set; }

        [Name("filter")]
        public string Filter { get; set; }
    }
}
