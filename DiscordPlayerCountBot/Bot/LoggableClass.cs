using DiscordPlayerCountBot.Attributes;
using log4net;
using System;

namespace PlayerCountBot
{
    public class LoggableClass
    {
        protected readonly ILog Logger;
        public readonly BotInformation Information;

        public LoggableClass(BotInformation information)
        {
            Logger = LogManager.GetLogger(GetType());
            Information = information;
        }

        public void Info(string message)
        {
            Logger.Info($"{GetLoggingPrefix()} {message}");
        }

        public void Warn(string message)
        {
            Logger.Warn($"{GetLoggingPrefix()} {message}");
        }

        public void Error(string message, Exception? exception = null)
        {
            Logger.Error($"{GetLoggingPrefix()} {message}", exception);
        }

        public string GetLoggingPrefix()
        {
            var label = AttributeHelper.GetNameFromAttribute(this) ?? GetType().Name;
            var returnString = $"[{label}] - {Information.Id} -";

            return returnString;
        }
    }
}
