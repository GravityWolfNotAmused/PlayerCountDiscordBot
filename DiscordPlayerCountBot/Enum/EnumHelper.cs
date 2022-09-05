using Discord;
using Microsoft.VisualBasic;

namespace DiscordPlayerCountBot.Enum
{
    public static class EnumHelper
    {
        public static int GetDataProvider(int value)
        {
            return System.Enum.IsDefined(typeof(DataProvider), value) ? value : (int)DataProvider.STEAM;
        }

        public static int GetActivityType(int value)
        {
            return System.Enum.IsDefined(typeof(ActivityType), value) ? value : (int)ActivityType.CustomStatus;
        }

    }
}
