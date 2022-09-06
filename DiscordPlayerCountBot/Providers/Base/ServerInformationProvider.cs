using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using log4net;
using PlayerCountBot;

namespace DiscordPlayerCountBot.Providers.Base
{
    public abstract class ServerInformationProvider : IServerInformationProvider
    {
        public bool WasLastExecutionAFailure { get; set; } = false;
        public Exception? LastException { get; set; }
        protected ILog Logger { get; set; }

        public ServerInformationProvider()
        {
            Logger = LogManager.GetLogger(GetType());
        }

        public abstract Task<GenericServerInformation?> GetServerInformation(BotInformation information, Dictionary<string, string> applicationVariables);
        protected void HandleLastException(BotInformation information)
        {
            if (WasLastExecutionAFailure)
            {
                Logger.Info($"[{GetType().Name}] - Bot for Address: {information.Address} successfully fetched data after failure.");
                LastException = null;
                WasLastExecutionAFailure = false;
            }
        }
    }
}
