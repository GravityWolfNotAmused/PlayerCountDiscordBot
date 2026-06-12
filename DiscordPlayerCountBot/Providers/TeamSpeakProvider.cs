using DiscordPlayerCountBot.Attributes;
using DiscordPlayerCountBot.Bot;
using DiscordPlayerCountBot.Enums;
using DiscordPlayerCountBot.Providers.Base;
using DiscordPlayerCountBot.ViewModels;
using System.Net.Sockets;
using System.Text;

namespace DiscordPlayerCountBot.Providers;

[Name("TeamSpeak")]
public class TeamSpeakProvider : ServerInformationProvider
{
    public override DataProvider GetRequiredProviderType()
    {
        return DataProvider.TEAMSPEAK;
    }

    public async override Task<BaseViewModel?> GetServerInformation(BotInformation information, Dictionary<string, string> applicationVariables)
    {
        try
        {
            var addressAndPort = information.GetAddressAndPort();
            var host = addressAndPort.Item1;
            var port = addressAndPort.Item2 == 0 ? (ushort)10011 : addressAndPort.Item2;

            Console.WriteLine($"[TS] Connecting {host}:{port}");

            using var client = new TcpClient();
            client.ReceiveTimeout = 8000;
            client.SendTimeout = 8000;
            await client.ConnectAsync(host, port);

            using var stream = client.GetStream();
            using var reader = new StreamReader(stream, Encoding.UTF8);
            using var writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

            // baca 3 baris banner: "TS3", "", "Welcome to..."
            var b1 = await reader.ReadLineAsync();
            var b2 = await reader.ReadLineAsync();
            var b3 = await reader.ReadLineAsync();
            Console.WriteLine($"[TS] B1: {b1}");
            Console.WriteLine($"[TS] B2: {b2}");
            Console.WriteLine($"[TS] B3: {b3}");

            await writer.WriteLineAsync("use sid=1");
            var useR1 = await reader.ReadLineAsync();
            var useR2 = await reader.ReadLineAsync();
            Console.WriteLine($"[TS] Use R1: {useR1}");
            Console.WriteLine($"[TS] Use R2: {useR2}");

            await writer.WriteLineAsync("serverinfo");
            var response = await reader.ReadLineAsync();
            Console.WriteLine($"[TS] Serverinfo: {response?.Substring(0, Math.Min(100, response?.Length ?? 0))}");

            await writer.WriteLineAsync("quit");

            if (string.IsNullOrEmpty(response) || !response.StartsWith("virtualserver_"))
                throw new ApplicationException($"Unexpected response: {response}");

            int online = 0, max = 0, queryClients = 0;
            foreach (var token in response.Split(' '))
            {
                if (token.StartsWith("virtualserver_clientsonline="))
                    int.TryParse(token.Split('=')[1], out online);
                else if (token.StartsWith("virtualserver_maxclients="))
                    int.TryParse(token.Split('=')[1], out max);
                else if (token.StartsWith("virtualserver_queryclientsonline="))
                    int.TryParse(token.Split('=')[1], out queryClients);
            }

            Console.WriteLine($"[TS] Result: online={online}, max={max}, query={queryClients}, players={Math.Max(0, online - queryClients)}");

            HandleLastException(information);

            return new BaseViewModel
            {
                Address = host,
                Port = port,
                Players = Math.Max(0, online - queryClients),
                MaxPlayers = max
            };
        }
        catch (Exception e)
        {
            Console.WriteLine($"[TS] Exception: {e.Message}");
            HandleException(e, information.Id.ToString());
            return null;
        }
    }
}
