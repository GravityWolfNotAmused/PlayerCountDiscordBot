namespace PlayerCountBot.Configuration.Base
{
    public interface IConfigurable
    {
        Task<Tuple<Dictionary<string, Bot>, int>> Configure(bool shouldStart = true);
    }
}
