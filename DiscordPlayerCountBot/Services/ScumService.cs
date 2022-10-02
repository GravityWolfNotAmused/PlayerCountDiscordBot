namespace PlayerCountBot.Services
{
    [Obsolete("Found to be worse than Battle Metrics", true)]
    public class ScumService : IProviderService<ScumProviderResponse>
    {
        public async Task<ScumProviderResponse?> GetInformation(string search, string? token = null)
        {
            using var httpClient = new HttpExecuter();
            var response = await httpClient.GET<object, ScumProviderResponse>($"https://api.hellbz.de/scum/api.php", new ScumGetServerInformationQueryParams()
            {
                Address = search
            });

            return response;
        }
    }
}
