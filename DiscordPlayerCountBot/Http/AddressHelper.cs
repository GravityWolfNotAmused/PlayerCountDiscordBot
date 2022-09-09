using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace DiscordPlayerCountBot.Http
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
