using System;
using System.Threading.Tasks;
using PlayerCountBot;

namespace DiscordPlayerCountBot.Providers.Base
{
    public abstract class ServerInformationProvider : IServerInformationProvider
    {
        public bool WasLastExecutionAFailure { get; set; } = false;
        public Exception? LastException { get; set; }

        public abstract Task<GenericServerInformation?> GetServerInformation(BotInformation information);
    }
}
