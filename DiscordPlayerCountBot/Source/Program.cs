using log4net;
using log4net.Config;
using System;
using System.IO;
using System.Reflection;

namespace PlayerCountBot
{
    public class Program
    {
        static UpdateController controller;

        static void Main(string[] args)
        {
            var repository = LogManager.GetRepository(Assembly.GetCallingAssembly());
            var fileInfo = new FileInfo(@"log4net.config");
            XmlConfigurator.Configure(repository, fileInfo);

            var tempLogger = LogManager.GetLogger(typeof(Program));
            tempLogger.Info("[PlayerCountBot]:: Starting Bot Controller.");

            controller = new UpdateController();
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(controller.OnProcessExit);

            controller.MainAsync().GetAwaiter().GetResult();
        }
    }
}
