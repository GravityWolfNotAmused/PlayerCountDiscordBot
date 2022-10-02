namespace PlayerCountBot.Providers
{
    [Obsolete("Found to be worse than Battle Metrics", true)]
    [Name("Scum")]
    public class ScumProvider : ServerInformationProvider<ScumService, ScumProviderResponse>
    {
        public ScumProvider(BotInformation info) : base(info)
        {
        }
    }
}
