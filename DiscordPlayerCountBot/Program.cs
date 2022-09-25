global using PlayerCountBot;
global using PlayerCountBot.Attributes;
global using PlayerCountBot.Configuration.Base;
global using PlayerCountBot.Data;
global using PlayerCountBot.Enum;
global using PlayerCountBot.Http;
global using PlayerCountBot.Json;
global using PlayerCountBot.Services;
global using PlayerCountBot.Providers.Base;
global using PlayerCountBot.Providers;
global using PlayerCountBot.ViewModels;
global using Newtonsoft.Json;
global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Threading.Tasks;
global using System.IO;
global using System.Text;

global using Discord;
global using Discord.WebSocket;
global using log4net;

using log4net.Config;
using System.Reflection;

var repository = LogManager.GetRepository(Assembly.GetCallingAssembly());
var fileInfo = new FileInfo(@"log4net.config");
XmlConfigurator.Configure(repository, fileInfo);

var tempLogger = LogManager.GetLogger(typeof(Program));
tempLogger.Info("[Application]:: Starting Controller.");

var controller = new UpdateController();
AppDomain.CurrentDomain.ProcessExit += new EventHandler(controller.OnProcessExit!);

controller.MainAsync().GetAwaiter().GetResult();