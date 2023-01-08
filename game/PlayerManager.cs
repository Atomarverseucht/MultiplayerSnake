using MultiplayerSnake.database.data;
using MultiplayerSnake.Database;
using MultiplayerSnake.utils;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;

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

        // can we send data to database (we should be invisible for other players
        // in countdown sequence, but visible for ourself)
        public bool isInvisibleForOthers = true;

        // our snake
        public List<PlayerPositionData> snake = new List<PlayerPositionData>();

        // horizontal snake move delta
        public int deltaX = 10;

        // vertical snake move delta
        public int deltaY = 0;

        // are we currently changing direction?
        public bool changingDirection = false;

        // the last score we had, to display it after death in title
        public int lastScore = 0;

        // how long should the snake tail stay in place, after a food was eaten?
        int tailWaitCount = 0;

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
                DialogResult res = MessageBox.Show("The Game is full (15/15). Click the button to retry.", "Error", MessageBoxButtons.OKCancel);
                if (res == DialogResult.Cancel)
                {
                    Application.Exit();
                    return;
                }
            }
        }

        /// <summary>
        /// prompts the user to choose its name
        /// </summary>
        /// <returns>if the name prompt was success (not closed by the user)</returns>
        public bool chooseName()
        {
            this.name = "";

            while (this.name == "")
            {
                // prompt the player to enter a name
                DialogResult res = InputBox.ShowDialog("Choose a player name:\n\n\".\", \"/\", \"#\", \"$\", \"[\", \"]\", \"<\" or \">\" will be replaced by \"_\".", "Name", InputBox.Icon.Question, InputBox.Buttons.Ok, InputBox.Type.TextBox);
                if (res == DialogResult.None)
                {
                    Application.Exit();
                    return false;
                }
                string tempName = InputBox.ResultValue;
                this.name = tempName == null ? "" : tempName;

                // replace unwanted symbols with underscore
                this.name = this.name.Replace(".", "_").Replace("#", "_").Replace("$", "_").Replace("[", "_").Replace("]", "_").Replace("/", "_").Replace("<", "_").Replace(">", "_");

                if (this.name.Length > 16)
                {
                    MessageBox.Show("This name is too long.", "Error");
                    this.name = "";
                    continue;
                }

                // the color value is set, so there must be a user, that has taken this name
                if (/*this.firebase.queryOnce<string>(Constants.FIREBASE_PLAYER_COLOR_KEY.Replace("%name%", this.name)) != null*/ this.allSnakes.TryGetValue(this.name, out var ignored))
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

            return true;
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
                usedColors[i] = otherSnake.color;
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

        /// <summary>
        /// generate a random snake array which is 5 long (only start point is random, other 4 are relative to start point)
        /// </summary>
        /// <returns></returns>
        public List<PlayerPositionData> generateRandomSnake()
        {
            // getting random coordinates, in x direction with a distance of min 4 gaps to the wall
            var randomX = Utils.randomCoordinateX(4);
            var randomY = Utils.randomCoordinateY();
            // the coordinates for the snake, we will calculate
            List<PlayerPositionData> snakeCoords = new List<PlayerPositionData>();

            // just fill the array with 5 coordinate pairs, the x is descending, the higher the key is
            for (int i = 0; i < 5; i++)
            {
                int currentPart = i * 10;
                snakeCoords.Add(new PlayerPositionData(randomX - currentPart, randomY));
            }

            return snakeCoords;
        }

        /// <summary>
        /// check if the player collides with any other player
        /// </summary>
        public bool checkForCollisionWithOtherSnakes()
        {
            foreach (PlayerData playerData in this.otherSnakes.Values)
            {
                List<PlayerPositionData> otherSnake = playerData.pos;

                if (otherSnake == null) continue;

                // if head is at same position as any part of the other snake, then the player collided
                foreach (PlayerPositionData positionData in otherSnake)
                {
                    if (positionData.x == this.snake.ElementAt(0).x && positionData.y == snake.ElementAt(0).y)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// draws the movement of other snakes to the graphics
        /// </summary>
        public void handleOtherSnakes(Graphics g)
        {
            foreach (var otherSnake in this.otherSnakes.Values)
            {
                if (otherSnake == null || otherSnake.pos == null)
                {
                    continue;
                }

                // update each parts
                foreach (PlayerPositionData part in otherSnake.pos)
                {
                    this.drawSnakePart(g, string.IsNullOrWhiteSpace(otherSnake.color) ? "Red" : otherSnake.color, part);
                }
            }
        }

        /// <summary>
        /// draw one snake part
        /// </summary>
        /// <param name="snakeColor">the color of the snake</param>
        /// <param name="part">the part of the snake to draw</param>
        /// <exception cref="NotImplementedException"></exception>
        private void drawSnakePart(Graphics g, string snakeColor, PlayerPositionData part)
        {
            Rectangle rect = new Rectangle(part.x, part.y, 10, 10);

            // draw a "filled" rectangle to represent the snake part at its coordinates
            g.FillRectangle(new SolidBrush(Color.FromName(snakeColor)), rect);
            // draw a border around the snake part
            g.DrawRectangle(Pens.Black, rect);
        }

        /// <summary>
        /// check for collission with wall, other snakes, oneself
        /// </summary>
        public bool checkForCollision()
        {
            // check for collsison with oneself
            for (var i = 4; i < this.snake.Count; i++)
            {
                if (this.snake.ElementAt(i).x == this.snake.ElementAt(0).x && this.snake.ElementAt(i).y == this.snake.ElementAt(0).y)
                {
                    return true;
                }
            }
            // check for collisions with other players
            if (checkForCollisionWithOtherSnakes())
            {
                return true;
            }

            // check for collission with wall
            bool hitLeftWall = this.snake.ElementAt(0).x < 0;
            bool hitRightWall = this.snake.ElementAt(0).x > Constants.SNAKEBOARD_MAX_X - 10;
            bool hitTopWall = this.snake.ElementAt(0).y < 0;
            bool hitBottomWall = this.snake.ElementAt(0).y > Constants.SNAKEBOARD_MAX_Y - 10;
            return hitLeftWall || hitRightWall || hitTopWall || hitBottomWall;
        }

        /// <summary>
        /// Draw the snake on the picture box
        /// </summary>
        /// <param name="g">the graphics object of the picture box</param>
        public void drawSnake(Graphics g)
        {
            foreach (PlayerPositionData part in this.snake)
            {
                this.drawSnakePart(g, string.IsNullOrWhiteSpace(this.color) ? "Red" : this.color, part);
            }
        }

        /// <summary>
        /// move the snake
        /// </summary>
        public void moveSnake()
        {
            // create the new Snake's head, based on snake move delta
            PlayerPositionData head = new PlayerPositionData(this.snake.ElementAt(0).x + deltaX, this.snake.ElementAt(0).y + deltaY);

            // add the new head to the beginning of snake body
            this.snake.Insert(0, head);

            // did we ate food?
            bool ateFood = this.foodManager.hasEatenFood();

            if (ateFood)
            {
                // handle collection of food and get the count, how many parts must be added
                int count = Utils.foodLevelToCount(this.foodManager.handleFoodCollect()) - 1;

                if (count < 0)
                {
                    // we need to remove parts
                    for (var i = 0; i > count; i--)
                    {
                        // but if the snake has just a length of 1, we don't remove more
                        if (this.snake.Count == 1) break;
                        snake.RemoveAt(this.snake.Count - 1);
                    }
                }
                else
                {
                    // we need to add parts, so add the count to our wait count so the end will stop
                    // and wait until the var is zero again
                    this.tailWaitCount += count;
                }

                count++;

                // show the player, how many points he got/lost
                if (count > 0)
                {
                    _ = this.mainForm.sendInfo("+" + count, 1000, "LimeGreen");
                }
                else if (count == 0)
                {
                    _ = this.mainForm.sendInfo("0", 1000);
                }
                else if (count < 0)
                {
                    _ = this.mainForm.sendInfo(count.ToString(), 1000, "Red");
                }
            }
            else if (this.tailWaitCount <= 0)
            {
                // remove the last part of snake body
                snake.RemoveAt(this.snake.Count - 1);
            }
            else
            {
                this.tailWaitCount--;
            }
        }

        public void onKeyDown(Keys key)
        {
            // prevent the snake from reversing
            if (this.changingDirection)
            {
                return;
            }

            this.changingDirection = true;

            bool goingUp = deltaY == -10;
            bool goingDown = deltaY == 10;
            bool goingRight = deltaX == 10;
            bool goingLeft = deltaX == -10;

            // change direction based on pressed key
            this.changeDirection(((key == Keys.Up) || (key == Keys.W)) && !goingDown,
                ((key == Keys.Left) || (key == Keys.A)) && !goingRight,
                ((key == Keys.Down) || (key == Keys.S)) && !goingUp,
                ((key == Keys.Right) || (key == Keys.D)) && !goingLeft);

            // retry
            if (key == Keys.Enter || key == Keys.R)
            {
                this.mainForm.onRetry();
            }
        }

        /// <summary>
        /// change the "walking" direction of the snake
        /// </summary>
        /// <param name="up">the snake should go up</param>
        /// <param name="left">the snake should go left</param>
        /// <param name="down">the snake should go down</param>
        /// <param name="right">the snake should go right</param>
        private void changeDirection(bool up, bool left, bool down, bool right)
        {
            // going up
            if (up)
            {
                deltaX = 0;
                deltaY = -10;
            }
            // going left
            else if (left)
            {
                deltaX = -10;
                deltaY = 0;
            }
            // going down
            else if (down)
            {
                deltaX = 0;
                deltaY = 10;
            }
            // going right
            else if (right)
            {
                deltaX = 10;
                deltaY = 0;
            }
        }
    }
}
