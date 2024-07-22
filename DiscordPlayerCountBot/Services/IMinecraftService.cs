namespace PlayerCountBot.Services
{
    public interface IMinecraftService
    {
        public Task<MinecraftServer?> GetMinecraftServerInformationAsync(string address, int port);
    }
}