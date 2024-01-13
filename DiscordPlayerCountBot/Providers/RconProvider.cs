using DiscordPlayerCountBot.Enums;
using DiscordPlayerCountBot.Enums;
using DiscordPlayerCountBot.Exceptions;
using DiscordPlayerCountBot.Services;

namespace PlayerCountBot.Providers
{
    [Name("Rcon")]
    public class RconProvider : ServerInformationProvider
    {
        public RconProvider(BotInformation info) : base(info)
        {
        }

        public async override Task<BaseViewModel?> GetServerInformation(BotInformation information, Dictionary<string, string> applicationVariables)
        {
            var values = $"Valid Values: {string.Join(",", Enum.GetNames<RconServiceType>())}";

            if (information.RconServiceName == null)
            {
                throw new ConfigurationException($"Bot: {information.Name} must have RconServiceName specified in it's config. {values}");
            }

            var service = new RconService();
            
            if(!Enum.TryParse<RconServiceType>(information.RconServiceName, true, out var serviceType))
            {
                throw new ConfigurationException($"Bot: {information.Name} has an invalid RconServiceName specified in it's config. {values}");
            }

            try
            {
                var addressAndPort = information.GetAddressAndPort();
                var response = await service.GetRconResponse(addressAndPort.Item1, addressAndPort.Item2, applicationVariables["RconPassword"], serviceType);

                if(response == null)
                    throw new ApplicationException($"Server Address: {information.Address} was not found in Steam's directory.");

                HandleLastException(information);

                return response;
            }
            catch (Exception e)
            {
                HandleException(e);
                return null;
            }
        }
    }
}
