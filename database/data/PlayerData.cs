using System.Collections.Concurrent;

namespace MultiplayerSnake.database.data
{
    public class PlayerData
    {
        /// <summary>
        /// color of the player as HTML color name
        /// </summary>
        public string color { get; set; }

        /// <summary>
        /// used for verification of the player sending data
        /// </summary>
        public string verifyName { get; set; }

        /// <summary>
        /// the current position list of the player
        /// </summary>
        public ConcurrentBag<PlayerPositionData> pos { get; set; }
    }
}
