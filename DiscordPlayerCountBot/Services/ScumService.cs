using DiscordPlayerCountBot.Data.Scum;
using DiscordPlayerCountBot.Http;
using System.Net.Http;
using System.Threading.Tasks;


namespace DiscordPlayerCountBot.Services
{
    public class ScumService : IScumService
    {
        public async Task<ScumProviderResponse?> GetPlayerInformationAsync(string address, int port)
        {
            using var httpClient = new HttpExecuter(new HttpClient());
            var response = await httpClient.GET<object, ScumProviderResponse>($"https://api.hellbz.de/scum/api.php", new ScumGetServerInformationQueryParams()
            {
                Address = address,
                Port = port
            });

            if(response?.Success ?? false)
            {
                return response;
            }

            return await httpClient.GET<object, ScumProviderResponse>($"https://api.hellbz.de/scum/api.php", new ScumGetServerInformationQueryParams()
            {
                Address = address
            });
        }
    }
}
