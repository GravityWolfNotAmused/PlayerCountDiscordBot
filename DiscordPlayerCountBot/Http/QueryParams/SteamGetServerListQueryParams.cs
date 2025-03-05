using DiscordPlayerCountBot.Attributes;
using DiscordPlayerCountBot.Http.QueryParams.Base;

namespace DiscordPlayerCountBot.Http.QueryParams;

public class SteamGetServerListQueryParams : QueryParameterBuilder
{
    [Name("key")]
    public string Key { get; set; }

    [Name("filter")]
    public string Filter { get; set; }
}