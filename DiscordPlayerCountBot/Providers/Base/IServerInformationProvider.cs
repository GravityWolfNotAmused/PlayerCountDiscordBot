using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DiscordPlayerCountBot.ViewModels;
using PlayerCountBot;

namespace DiscordPlayerCountBot.Providers.Base
{
    public interface IServerInformationProvider
    {
        bool WasLastExecutionAFailure { get; set; }
        Exception? LastException { get; set; }
        Task<BaseViewModel?> GetServerInformation(BotInformation information, Dictionary<string, string> applicationVariables);
    }
}
