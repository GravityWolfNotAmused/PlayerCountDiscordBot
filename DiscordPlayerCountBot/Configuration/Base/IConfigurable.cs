namespace PlayerCountBot.Configuration.Base
{
    public interface IConfigurable
    {
        public HostEnvironment GetRequiredEnvironment();
        Task<Tuple<Dictionary<string, Bot>, int>> Configure(bool shouldStart = true);
    }
}