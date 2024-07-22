namespace PlayerCountBot.ViewModels
{
    public class BaseViewModel
    {
        public string Address { get; set; }
        public int Port { get; set; }
        public int Players { get; set; }
        public int MaxPlayers { get; set; }
        public int QueuedPlayers { get; set; }

        public string ReplaceTagsWithValues(string? format, bool useNameAsLabel, string label = "")
        {
            string status = format ?? "";

            if (useNameAsLabel)
                status += $"{label} ";

            if (format == null)
            {
                status += $"{Players}/{MaxPlayers} ";

                if (QueuedPlayers > 0)
                    status += $"Q: {QueuedPlayers}";

                return status;
            }

            var type = GetType();
            var properties = type.GetProperties().ToList();

            foreach (var property in properties)
            {
                var tag = $"@{property.Name}";
                object? value = property.GetValue(this);

                if (format.Contains(tag) && value != null)
                {
                    if (property.Name == nameof(QueuedPlayers) && ((int?)value ?? 0) == 0)
                    {
                        status = status.Replace("Q: " + tag, "");
                        continue;
                    }

                    status = status.Replace(tag, value?.ToString());
                }
            }

            return status;
        }
    }
}