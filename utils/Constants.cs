using Firebase.Database;
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
        /// database api key
        /// </summary>
        public const string FIREBASE_API_KEY = "yQ1sXul4apTcUJgy6JwmW5lZ3r8uMbFn2tdQmbIY";

        //public static readonly FirebaseAuthProvider FIREBASE_AUTH_PROVIDER = new FirebaseAuthProvider(new FirebaseConfig(FIREBASE_API_KEY));

        /// <summary>
        /// The firebase configuration
        /// </summary>
        public static FirebaseOptions FIREBASE_CONFIG = new FirebaseOptions
        {
            AuthTokenAsyncFactory = () => Task.FromResult(FIREBASE_API_KEY)
        };

        /// <summary>
        /// the version of the client
        /// </summary>
        public const int CLIENT_VERSION = 12;

        /// <summary>
        /// the database key for the version
        /// </summary>
        public const string FIREBASE_VERSION_KEY = "variables/version";

        /// <summary>
        /// 
        /// </summary>
        public const string FIREBASE_FOODS_KEY = "foods";

        /// <summary>
        /// the database key for the root of a player's data<br/>
        /// %name% must be replaced with the player's name
        /// </summary>
        public const string FIREBASE_PLAYER_KEY = "players/%name%";

        /// <summary>
        /// the database key for a player's position data.<br/>
        /// %name% must be replaced with the player's name
        /// </summary>
        public const string FIREBASE_PLAYER_POS_KEY = FIREBASE_PLAYER_KEY + "/pos";

        /// <summary>
        /// the database key for the player color.<br/>
        /// %name% must be replaced with the player's name
        /// </summary>
        public const string FIREBASE_PLAYER_COLOR_KEY = FIREBASE_PLAYER_KEY + "/color";

        /// <summary>
        /// the database key for the player verify system.<br/>
        /// %name% must be replaced with the player's name
        /// </summary>
        public const string FIREBASE_PLAYER_VERIFY_NAME_KEY = FIREBASE_PLAYER_KEY + "/verifyName";

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
