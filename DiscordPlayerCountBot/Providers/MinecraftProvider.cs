namespace PlayerCountBot.Providers
{
    [Name("Minecraft")]
    public class MinecraftProvider : ServerInformationProvider<MinecraftService, MinecraftServer>
    {
        public MinecraftProvider(BotInformation info) : base(info)
        {
        }
    }
}
