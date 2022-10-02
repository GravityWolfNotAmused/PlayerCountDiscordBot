namespace PlayerCountBot.Services
{
    public class MinecraftService : IProviderService<MinecraftServer>
    {
        public async Task<MinecraftServer?> GetInformation(string search, string? token = null)
        {
            using var httpClient = new HttpExecuter();
            return await httpClient.GET<object, MinecraftServer>($"https://api.mcsrvstat.us/2/{search}");
        }
    }
}
