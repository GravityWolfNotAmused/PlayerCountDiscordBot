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

            using var client = new TcpClient();
            client.ReceiveTimeout = 5000;
            client.SendTimeout = 5000;
            await client.ConnectAsync(host, port);

            using var stream = client.GetStream();
            using var reader = new StreamReader(stream, Encoding.UTF8);
            using var writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

            await reader.ReadLineAsync();
            await reader.ReadLineAsync();

            if (!string.IsNullOrEmpty(information.QueryUsername) && !string.IsNullOrEmpty(information.QueryPassword))
            {
                await writer.WriteLineAsync($"login {information.QueryUsername} {information.QueryPassword}");
                await reader.ReadLineAsync();
                await reader.ReadLineAsync();
            }

            await writer.WriteLineAsync("use sid=1");
            await reader.ReadLineAsync();
            await reader.ReadLineAsync();

            await writer.WriteLineAsync("serverinfo");
            var response = await reader.ReadLineAsync();

            await writer.WriteLineAsync("quit");

            if (string.IsNullOrEmpty(response))
                throw new ApplicationException("Empty response from TeamSpeak server.");

            int online = 0, max = 0;
            foreach (var token in response.Split(' '))
            {
                if (token.StartsWith("virtualserver_clientsonline="))
                    int.TryParse(token.Split('=')[1], out online);
                else if (token.StartsWith("virtualserver_maxclients="))
                    int.TryParse(token.Split('=')[1], out max);
            }

            HandleLastException(information);

            return new BaseViewModel
            {
                Address = host,
                Port = port,
                Players = online,
                MaxPlayers = max
            };
        }
        catch (Exception e)
        {
            HandleException(e, information.Id.ToString());
            return null;
        }
    }
}
