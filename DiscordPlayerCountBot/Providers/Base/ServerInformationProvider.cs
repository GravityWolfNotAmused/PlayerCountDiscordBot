using SteamServerQuery;
using System.Net;
using System.Net.Http;

namespace PlayerCountBot.Providers.Base
{
    public abstract class ServerInformationProvider : LoggableClass, IServerInformationProvider
    {
        public bool WasLastExecutionAFailure { get; set; } = false;
        public Exception? LastException { get; set; }

        public abstract Task<BaseViewModel?> GetServerInformation(BotInformation information, Dictionary<string, string> applicationVariables);

        protected void HandleLastException(BotInformation information)
        {
            if (WasLastExecutionAFailure)
            {
                Info($"Bot named: {information.Name} at address: {information.Address} successfully fetched data after failure.");
                LastException = null;
                WasLastExecutionAFailure = false;
            }
        }

        protected void HandleException(Exception e, string? id = null)
        {
            if (e.Message == LastException?.Message)
                return;

            WasLastExecutionAFailure = true;
            LastException = e;
            var Label = AttributeHelper.GetNameFromAttribute(this);

            if (e is TaskCanceledException canceledException)
            {
                Error($"Update task was canceled likely because of system timeout.", id);
                return;
            }

            if (e is KeyNotFoundException keyNotFoundException)
            {
                Error($"An application variable is missing from configuration.", id);
                return;
            }

            if (e is HttpRequestException requestException)
            {
                Error($"The {Label} has failed to respond. {requestException.StatusCode}", id);
                return;
            }

            if (e is WebException webException)
            {
                if (webException.Status == WebExceptionStatus.Timeout)
                {
                    Error($"Speaking with {Label} has timed out.", id);
                    return;
                }
                else if (webException.Status == WebExceptionStatus.ConnectFailure)
                {
                    Error($"Could not connect to {Label}.", id);
                    return;
                }
                else
                {
                    Error($"There was an error speaking with your {Label} server.", id, e);
                    return;
                }
            }

            if (e is ApplicationException applicationException)
            {
                Error($"{applicationException.Message}", id);
                return;
            }

            if (e is SteamException steamException)
            {
                Error($"There was an issue speaking with Steam Query Server.", e);
                return;
            }

            Error($"There was an error speaking with {Label}.", e);
            throw e;
        }

        public abstract DataProvider GetRequiredProviderType();
    }
}
