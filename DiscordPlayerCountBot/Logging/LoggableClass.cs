namespace PlayerCountBot
{
    public class LoggableClass
    {
        protected readonly ILog Logger;
        public readonly BotInformation? Information;

        public LoggableClass()
        {
            Logger = LogManager.GetLogger(GetType());
        }

        public LoggableClass(BotInformation information) : this()
        {
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

        public void Debug(string message)
        {
            Logger.Debug($"{GetLoggingPrefix()} {message}");
        }
        public string GetLoggingPrefix()
        {
            var label = AttributeHelper.GetNameFromAttribute(this) ?? GetType().Name;

            if (Information != null)
            {
                return $"[{label}] - {Information.Id} -";
            }

            return $"[{label}] -";
        }
    }
}
