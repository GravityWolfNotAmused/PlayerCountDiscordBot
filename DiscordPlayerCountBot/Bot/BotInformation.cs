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
        public bool UseNameAsLabel { get; set; }
        public int ProviderType { get; set; } = 0;
        public ulong? ChannelID { get; set; }
        public string? StatusFormat { get; set; }

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
