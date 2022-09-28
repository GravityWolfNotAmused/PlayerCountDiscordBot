namespace PlayerCountBot.Http
{
    public class ScumGetServerInformationQueryParams : QueryParameterBuilder
    {
        [Name("address")]
        public string Address { get; set; }

        [Name("port")]
        public int? Port { get; set; } = null;
    }
}
