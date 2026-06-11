using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using DiscordPlayerCountBot.Providers.Base;
using GenericBot.Entities;
using Microsoft.Extensions.Logging;

namespace DiscordPlayerCountBot.Providers
{
    public class TeamSpeakProvider : IServerQueryProvider
    {
        private readonly ILogger<TeamSpeakProvider> _logger;

        public TeamSpeakProvider(ILogger<TeamSpeakProvider> logger)
        {
            _logger = logger;
        }

        public async Task<ServerInfo> GetServerInfoAsync(BotInformation information)
        {
            var parts = information.BotAddress.Split(':');
            if (parts.Length < 2 || !int.TryParse(parts[1], out int queryPort))
            {
                _logger.LogError("TeamSpeak address must be in format host:queryport (e.g. 127.0.0.1:10011)");
                return null;
            }

            string host = parts[0];
            int virtualServerId = information.ApplicationInformation?.TeamSpeakVirtualServerId ?? 1;

            try
            {
                using var client = new TcpClient();
                await client.ConnectAsync(host, queryPort);
                using var stream = client.GetStream();
                using var reader = new StreamReader(stream, Encoding.UTF8);
                using var writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

                await reader.ReadLineAsync();
                await reader.ReadLineAsync();

                await writer.WriteLineAsync($"use sid={virtualServerId}");
                await reader.ReadLineAsync();
                await reader.ReadLineAsync();

                await writer.WriteLineAsync("serverinfo");
                string response = await reader.ReadLineAsync();

                await writer.WriteLineAsync("quit");

                return ParseServerInfo(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to query TeamSpeak server at {Address}", information.BotAddress);
                return null;
            }
        }

        private ServerInfo ParseServerInfo(string response)
        {
            if (string.IsNullOrEmpty(response)) return null;

            int online = 0;
            int max = 0;

            foreach (var token in response.Split(' '))
            {
                if (token.StartsWith("virtualserver_clientsonline="))
                    int.TryParse(token.Split('=')[1], out online);
                else if (token.StartsWith("virtualserver_maxclients="))
                    int.TryParse(token.Split('=')[1], out max);
            }

            return new ServerInfo
            {
                CurrentPlayers = online,
                MaxPlayers = max
            };
        }
    }
}
