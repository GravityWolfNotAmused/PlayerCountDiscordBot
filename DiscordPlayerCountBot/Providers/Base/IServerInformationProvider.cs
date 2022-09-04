using System;
using System.Threading.Tasks;
using PlayerCountBot;

namespace DiscordPlayerCountBot.Providers.Base
{
    public interface IServerInformationProvider
    {
        bool WasLastExecutionAFailure { get; set; }
        Exception? LastException { get; set; }
        Task<GenericServerInformation?> GetServerInformation(BotInformation information);
    }
}
