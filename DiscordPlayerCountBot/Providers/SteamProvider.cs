namespace PlayerCountBot.Providers
{
    [Name("Steam")]
    public class SteamProvider : ServerInformationProvider<SteamService, SteamApiResponseData>
    {
        public SteamProvider(BotInformation info) : base(info)
        {
        }
    }
}
