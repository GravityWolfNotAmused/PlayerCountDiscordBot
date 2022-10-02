using System.Net;

namespace PlayerCountBot.Services
{
    public class CFXService : IProviderService<CFXServer>
    {
        public async Task<CFXServer?> GetInformation(string search, string? token = null)
        {
            using var httpClient = new HttpExecuter();
            return await httpClient.GET<object, CFXServer>($"http://{search}/Dynamic.json");
        }
    }
}
