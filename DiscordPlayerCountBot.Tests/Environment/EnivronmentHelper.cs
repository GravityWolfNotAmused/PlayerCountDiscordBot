namespace PlayerCountBot.Tests.Environment
{
    public static class EnvironmentHelper
    {
        public static void SetTestEnvironmentWithAllVariables()
        {
            System.Environment.SetEnvironmentVariable("ISDOCKER", "true");
            System.Environment.SetEnvironmentVariable("BOT_NAMES", "Steam;CFX;Scum;Minecraft;BattleMetrics");
            System.Environment.SetEnvironmentVariable("BOT_PUBADDRESSES", "51.222.191.212;96.43.133.194;178.63.74.20;minecraft.hypixel.net;15086629");
            System.Environment.SetEnvironmentVariable("BOT_PORTS", "2403;30120;7042;25565;0");
            System.Environment.SetEnvironmentVariable("BOT_DISCORD_TOKENS", "1;2;3;4;5");
            System.Environment.SetEnvironmentVariable("BOT_USENAMETAGS", "false;false;false;false;false");
            System.Environment.SetEnvironmentVariable("BOT_STATUSES", "0;0;0;0;0");
            System.Environment.SetEnvironmentVariable("BOT_PROVIDERTYPES", "3;3;3;3;3");
            System.Environment.SetEnvironmentVariable("BOT_UPDATE_TIME", "10");
            System.Environment.SetEnvironmentVariable("BOT_APPLICATION_VARIABLES", "SteamAPIKey,12345;BattleMetricsKey,12345");
        }
        public static void SetTestEnvironmentWithDuplicateAddresses()
        {
            System.Environment.SetEnvironmentVariable("ISDOCKER", "true");
            System.Environment.SetEnvironmentVariable("BOT_NAMES", "Steam;CFX;Scum;Minecraft;BattleMetrics;BattleMetrics2");
            System.Environment.SetEnvironmentVariable("BOT_PUBADDRESSES", "51.222.191.212;96.43.133.194;178.63.74.20;minecraft.hypixel.net;15086629;15086629");
            System.Environment.SetEnvironmentVariable("BOT_PORTS", "2403;30120;7042;25565;0;0");
            System.Environment.SetEnvironmentVariable("BOT_DISCORD_TOKENS", "1;2;3;4;5;6");
            System.Environment.SetEnvironmentVariable("BOT_USENAMETAGS", "false;false;false;false;false;false");
            System.Environment.SetEnvironmentVariable("BOT_STATUSES", "0;0;0;0;0;0");
            System.Environment.SetEnvironmentVariable("BOT_PROVIDERTYPES", "3;3;3;3;3;3");
            System.Environment.SetEnvironmentVariable("BOT_UPDATE_TIME", "10");
            System.Environment.SetEnvironmentVariable("BOT_APPLICATION_VARIABLES", "SteamAPIKey,12345;BattleMetricsKey,12345");
        }
        public static void SetTestEnvironmentWithoutSteam()
        {
            System.Environment.SetEnvironmentVariable("ISDOCKER", "true");
            System.Environment.SetEnvironmentVariable("BOT_NAMES", "CFX;Scum;Minecraft;BattleMetrics");
            System.Environment.SetEnvironmentVariable("BOT_PUBADDRESSES", "96.43.133.194;178.63.74.20;minecraft.hypixel.net;15086629");
            System.Environment.SetEnvironmentVariable("BOT_PORTS", "30120;7042;25565;0");
            System.Environment.SetEnvironmentVariable("BOT_DISCORD_TOKENS", "2;3;4;5");
            System.Environment.SetEnvironmentVariable("BOT_USENAMETAGS", "false;false;false;false");
            System.Environment.SetEnvironmentVariable("BOT_STATUSES", "0;0;0;0;0");
            System.Environment.SetEnvironmentVariable("BOT_PROVIDERTYPES", "3;3;3;3");
            System.Environment.SetEnvironmentVariable("BOT_UPDATE_TIME", "10");
            System.Environment.SetEnvironmentVariable("BOT_APPLICATION_VARIABLES", "BattleMetricsKey,12345");
        }

        public static void SetTestEnvironmentWithoutBattleMetrics()
        {
            System.Environment.SetEnvironmentVariable("ISDOCKER", "true");
            System.Environment.SetEnvironmentVariable("BOT_NAMES", "Steam;CFX;Scum;Minecraft");
            System.Environment.SetEnvironmentVariable("BOT_PUBADDRESSES", "51.222.191.212;96.43.133.194;178.63.74.20;minecraft.hypixel.net");
            System.Environment.SetEnvironmentVariable("BOT_PORTS", "2403;30120;7042;25565");
            System.Environment.SetEnvironmentVariable("BOT_DISCORD_TOKENS", "1;2;3;4");
            System.Environment.SetEnvironmentVariable("BOT_USENAMETAGS", "false;false;false;false");
            System.Environment.SetEnvironmentVariable("BOT_STATUSES", "0;0;0;0;0");
            System.Environment.SetEnvironmentVariable("BOT_PROVIDERTYPES", "3;3;3;3");
            System.Environment.SetEnvironmentVariable("BOT_UPDATE_TIME", "10");
            System.Environment.SetEnvironmentVariable("BOT_APPLICATION_VARIABLES", "SteamAPIKey,12345");
        }

        public static void SetTestEnvironmentWithoutApplicationVariables()
        {
            System.Environment.SetEnvironmentVariable("ISDOCKER", "true");
            System.Environment.SetEnvironmentVariable("BOT_NAMES", "Steam;CFX;Scum;Minecraft;BattleMetrics");
            System.Environment.SetEnvironmentVariable("BOT_PUBADDRESSES", "51.222.191.212;96.43.133.194;178.63.74.20;minecraft.hypixel.net;15086629");
            System.Environment.SetEnvironmentVariable("BOT_PORTS", "2403;30120;7042;25565;0");
            System.Environment.SetEnvironmentVariable("BOT_DISCORD_TOKENS", "1;2;3;4;5");
            System.Environment.SetEnvironmentVariable("BOT_USENAMETAGS", "false;false;false;false;false");
            System.Environment.SetEnvironmentVariable("BOT_STATUSES", "0;0;0;0;0");
            System.Environment.SetEnvironmentVariable("BOT_PROVIDERTYPES", "3;3;3;3;3");
            System.Environment.SetEnvironmentVariable("BOT_UPDATE_TIME", "10");
        }

        public static void ClearTestEnvironmentVariables()
        {
            System.Environment.SetEnvironmentVariable("ISDOCKER", null);
            System.Environment.SetEnvironmentVariable("BOT_NAMES", null);
            System.Environment.SetEnvironmentVariable("BOT_PUBADDRESSES", null);
            System.Environment.SetEnvironmentVariable("BOT_PORTS", null);
            System.Environment.SetEnvironmentVariable("BOT_DISCORD_TOKENS", null);
            System.Environment.SetEnvironmentVariable("BOT_USENAMETAGS", null);
            System.Environment.SetEnvironmentVariable("BOT_STATUSES", null);
            System.Environment.SetEnvironmentVariable("BOT_PROVIDERTYPES", null);
            System.Environment.SetEnvironmentVariable("BOT_UPDATE_TIME", null);
            System.Environment.SetEnvironmentVariable("BOT_APPLICATION_VARIABLES", null);
        }
    }
}
