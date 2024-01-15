namespace PlayerCountBot.Services
{
    public interface ICFXService
    {
        public Task<CFXServer?> GetServerInformationAsync(string address);
        public Task<List<CFXPlayerInformation>?> GetPlayerInformationAsync(string address);
    }
}