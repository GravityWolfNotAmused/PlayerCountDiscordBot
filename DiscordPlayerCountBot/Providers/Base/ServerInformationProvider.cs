using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using log4net;
using PlayerCountBot;
using DiscordPlayerCountBot.Attributes;

namespace DiscordPlayerCountBot.Providers.Base
{
    public abstract class ServerInformationProvider : IServerInformationProvider
    {
        public bool WasLastExecutionAFailure { get; set; } = false;
        public Exception? LastException { get; set; }
        protected ILog Logger { get; set; }
        public readonly string Label;

        public ServerInformationProvider()
        {
            Logger = LogManager.GetLogger(GetType());
            Label = AttributeHelper.GetNameFromAttribute(this);
        }

        public abstract Task<GenericServerInformation?> GetServerInformation(BotInformation information, Dictionary<string, string> applicationVariables);
        protected void HandleLastException(BotInformation information)
        {
            if (WasLastExecutionAFailure)
            {

                Logger.Info($"[{Label}] - Bot named: {information.Name} at address: {information.Address} successfully fetched data after failure.");
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

            if (e is TaskCanceledException canceledException)
            {
                Logger.Error($"[{Label}] - Update task was canceled likely because of system timeout.");
                return;
            }

            if (e is KeyNotFoundException keyNotFoundException)
            {
                Logger.Error($"[{Label}] - SteamAPIKey is missing from Application variable configuration.");
                return;
            }

            if (e is HttpRequestException requestException)
            {
                Logger.Error($"[{Label}] - The {Label} has failed to respond. {requestException.StatusCode}");
                return;
            }

            if (e is WebException webException)
            {
                if (webException.Status == WebExceptionStatus.Timeout)
                {
                    Logger.Error($"[{Label}] - Speaking with {Label} has timed out.");
                    return;
                }
                else if (webException.Status == WebExceptionStatus.ConnectFailure)
                {
                    Logger.Error($"[{Label}] - Could not connect to {Label}.");
                    return;
                }
                else
                {
                    Logger.Error($"[{Label}] - There was an error speaking with your {Label} server.", e);
                    return;
                }
            }

            if (e is ApplicationException applicationException)
            {
                Logger.Error($"[{Label}] - {applicationException.Message}");
                return;
            }

            Logger.Error($"[{Label}] - There was an error speaking with {Label}.", e);
            throw e;
        }
    }
}
