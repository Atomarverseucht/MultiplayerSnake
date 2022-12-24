using MultiplayerSnake.Database;
using System;

namespace MultiplayerSnake.utils
{
    public class Utils
    {
        /// <summary>
        /// The random object used for the whole system
        /// </summary>
        public static readonly Random RANDOM = new Random();

        /// <summary>
        /// Get random coordinate for x on the board, min is the minimum distance to the wall from left
        /// </summary>
        /// <param name="min"></param>
        /// <returns></returns>
        public static int randomCoordinateX(int min = 0)
        {
            return (int) Math.Round(RANDOM.Next(min * 10, Constants.SNAKEBOARD_MAX_X - 10 + 1) / 10.0) * 10;
        }

        /// <summary>
        /// get random coordinate for y on the board
        /// </summary>
        /// <returns></returns>
        public static int randomCoordinateY()
        {
            return (int) Math.Round(RANDOM.Next(Constants.SNAKEBOARD_MAX_Y - 10 + 1) / 10.0) * 10;
        }

        /// <summary>
        /// Gets the current time and date in milliseconds.
        /// </summary>
        public static long currentTimeMillis()
        {
            return DateTime.UtcNow.Ticks / TimeSpan.TicksPerMillisecond;
        }

        /// <summary>
        /// Escapes html specific characters (like &lt; or &gt;)
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string htmlEntities(string str)
        {
            return str.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;");
        }
    }
}
