using DiscordPlayerCountBot.ViewModels;

namespace DiscordPlayerCountBot.Services.Rcon.Praser;

public interface IRconInformationParser
{
    public BaseViewModel Parse(string message);
}