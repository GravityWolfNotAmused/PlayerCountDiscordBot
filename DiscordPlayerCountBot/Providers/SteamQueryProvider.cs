using DiscordPlayerCountBot.Services.SteamQuery;

namespace PlayerCountBot.Providers
{
    [Name("Steam Query")]
    public class SteamQueryProvider : ServerInformationProvider
    {
        public SteamQueryProvider(BotInformation info) : base(info)
        {
        }

        public async override Task<BaseViewModel?> GetServerInformation(BotInformation information, Dictionary<string, string> applicationVariables)
        {
            var service = new SteamQueryService();

            try
            {
                var addressAndPort = information.GetAddressAndPort();
                var response = await service.GetQueryResponse(addressAndPort.Item1, addressAndPort.Item2);

                if (response == null)
                {
                    throw new ApplicationException($" Failed to get a Server Information response from Steam Query.");
                }

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
