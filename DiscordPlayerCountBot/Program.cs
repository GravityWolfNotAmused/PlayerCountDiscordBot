global using Newtonsoft.Json;
global using System.Text;

global using Discord;
global using Discord.WebSocket;

using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using Microsoft.Extensions.DependencyInjection;

using DiscordPlayerCountBot.EnvironmentParser.Base;
using DiscordPlayerCountBot.EnvironmentParser;
using DiscordPlayerCountBot.Configuration;
using DiscordPlayerCountBot;
using DiscordPlayerCountBot.Providers.Base;
using DiscordPlayerCountBot.Providers;
using DiscordPlayerCountBot.Services.SteamQuery;
using DiscordPlayerCountBot.Services;
using DiscordPlayerCountBot.Configuration.Base;
using DiscordPlayerCountBot.Services.Rcon;
using DiscordPlayerCountBot.Services.Rcon.ServiceInformation;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(theme: AnsiConsoleTheme.Literate, outputTemplate: "[{Timestamp:HH:mm:ss}] [{Level:u3}] {Message:lj}{NewLine}{Exception}", restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Debug, applyThemeToRedirectedOutput: true)
    .WriteTo.File("logs.txt", Serilog.Events.LogEventLevel.Warning)
    .CreateLogger();

Log.Information("[Application] - Starting Player Count Discord Bot.");

var serviceCollection = new ServiceCollection()
    .AddSingleton<UpdateController>();

serviceCollection.AddTransient<IConfigurable, StandardConfiguration>();
serviceCollection.AddTransient<IConfigurable, DockerConfiguration>();

serviceCollection.AddSingleton<IEnvironmentParser, BotNameParser>();
serviceCollection.AddSingleton<IEnvironmentParser, BotAddressParser>();
serviceCollection.AddSingleton<IEnvironmentParser, BotPortParser>();
serviceCollection.AddSingleton<IEnvironmentParser, BotTokenParser>();
serviceCollection.AddSingleton<IEnvironmentParser, BotStatusParser>();
serviceCollection.AddSingleton<IEnvironmentParser, BotTagParser>();
serviceCollection.AddSingleton<IEnvironmentParser, BotProviderTypeParser>();
serviceCollection.AddSingleton<IEnvironmentParser, BotStatusFormatParser>();
serviceCollection.AddSingleton<IEnvironmentParser, BotApplicationVariableParser>();
serviceCollection.AddSingleton<IEnvironmentParser, BotChannelIdParser>();
serviceCollection.AddSingleton<IEnvironmentParser, BotUpdateTimeParser>();

serviceCollection.AddTransient<SteamService>();
serviceCollection.AddTransient<SteamQueryService>();
serviceCollection.AddTransient<BattleMetricsService>();
serviceCollection.AddTransient<CFXService>();
serviceCollection.AddTransient<MinecraftService>();
serviceCollection.AddTransient<RconService>();

serviceCollection.AddTransient<IServerInformationProvider, SteamProvider>();
serviceCollection.AddTransient<IServerInformationProvider, CFXProvider>();
serviceCollection.AddTransient<IServerInformationProvider, MinecraftProvider>();
serviceCollection.AddTransient<IServerInformationProvider, BattleMetricsProvider>();
serviceCollection.AddTransient<IServerInformationProvider, RconProvider>();
serviceCollection.AddTransient<IServerInformationProvider, SteamQueryProvider>();

serviceCollection.AddTransient<IRconServiceInformation, CSGORconServiceInformation>();
serviceCollection.AddTransient<IRconServiceInformation, MinecraftRconServiceInformation>();
serviceCollection.AddTransient<IRconServiceInformation, ArkRconServiceInformation>();

var app = serviceCollection.BuildServiceProvider();

var controller = app.GetRequiredService<UpdateController>();
await controller.MainAsync();