global using Newtonsoft.Json;
global using System.Text;

global using Discord;
global using Discord.WebSocket;

using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using Microsoft.Extensions.DependencyInjection;

using DiscordPlayerCountBot.EnvironmentParser.Base;
using DiscordPlayerCountBot.EnvironmentParser;
using DiscordPlayerCountBot;
using DiscordPlayerCountBot.Providers.Base;
using DiscordPlayerCountBot.Services.SteamQuery;
using DiscordPlayerCountBot.Services;
using DiscordPlayerCountBot.Configuration.Base;
using DiscordPlayerCountBot.Services.Rcon;
using DiscordPlayerCountBot.Extensions;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(theme: AnsiConsoleTheme.Literate, outputTemplate: "[{Timestamp:HH:mm:ss}] [{Level:u3}] {Message:lj}{NewLine}{Exception}", restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Debug, applyThemeToRedirectedOutput: true)
    .WriteTo.File("logs.txt", Serilog.Events.LogEventLevel.Warning)
    .CreateLogger();

Log.Information("[Application] - Starting Player Count Discord Bot.");

var serviceCollection = new ServiceCollection();

serviceCollection.AddAllImplementationsOf<IEnvironmentParser>();
serviceCollection.AddAllImplementationsOf<IConfigurable>();

serviceCollection.AddTransient<SteamService>();
serviceCollection.AddTransient<SteamQueryService>();
serviceCollection.AddTransient<BattleMetricsService>();
serviceCollection.AddTransient<CFXService>();
serviceCollection.AddTransient<MinecraftService>();
serviceCollection.AddTransient<RconService>();

serviceCollection.AddAllImplementationsOf<IServerInformationProvider>(true);
serviceCollection.AddAllImplementationsOf<IRconServiceInformation>(true);

serviceCollection.AddSingleton<EnvironmentParserResolver>();
serviceCollection.AddSingleton<UpdateController>();

var app = serviceCollection.BuildServiceProvider();

var controller = app.GetRequiredService<UpdateController>();
await controller.MainAsync();
