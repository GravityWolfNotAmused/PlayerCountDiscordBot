global using PlayerCountBot;
global using PlayerCountBot.Attributes;
global using PlayerCountBot.Configuration.Base;
global using PlayerCountBot.Data;
global using PlayerCountBot.Enums;
global using PlayerCountBot.Http;
global using PlayerCountBot.Json;
global using PlayerCountBot.Services;
global using PlayerCountBot.Providers.Base;
global using PlayerCountBot.Providers;
global using PlayerCountBot.ViewModels;
global using Newtonsoft.Json;
global using System.Text;

global using Discord;
global using Discord.WebSocket;

using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using Microsoft.Extensions.DependencyInjection;
using PlayerCountBot.Configuration;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(theme: AnsiConsoleTheme.Literate, outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}", restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Debug, applyThemeToRedirectedOutput: true)
    .WriteTo.File("logs.txt", Serilog.Events.LogEventLevel.Warning)
    .CreateLogger();

Log.Information("[Application] - Starting Player Count Discord Bot.");

var serviceCollection = new ServiceCollection()
    .AddSingleton<UpdateController>();

serviceCollection.AddTransient<IConfigurable, StandardConfiguration>();
serviceCollection.AddTransient<IConfigurable, DockerConfiguration>();

serviceCollection.AddTransient<IServerInformationProvider, SteamProvider>();
serviceCollection.AddTransient<IServerInformationProvider, CFXProvider>();
serviceCollection.AddTransient<IServerInformationProvider, MinecraftProvider>();
serviceCollection.AddTransient<IServerInformationProvider, BattleMetricsProvider>();


var app = serviceCollection.BuildServiceProvider();

var controller = app.GetRequiredService<UpdateController>();
await controller.MainAsync();