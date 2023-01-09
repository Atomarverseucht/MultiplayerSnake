using MultiplayerSnake.Database;
using System;
using System.Drawing;

namespace MultiplayerSnake.utils
{
    public static class Utils
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

        /// <summary>
        /// get the color for a food by the food level
        /// </summary>
        /// <param name="level">the level of the food</param>
        /// <returns>the hue of the color</returns>
        public static double getFoodColorByLevel(int level)
        {
            switch (level)
            {
                case Constants.FOOD_LEVEL_LESS:
                    return Constants.FOOD_LEVEL_LESS_COLOR;
                case Constants.FOOD_LEVEL_MEDIUM:
                    return Constants.FOOD_LEVEL_MEDIUM_COLOR;
                case Constants.FOOD_LEVEL_MUCH:
                    return Constants.FOOD_LEVEL_MUCH_COLOR;
                case Constants.FOOD_LEVEL_RANDOM:
                    return Utils.RANDOM.Next(37) * 10;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// convert the food level count to the count of the parts that must be added/removed
        /// </summary>
        /// <param name="level">the food level</param>
        public static int foodLevelToCount(int level)
        {
            switch (level)
            {
                case Constants.FOOD_LEVEL_LESS:
                case Constants.FOOD_LEVEL_MEDIUM:
                case Constants.FOOD_LEVEL_MUCH:
                    return level + 1;
                case Constants.FOOD_LEVEL_RANDOM:
                    return RANDOM.Next(-5, 5);
                default:
                    return 0;
            }
        }

        //
        // Everything below from: https://stackoverflow.com/questions/1335426/is-there-a-built-in-c-net-system-api-for-hsv-to-rgb
        //

        /// <summary>
        /// Converts a Color object to HSV.
        /// </summary>
        /// <param name="color">The color to convert</param>
        /// <param name="hue">The hue.</param>
        /// <param name="saturation">The saturation.</param>
        /// <param name="value">The value/brightness.</param>
        public static void ColorToHSV(Color color, out double hue, out double saturation, out double value)
        {
            int max = Math.Max(color.R, Math.Max(color.G, color.B));
            int min = Math.Min(color.R, Math.Min(color.G, color.B));

            hue = color.GetHue();
            saturation = (max == 0) ? 0 : 1d - (1d * min / max);
            value = max / 255d;
        }

        /// <summary>
        /// Converts HSV to a Color object.
        /// </summary>
        /// <param name="hue">The hue.</param>
        /// <param name="saturation">The saturation.</param>
        /// <param name="value">The value/brightness.</param>
        /// <returns>the generated color object.</returns>
        public static Color ColorFromHSV(double hue, double saturation, double value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            try
            {
                if (hi == 0)
                    return Color.FromArgb(255, v, t, p);
                else if (hi == 1)
                    return Color.FromArgb(255, q, v, p);
                else if (hi == 2)
                    return Color.FromArgb(255, p, v, t);
                else if (hi == 3)
                    return Color.FromArgb(255, p, q, v);
                else if (hi == 4)
                    return Color.FromArgb(255, t, p, v);
                else
                    return Color.FromArgb(255, v, p, q);
            } catch (Exception ex)
            {
                return Color.White;
            }
            
        }
    }
}
