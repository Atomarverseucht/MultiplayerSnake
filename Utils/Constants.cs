using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerSnake.Database
{
    public static class Constants
    {
        /// <summary>
        /// database url
        /// </summary>
        public const string FIREBASE_URL = "https://psyched-canto-311609-default-rtdb.europe-west1.firebasedatabase.app";

        /// <summary>
        /// database app secret
        /// </summary>
        public const string FIREBASE_AUTH = "AIzaSyBbRpK_BcltEmRQzLAUCFykMHEq5PQWWz4";

        /// <summary>
        /// The firebase configuration
        /// </summary>
        public static FirebaseOptions FIREBASE_CONFIG = new FirebaseOptions
        {
            AuthTokenAsyncFactory = () => Task.FromResult(FIREBASE_AUTH)
        };

        /// <summary>
        /// the version of the client
        /// </summary>
        public const int CLIENT_VERSION = 11;

        /// <summary>
        /// the database key for the version
        /// </summary>
        public const string FIREBASE_KEY_VERSION = "version";

        /// <summary>
        /// All snake colors available
        /// </summary>
        public static readonly string[] COLORS = {"Aqua", "Yellow", "Red", "Black", "White", "DeepPink", "LawnGreen", "Orange",
            "SaddleBrown", "OrangeRed", "DarkViolet", "Gold", "Indigo", "Silver", "DarkGreen" };

        /// <summary>
        /// the delay between snake updates in ms
        /// </summary>
        public const int SNAKE_UPDATE_DELAY = 100;

        // the food levels
        public const int FOOD_LEVEL_LESS = 0;
        public const int FOOD_LEVEL_MEDIUM = 1;
        public const int FOOD_LEVEL_MUCH = 2;
        public const int FOOD_LEVEL_RANDOM = 3;

        // the food colors for the levels
        public const int FOOD_LEVEL_LESS_COLOR = 175;
        public const int FOOD_LEVEL_MEDIUM_COLOR = 110;
        public const int FOOD_LEVEL_MUCH_COLOR = 0;

        /// <summary>
        /// max player count (currently, set to the amount of colors, available)
        /// </summary>
        public static readonly int MAX_PLAYERS = COLORS.Length;

        // set the coordinate system resolution (always 16:9)
        public const int SNAKEBOARD_MAX_X = 1280;
        public const int SNAKEBOARD_MAX_Y = 720;
    }
}
