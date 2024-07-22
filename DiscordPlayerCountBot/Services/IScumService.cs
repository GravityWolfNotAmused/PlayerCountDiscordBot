namespace PlayerCountBot.Services
{
    public interface IScumService
    {
        public Task<ScumProviderResponse?> GetPlayerInformationAsync(string address, int port);
    }
}