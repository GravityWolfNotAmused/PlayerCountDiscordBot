using PlayerCountBot.Services.SteamQuery;

namespace PlayerCountBot.Providers
{
    [Name("Steam Query")]
    public class SteamQueryProvider : ServerInformationProvider
    {
        private readonly SteamQueryService Service;

        public SteamQueryProvider(SteamQueryService service)
        {
            Service = service;
        }

        public override DataProvider GetRequiredProviderType()
        {
            return DataProvider.STEAMQUERY;
        }

        public async override Task<BaseViewModel?> GetServerInformation(BotInformation information, Dictionary<string, string> applicationVariables)
        {
            try
            {
                var addressAndPort = information.GetAddressAndPort();
                var response = await Service.GetQueryResponse(addressAndPort.Item1, addressAndPort.Item2);

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