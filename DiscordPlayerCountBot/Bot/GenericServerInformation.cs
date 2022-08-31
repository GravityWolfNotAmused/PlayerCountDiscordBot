using Microsoft.VisualBasic;

namespace PlayerCountBot
{
    public class GenericServerInformation
    {
        public string Address { get; set; }
        public int Port { get; set; }
        public int CurrentPlayers { get; set; } = 0;
        public int MaxPlayers { get; set; } = 0;
        public int PlayersInQueue { get; set; } = 0;

        public string GetStatusString(string label, bool userLabel)
        {
            string gameStatus = "";

            if (userLabel)
            {
                gameStatus += $"{label} ";
            }

            gameStatus += $"{CurrentPlayers}/{MaxPlayers} ";

            //This may change in the future when other games use this bot.
            int queueCount = PlayersInQueue;

            if (queueCount > 0)
            {
                gameStatus += $"Q: {queueCount}";
            }

            return gameStatus;
        }
    }
}
