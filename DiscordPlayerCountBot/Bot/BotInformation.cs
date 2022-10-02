namespace PlayerCountBot
{
    public class BotInformation
    {
        [JsonIgnore]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Address { get; set; }
        public string Token { get; set; }
        public int Status { get; set; }
        public int ProviderType { get; set; } = 0;
        public ulong? ChannelID { get; set; }
        public string? StatusFormat { get; set; }

        public List<string> GetFormats() => StatusFormat?.Split("|").ToList() ?? new();

        [JsonIgnore]
        public int CurrentFormat = 0;

        public string? GetCurrentFormat()
        {
            if (string.IsNullOrEmpty(StatusFormat) || string.IsNullOrWhiteSpace(StatusFormat)) return null;

            var formats = GetFormats();

            if (CurrentFormat >= formats.Count)
            {
                CurrentFormat = 0;
            }

            var selectedFormat = formats[CurrentFormat];
            CurrentFormat++;

            return selectedFormat;
        }

        public Tuple<string, ushort> GetAddressAndPort()
        {
            string[] splitData = Address.Split(":");
            try
            {
                if (splitData.Length == 1)
                    return new Tuple<string, ushort>(splitData[0], 0);

                ushort port = ushort.Parse(splitData[1]);
                return new Tuple<string, ushort>(splitData[0], port);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
