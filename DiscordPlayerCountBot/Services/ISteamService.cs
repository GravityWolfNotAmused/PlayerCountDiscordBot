namespace PlayerCountBot.Services
{
    public interface ISteamService
    {
        public Task<SteamApiResponseData?> GetSteamApiResponse(string address, int port, string token);
    }
}
