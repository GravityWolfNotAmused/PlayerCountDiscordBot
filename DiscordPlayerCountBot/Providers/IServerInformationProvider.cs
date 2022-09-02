using System.Threading.Tasks;
using PlayerCountBot;

namespace DiscordPlayerCountBot.Providers
{
    public interface IServerInformationProvider
    {
        Task<GenericServerInformation?> GetServerInformation(BotInformation information);
    }
}
