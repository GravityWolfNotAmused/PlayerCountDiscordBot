namespace PlayerCountBot.Services
{
    [Obsolete("Found to be worse than Battle Metrics", true)]
    public class ScumService : IScumService
    {
        public async Task<ScumProviderResponse?> GetPlayerInformationAsync(string address, int port)
        {
            using var httpClient = new HttpExecuter();
            var response = await httpClient.GET<object, ScumProviderResponse>($"https://api.hellbz.de/scum/api.php", new ScumGetServerInformationQueryParams()
            {
                Address = address
            });

            return response;
        }
    }
}
