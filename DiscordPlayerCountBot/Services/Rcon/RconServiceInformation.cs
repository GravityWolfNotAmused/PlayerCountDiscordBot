using DiscordPlayerCountBot.Enums;
using DiscordPlayerCountBot.Services.Rcon.Praser;

namespace DiscordPlayerCountBot.Services.Rcon;

public interface IRconServiceInformation
{
    public abstract RconServiceType GetServiceType();
    public abstract string GetPlayerListCommand();
    public abstract IRconInformationParser GetParser();
}