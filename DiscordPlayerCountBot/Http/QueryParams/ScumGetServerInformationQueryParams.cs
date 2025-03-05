using DiscordPlayerCountBot.Attributes;
using DiscordPlayerCountBot.Http.QueryParams.Base;

namespace DiscordPlayerCountBot.Http.QueryParams;

public class ScumGetServerInformationQueryParams : QueryParameterBuilder
{
    [Name("address")]
    public string Address { get; set; }

    [Name("port")]
    public int? Port { get; set; } = null;
}