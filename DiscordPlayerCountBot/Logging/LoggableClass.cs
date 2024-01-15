using Serilog;

namespace PlayerCountBot
{
    public class LoggableClass
    {
        public void Info(string message, string? id = null)
        {
            Log.Information(BuildLogMessage(id, GetType().Name, message));
        }

        public void Warn(string message, string? id = null)
        {
            Log.Warning(BuildLogMessage(id, GetType().Name, message));
        }

        public void Error(string message, string? id = null, Exception? exception = null)
        {
            Log.Error(BuildLogMessage(id, GetType().Name, message), exception);
        }

        public void Debug(string message, string? id = null)
        {
            Log.Debug(BuildLogMessage(id, GetType().Name, message));
        }

        public string BuildLogMessage(string? id, string label, string msg)
        {
            var message = "";

            message += $"[{label}]";

            if (!string.IsNullOrEmpty(id))
            {
                message += $" - {id}";
            }

            return message + $" - {msg}";
        }
    }
}
