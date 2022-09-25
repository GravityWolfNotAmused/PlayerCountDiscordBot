namespace PlayerCountBot.Providers.Base
{
    public interface IServerInformationProvider
    {
        bool WasLastExecutionAFailure { get; set; }
        Exception? LastException { get; set; }
        Task<BaseViewModel?> GetServerInformation(BotInformation information, Dictionary<string, string> applicationVariables);
    }
}
