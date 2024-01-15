namespace PlayerCountBot.Enums
{
    public static class EnumHelper
    {
        public static readonly int[] DeprecatedDataProviders = new int[] { 2 };

        public static int GetDataProvider(int value)
        {
            return System.Enum.IsDefined(typeof(DataProvider), value) && !DeprecatedDataProviders.Contains(value) ? value : (int)DataProvider.STEAM;
        }

        public static int GetActivityType(int value)
        {
            return System.Enum.IsDefined(typeof(ActivityType), value) ? value : (int)ActivityType.CustomStatus;
        }

    }
}
