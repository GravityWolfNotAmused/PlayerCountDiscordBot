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

    private async Task<string?> ReadUntilError(StreamReader reader)
    {
        string? last = null;
        string? line;
        while ((line = await reader.ReadLineAsync()) != null)
        {
            if (line.StartsWith("virtualserver_"))
                last = line;
            if (line.StartsWith("error"))
                return line.Contains("id=0") ? (last ?? line) : throw new ApplicationException(line);
        }
        return last;
    }

    private async Task<bool> TrySendLogin(StreamWriter writer, StreamReader reader, string username, string password)
    {
        await writer.WriteLineAsync($"login {username} {password}");
        string? line;
        while ((line = await reader.ReadLineAsync()) != null)
        {
            if (line.StartsWith("error"))
                return line.Contains("id=0");
        }
        return false;
    }

    public async override Task<BaseViewModel?> GetServerInformation(BotInformation information, Dictionary<string, string> applicationVariables)
    {
        try
        {
            var addressAndPort = information.GetAddressAndPort();
            var host = addressAndPort.Item1;
            var port = addressAndPort.Item2 == 0 ? (ushort)10011 : addressAndPort.Item2;

            using var client = new TcpClient();
            client.ReceiveTimeout = 8000;
            client.SendTimeout = 8000;
            await client.ConnectAsync(host, port);

            using var stream = client.GetStream();
            using var reader = new StreamReader(stream, Encoding.UTF8);
            using var writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };


            string? line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                if (line.Contains("for a list of commands"))
                    break;
            }


            if (!string.IsNullOrEmpty(information.QueryUsername) && !string.IsNullOrEmpty(information.QueryPassword))
            {
                bool loggedIn = false;
                for (int i = 0; i < 5; i++)
                {
                    await Task.Delay(300);
                    loggedIn = await TrySendLogin(writer, reader, information.QueryUsername, information.QueryPassword);
                    if (loggedIn) break;
                }
                if (!loggedIn)
                    throw new ApplicationException("Login failed after 5 attempts.");
            }

            await writer.WriteLineAsync("use sid=1");
            await ReadUntilError(reader);

            await writer.WriteLineAsync("serverinfo");
            var response = await ReadUntilError(reader);

            await writer.WriteLineAsync("quit");

            if (string.IsNullOrEmpty(response) || !response.StartsWith("virtualserver_"))
                throw new ApplicationException($"Unexpected serverinfo response: {response}");

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
            HandleException(e, information.Id.ToString());
            return null;
        }
    }
}
