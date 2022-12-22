using MultiplayerSnake.database.data;
using MultiplayerSnake.Database;
using MultiplayerSnake.utils;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Linq;
using System.Windows.Forms;

namespace MultiplayerSnake.game
{
    public class PlayerManager
    {
        // access to main form
        private MainForm mainForm;

        // access to database
        private Firebase firebase;

        // food manager
        private FoodManager foodManager;

        // the player name
        public string name = "";

        // our snake color
        public string color = "";

        // the other players
        public ConcurrentDictionary<string, PlayerData> otherSnakes = new ConcurrentDictionary<string, PlayerData>();

        // used to count online (registered) players
        public ConcurrentDictionary<string, PlayerData> allSnakes = new ConcurrentDictionary<string, PlayerData>();

        public PlayerManager(MainForm mainForm)
        {
            this.mainForm = mainForm;
        }

        /// <summary>
        /// This must be called, when all class variables in main form have been set.
        /// </summary>
        public void init()
        {
            this.firebase = this.mainForm.firebase;
            this.foodManager = this.mainForm.foodManager;
        }

        /// <summary>
        /// Get the current online players.
        /// </summary>
        public int getOnlinePlayers()
        {
            return this.allSnakes.Count;
        }

        /// <summary>
        /// Get the players, currently playing.
        /// </summary>
        public int getActivePlayers()
        {
            int count = 0;

            // go threw all snakes
            foreach (PlayerData snake in this.allSnakes.Values)
            {

                // check if the snake is playing, by checking if there are positions set
                if (snake.pos != null && snake.pos.Any())
                {
                    count++;
                }
            }
            return count;
        }

        /// <summary>
        /// Check if there are more players online, then max player count.
        /// </summary>
        public void checkMaxPlayerCount()
        {
            // max 15 players
            while (getOnlinePlayers() >= Constants.MAX_PLAYERS)
            {
                MessageBox.Show("The Game is full (15/15). Click the button to retry.", "Error");
            }
        }

        public void chooseName()
        {
            this.name = "";

            while (this.name == "")
            {
                // prompt the player to enter a name
                DialogResult res = InputBox.ShowDialog("Choose a player name:\n\n\".\", \"/\", \"#\", \"$\", \"[\", or \"]\" will be replaced by \"_\".", "Name", InputBox.Icon.Question, InputBox.Buttons.Ok, InputBox.Type.TextBox);
                if (res == DialogResult.None)
                {
                    Application.Exit();
                }
                string tempName = InputBox.ResultValue;
                this.name = tempName == null ? "" : tempName;

                // replace unwanted symbols with underscore
                this.name = this.name.Replace(".", "_").Replace("#", "_").Replace("$", "_").Replace("[", "_").Replace("]", "_").Replace("/", "_");

                if (this.name.Length > 16)
                {
                    MessageBox.Show("This name is too long.", "Error");
                    this.name = "";
                    continue;
                }

                // the color value is set, so there must be a user, that has taken this name
                if (this.firebase.queryOnce<string>(Constants.FIREBASE_PLAYER_COLOR_KEY.Replace("%name%", this.name)) != null)
                {
                    MessageBox.Show("Someone has already chosen this name.", "Error");
                    this.name = "";
                }
            }

            // name set successfully, we can run the game now  and we need to save the name
            // in a separate key to get access to write our data later, this will also restrict
            // the access to this entry only for us
            this.firebase.put(Constants.FIREBASE_PLAYER_VERIFY_NAME_KEY.Replace("%name%", this.name), this.name);

            // if player disconnect (closes window), remove data
            Application.ApplicationExit += (sender, e) =>
            {
                this.firebase.delete(Constants.FIREBASE_PLAYER_KEY.Replace("%name%", this.name));
            };
        }

        /// <summary>
        /// Choose a random color for our snake, that isn't used.
        /// </summary>
        public void chooseRandomColor()
        {
            string[] unusedColors = getUnusedColors();

            this.color = unusedColors[Utils.RANDOM.Next(unusedColors.Length)];

            this.firebase.put(Constants.FIREBASE_PLAYER_COLOR_KEY.Replace("%name%", this.name), this.color);
        }

        /// <summary>
        /// Get all colors, which aren't used by any player.
        /// </summary>
        public string[] getUnusedColors()
        {
            string[] usedColors = new string[Constants.MAX_PLAYERS];
            string[] unusedColors = new string[Constants.MAX_PLAYERS];

            // getting all used colors
            int i = 0;
            foreach (PlayerData otherSnake in otherSnakes.Values)
            {
                if (otherSnake == null || otherSnake.color == null)
                {
                    continue;
                }
                usedColors[i++] = otherSnake.color;
                i++;
            }

            // looping threw all available colors, then checking if the color
            // is in the list of used colors, if it isn't, the color is unused
            i = 0;
            foreach (string colorName in Constants.COLORS)
            {
                if (!usedColors.Contains(colorName))
                {
                    unusedColors[i] = colorName;
                    i++;
                }
            }

            // now we have a list of unused colors, which we can return
            return unusedColors;
        }
    }
}
