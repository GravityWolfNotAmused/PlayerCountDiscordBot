using Microsoft.VisualBasic;
using System.Net;

namespace PlayerCountBot.Providers.Base
{
    public abstract class ServerInformationProvider<T, T2> : LoggableClass, IServerInformationProvider where T : IProviderService<T2> where T2 : IViewModelConverter
    {
        public bool WasLastExecutionAFailure { get; set; } = false;
        public Exception? LastException { get; set; }
        public readonly Dictionary<int, string> Keys = new()
        {
            {(int)DataProvider.CFX, "" },
            {(int)DataProvider.SCUM, "" },
            {(int)DataProvider.MINECRAFT, "" },
            {(int)DataProvider.STEAM, "SteamAPIKey" },
            {(int)DataProvider.BATTLEMETRICS, "BattleMetricsKey"}
        };

        public T Service { get; set; }

        public ServerInformationProvider(BotInformation info) : base(info)
        {
            Service = Activator.CreateInstance<T>();
        }

        public string? GetToken(Dictionary<string, string> applicationVariables, int providerType)
        {
            var token = "";
            var key = Keys[providerType];

            if (key == "") return null;

            applicationVariables.TryGetValue(key, out token);

            if (string.IsNullOrEmpty(token) || string.IsNullOrWhiteSpace(token)) throw new ApplicationException($"Missing application variable: {key}");

            return token;
        }

        public async Task<BaseViewModel?> GetServerInformation(Dictionary<string, string> applicationVariables)
        {
            try
            {
                var addressAndPort = Information!.GetAddressAndPort();
                var response = await Service?.GetInformation($"{addressAndPort.Item1}:{addressAndPort.Item2}", GetToken(applicationVariables, Information!.ProviderType))!;

                if (response == null)
                    throw new ApplicationException("Response cannot be null.");

                HandleLastException(Information!);

                return response.ToViewModel();
            }
            catch (Exception e)
            {
                HandleException(e);
                return null;
            }
        }

        protected void HandleLastException(BotInformation information)
        {
            if (WasLastExecutionAFailure)
            {

                Info($"Bot named: {information.Name} at address: {information.Address} successfully fetched data after failure.");
                LastException = null;
                WasLastExecutionAFailure = false;
            }
        }


        protected void HandleException(Exception e)
        {
            if (e.Message == LastException?.Message)
                return;

            WasLastExecutionAFailure = true;
            LastException = e;
            var Label = AttributeHelper.GetNameFromAttribute(this);

            if (e is TaskCanceledException canceledException)
            {
                Error($"Update task was canceled likely because of system timeout.");
                return;
            }

            if (e is KeyNotFoundException keyNotFoundException)
            {
                Error($"An application variable is missing from configuration.");
                return;
            }

            if (e is HttpRequestException requestException)
            {
                Error($"The {Label} has failed to respond. {requestException.StatusCode}");
                return;
            }

            if (e is WebException webException)
            {
                if (webException.Status == WebExceptionStatus.Timeout)
                {
                    Error($"Speaking with {Label} has timed out.");
                    return;
                }
                else if (webException.Status == WebExceptionStatus.ConnectFailure)
                {
                    Error($"Could not connect to {Label}.");
                    return;
                }
                else
                {
                    Error($"There was an error speaking with your {Label} server.", e);
                    return;
                }
            }

            if (e is ApplicationException applicationException)
            {
                Error($"{applicationException.Message}");
                return;
            }

            Error($"There was an error speaking with {Label}.", e);
            throw e;
        }
    }
}
