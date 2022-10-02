namespace PlayerCountBot.Providers
{
    [Name("CFX")]
    public class CFXProvider : ServerInformationProvider<CFXService, CFXServer>
    {
        public CFXProvider(BotInformation info) : base(info)
        {
        }
    }
}
