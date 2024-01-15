using DiscordPlayerCountBot.Enums;
using DiscordPlayerCountBot.Exceptions;
using DiscordPlayerCountBot.Services.Rcon.ServiceInformation;
using RconSharp;

namespace DiscordPlayerCountBot.Services
{
    public class RconService : IRconService
    {
        private readonly Dictionary<RconServiceType, IRconServiceInformation> PlayerCommands = new()
        {
            {RconServiceType.CSGO, new CSGORconServiceInformation()},
            {RconServiceType.Minecraft, new MinecraftRconServiceInformation()},
            {RconServiceType.Ark, new ArkRconServiceInformation()}
        };

        public async Task<BaseViewModel> GetRconResponse(string address, int port, string authorizationToken, RconServiceType serviceType)
        {
            if (!PlayerCommands.TryGetValue(serviceType, out var serviceInformation))
            {
                throw new ArgumentException("Unsupported Rcon Service Type.... How did you get here?");
            }

            var client = RconClient.Create(address, port);

            await client.ConnectAsync();

            var authenticated = await client.AuthenticateAsync(authorizationToken);

            if (!authenticated)
            {
                throw new RconAuthenticationException($"There was a failure to authenticate with server: {address}:{port}.\nIs your password correct?\nIs this the correct server address?");
            }

            var command = serviceInformation.GetPlayerListCommand();
            var response = await client.ExecuteCommandAsync(command);
            var viewModel = serviceInformation.GetParser().Parse(response);

            viewModel.Address = address;
            viewModel.Port = port;

            return viewModel;
        }
    }
}
