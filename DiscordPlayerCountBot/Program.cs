using DiscordPlayerCountBot;
using log4net.Config;
using log4net;
using System.IO;
using System.Reflection;
using System;

var repository = LogManager.GetRepository(Assembly.GetCallingAssembly());
var fileInfo = new FileInfo(@"log4net.config");
XmlConfigurator.Configure(repository, fileInfo);

var tempLogger = LogManager.GetLogger(typeof(Program));
tempLogger.Info("[Application]:: Starting Controller.");

var controller = new UpdateController();
AppDomain.CurrentDomain.ProcessExit += new EventHandler(controller.OnProcessExit!);

controller.MainAsync().GetAwaiter().GetResult();