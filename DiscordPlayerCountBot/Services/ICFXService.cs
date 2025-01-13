using DiscordPlayerCountBot.Data.CFX;

namespace DiscordPlayerCountBot.Services;

public interface ICFXService
{
    public Task<CFXServer?> GetServerInformationAsync(string address);
    public Task<List<CFXPlayerInformation>?> GetPlayerInformationAsync(string address);
}