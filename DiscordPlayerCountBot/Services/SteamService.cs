namespace PlayerCountBot.Services
{

    public class SteamService : IProviderService<SteamApiResponseData>
    {
        public async Task<SteamApiResponseData?> GetInformation(string search, string? token = null)
        {
            using var httpClient = new HttpExecuter();
            var response = await httpClient.GET<object, SteamServerListResponse>("https://api.steampowered.com/IGameServersService/GetServerList/v1/", new SteamGetServerListQueryParams()
            {
                Key = token ?? throw new ApplicationException("Application is using Steam as a provider, but doesn\'t have a Steam API token."),
                Filter = $"\\addr\\{search}"
            });

            if (response == null) return null;

            var port = int.Parse(search.Split(":")[1]);
            var data = response.GetServerDataByPort(port);

            if (data == null) return null;

            return data;
        }
    }
}
