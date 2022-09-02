using DiscordPlayerCountBot.Source.CFX;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordPlayerCountBot.Services
{
    public interface ICFXService
    {
        public Task<CFXServer> GetServerInformationAsync(string address);
        public Task<List<CFXPlayerInformation>> GetPlayerInformationAsync(string address);
    }
}
