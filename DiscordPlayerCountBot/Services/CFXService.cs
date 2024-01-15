namespace PlayerCountBot.Services
{
    public class CFXService : ICFXService
    {
        public async Task<List<CFXPlayerInformation>?> GetPlayerInformationAsync(string address)
        {
            using var httpClient = new HttpExecuter();
            return await httpClient.GET<object, List<CFXPlayerInformation>>($"http://{address}/Players.json");
        }

        public async Task<CFXServer?> GetServerInformationAsync(string address)
        {
            using var httpClient = new HttpExecuter();
            return await httpClient.GET<object, CFXServer>($"http://{address}/Info.json");
        }
    }
}