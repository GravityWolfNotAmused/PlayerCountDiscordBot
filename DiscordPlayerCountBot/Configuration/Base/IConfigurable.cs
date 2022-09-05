using PlayerCountBot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordPlayerCountBot.Configuration.Base
{
    public interface IConfigurable
    {
        Task<Tuple<Dictionary<string, Bot>, int>> Configure();
    }
}
