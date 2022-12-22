using MultiplayerSnake.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerSnake.utils
{
    public class Utils
    {
        /// <summary>
        /// The random object used for the whole system
        /// </summary>
        public static readonly Random RANDOM = new Random();

        // get random coordinate for x on the board, min is the minimum distance to the wall from left
        public static int randomCoordinateX(int min = 0)
        {
            return (int) Math.Round(RANDOM.Next(min * 10, Constants.SNAKEBOARD_MAX_X - 10 + 1) / 10.0) * 10;
        }

        // get random coordinate for y on the board
        public static int randomCoordinateY()
        {
            return (int) Math.Round(RANDOM.Next(Constants.SNAKEBOARD_MAX_Y - 10 + 1) / 10.0) * 10;
        }
    }
}
