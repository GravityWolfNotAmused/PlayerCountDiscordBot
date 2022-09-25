namespace PlayerCountBot.Http
{
    public static class AddressHelper
    {
        public static async Task<string> GetHostAddress()
        {
            var publicIPAddress = string.Empty;
            var httpClient = new HttpExecuter();
            var ipAddress = await httpClient.GET<object, string>("http://ifconfig.me");

            if (string.IsNullOrEmpty(ipAddress))
                throw new ApplicationException("IP Address cannot be null. Host failed to resolve address.");

            return ipAddress;
        }

        public static async Task<string> ResolveAddress(string address)
        {
            var splitAddr = address.Split(":");
            var resolvedAddress = await GetHostAddress();
            var port = splitAddr[1];
            return resolvedAddress + ":" + port;
        }
    }
}
