using MultiplayerSnake.Database;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using MultiplayerSnake.game;
using System.Threading.Tasks;
using MultiplayerSnake.utils;
using MultiplayerSnake.database.data;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Collections.Concurrent;

namespace MultiplayerSnake
{
    public partial class MainForm : Form
    {
        // class to control going in fullscreen with F11
        private FullScreen fullScreen;

        // is this the first time, the main form was initialized?
        private bool firstInit = false;

        // did we request an update of the picture box?
        private bool updateRequested = false;

        // countdown before start
        private int countdown = 6;

        // the time of the last graphics update
        private long timeLastGraphicsUpdate = -1;

        // is the game already ended?
        private bool isGameEnded = false;

        // the scaling of the picturebox graphics object
        private float scalingX = 1;
        private float scalingY = 1;

        // message filter for key down event
        private KeyMessageFilter m_filter;

        // class containing everything about food
        public FoodManager foodManager { get; set; }

        // class containing everything about players
        public PlayerManager playerManager { get; set; }

        // the database
        public Firebase firebase { get; set; }

        // all highscores from db
        public ConcurrentDictionary<string, int> highscores { get; set; }

        public MainForm()
        {
            InitializeComponent();
            // allow to go in fullscreen with f11
            KeyPreview = true;
            this.fullScreen = new FullScreen(this);
            this.m_filter = new KeyMessageFilter(this);
            Application.AddMessageFilter(this.m_filter);

            // on first run, make sure that all sizes are correct
            this.resizeSnakeboard(this);

            // tell windows, that we need high priority
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.RealTime;
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            Control control = (Control)sender;
            this.resizeSnakeboard(control);
        }

        // resize the snakeboard, based on the size of the window
        private void resizeSnakeboard(Control control)
        {
            // the calculated width and height of the canvas (based on screen size)
            int snakeboardCalculatedWidth, snakeboardCalculatedHeight;

            // calculate the max width and height
            int width50Percent = control.Size.Width - 20 - 280;
            int height75Percent = 3 * control.Size.Height / 4;

            // calculate aspect ratio
            double aspectRatio = (double)Constants.SNAKEBOARD_MAX_X / Constants.SNAKEBOARD_MAX_Y;

            // only allow the smallest width, to be full size
            double widthMax = (double)Constants.SNAKEBOARD_MAX_X / width50Percent;
            double heightMax = (double)Constants.SNAKEBOARD_MAX_Y / height75Percent;

            // the other size (which is too big to fit on the monitor) will shrink,
            // but it will also consider the aspect ratio
            if (widthMax > heightMax)
            {
                snakeboardCalculatedWidth = width50Percent;
                snakeboardCalculatedHeight = (int)Math.Round(width50Percent / aspectRatio);
            }
            else if (heightMax > widthMax)
            {
                snakeboardCalculatedWidth = (int)Math.Round(height75Percent * aspectRatio);
                snakeboardCalculatedHeight = height75Percent;
            }
            else
            {
                snakeboardCalculatedWidth = width50Percent;
                snakeboardCalculatedHeight = height75Percent;
            }


            // set the calculated width and height
            pbGame.Width = snakeboardCalculatedWidth;
            pbGame.Height = snakeboardCalculatedHeight;

            // adjust location to be on the right, centered
            pbGame.Location = new Point(
                control.Size.Width - snakeboardCalculatedWidth - 20,
                (control.Size.Height - snakeboardCalculatedHeight) / 2
            );
            // set scale multiplier for x and y
            this.scalingX = (float)snakeboardCalculatedWidth / Constants.SNAKEBOARD_MAX_X;
            this.scalingY = (float)snakeboardCalculatedHeight / Constants.SNAKEBOARD_MAX_Y;
            pbGame.BackColor = Color.FromName(Constants.BOARD_BACKGROUND);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (this.firstInit)
                return;

            // create the food manager class
            this.foodManager = new FoodManager(this);

            // create the player manager class
            this.playerManager = new PlayerManager(this);

            // connect to database
            this.firebase = new Firebase(this);

            this.foodManager.init();
            this.playerManager.init();

            // register the firebase listeners. this also performs a version check
            this.firebase.registerFireBaseListeners();

            if (!this.firebase.isOffline())
            {
                // let the user decide, which name he wants to use
                if (!this.playerManager.chooseName())
                {
                    return;
                }

                // check if the game is already full
                this.playerManager.checkMaxPlayerCount();
            }

            // choose the color
            this.playerManager.chooseRandomColor();

            // now we need to check, if we have to add foods
            this.foodManager.checkFoodsAvailable();

            // set the last graphics update to now
            this.timeLastGraphicsUpdate = Utils.currentTimeMillis();

            // prepare for the game to start
            this.startGame();

            // then start the main loop
            _ = this.loop();

            this.firstInit = true;
        }

        /// <summary>
        /// reset everything and start the game
        /// </summary>
        public void startGame()
        {
            // reset the last graphics update time to now
            this.timeLastGraphicsUpdate = Utils.currentTimeMillis();
            // reset countdown
            this.countdown = 6;
            // reset snake pos to random position
            this.playerManager.isInvisibleForOthers = true;
            this.playerManager.snake = this.playerManager.generateRandomSnake();
            // reset game isn't ended
            this.isGameEnded = false;

            // start the countdown
            _ = this.startCountdown();
        }

        /// <summary>
        /// show countdown
        /// </summary>
        public async Task startCountdown()
        {
            lbStatus.ForeColor = Color.Black;

            while (true)
            {
                this.countdown--;
                if (this.countdown > 0 && this.countdown <= 3)
                {
                    // countdown visible
                    lbStatus.Text = this.countdown.ToString();
                    await Task.Delay(500);
                    continue;
                }
                else if (this.countdown > 0)
                {
                    // "Get ready!" visible
                    lbStatus.Text = "Get ready!";
                    await Task.Delay(500);
                    continue;
                }
                else
                {
                    // if there is a snake on our position, wait for it to go away
                    if (this.playerManager.checkForCollisionWithOtherSnakes())
                    {
                        this.countdown = -1;
                        lbStatus.Text = "Waiting for other snake to go away...";
                        await Task.Delay(500);
                        continue;
                    }

                    // reset go direction
                    this.playerManager.deltaX = 10;
                    this.playerManager.deltaY = 0;

                    // countdown is finished, we don't need to wait
                    this.countdown = 0;

                    this.playerManager.isInvisibleForOthers = false;

                    // "Go" visible for 3 secs
                    await this.sendInfo("Go!", 3000);
                    break;
                }
            }
        }

        /// <summary>
        /// the game/main loop.
        /// </summary>
        public async Task loop()
        {
            await Task.Delay(500);

            while (true)
            {
                await Task.Delay(1);

                this.updateRequested = true;
                // invalidating the picture box will trigger the event handler below which will
                // execute the tick methodwith a graphics object, that is applied immediately
                pbGame.Invalidate();
            }
        }

        private void pbGame_Paint(object sender, PaintEventArgs e)
        {
            // we can only update, if there was an update requested by us (and not the system)
            if (!this.updateRequested)
                return;

            this.updateRequested = false;

            // tick the game with the graphics object
            this.tick(e.Graphics);
        }

        /// <summary>
        /// tick the game
        /// </summary>
        /// <param name="g">the graphics object of the picturebox</param>
        public void tick(Graphics g)
        {
            // set the coordinate system size
            g.ScaleTransform(this.scalingX, this.scalingY);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // the current time
            long timeNow = Utils.currentTimeMillis();
            // the time estimated since last update
            long timeEstimated = Math.Max(0, timeNow - this.timeLastGraphicsUpdate);

            if (timeEstimated >= Constants.SNAKE_UPDATE_DELAY)
            {
                // the time, we were waiting too long
                var timeTooLong = timeEstimated - Constants.SNAKE_UPDATE_DELAY;

                // the count, how often we have to call the tick method, based on the time, we were waiting too long, at least one time
                int loopCount = Math.Max(1, (int)Math.Round((double)timeTooLong / Constants.SNAKE_UPDATE_DELAY));

                for (int i = 0; i < loopCount; i++)
                {
                    // tick the game
                    this.playerManager.changingDirection = false;
                    if (this.countdown != 0)
                    {
                        // wait for countdown finsih
                    }
                    else if (this.isGameEnded || this.playerManager.snake.Count <= 0 || this.playerManager.checkForCollision())
                    {
                        // game ended for us
                        // first call, then call onGameEnd()
                        if (!this.isGameEnded) this.onGameEnd();
                    }
                    else
                    {
                        // we are still in
                        // move the snake (change values in array, and handle key presses)
                        this.playerManager.moveSnake();
                    }
                }

                // send playerdata just once after the for loop, if snake is set
                if (this.playerManager.snake.Count != 0) this.firebase.setPlayerData(this.playerManager.snake);
                

                // set the last update time to now
                this.timeLastGraphicsUpdate = Utils.currentTimeMillis();
            }

            this.updateSideBar();

            // we always need to draw in the graphics, so if somethings is there, it will be shown
            this.playerManager.handleOtherSnakes(g);
            this.playerManager.drawSnake(g);
            this.foodManager.drawFoods(g);
        }

        /// <summary>
        /// called when the game ends
        /// </summary>
        public void onGameEnd(bool kicked=false)
        {
            this.foodManager.removedFoods.Clear();
            this.playerManager.lastScore = this.playerManager.snake.Count - 5;
            this.isGameEnded = true;
            // remove our snake from the board
            this.firebase.setPlayerData(new List<PlayerPositionData>());
            // drop random food from out snake
            this.foodManager.dropRandomFood();
            this.playerManager.snake.Clear();

            // show game over message
            lbStatus.ForeColor = Color.Red;
            lbStatus.Text = "Game Over!";

            // show retry, highscore button and last score
            btnRetry.Show();
            btnHighscores.Show();
            lbScore.Show();
            lbScore.Text = "Your Score: " + this.playerManager.lastScore;

            // start the highscore task a bit delayed, so the database can give us the current (correct) data
            // don't start at all, if we are offline
            if (!this.firebase.isOffline())
            {
                new Task(async () =>
                {
                    await Task.Delay(500);

                    if (this.playerManager.lastScore <= 0)
                    {
                        // don´t put data in database
                        return;
                    }

                    // this is needed, because some names may get interpreted as a number and not string by the database
                    string highscoreName = "<>" + this.playerManager.name;

                    // try to get the current highscores from db
                    //ConcurrentDictionary<string, int> highscores = this.firebase.queryOnce<ConcurrentDictionary<string, int>>(Constants.FIREBASE_HIGHSCORES_KEY);
                    // make sure, it is not null
                    this.highscores = this.highscores == null ? new ConcurrentDictionary<string, int>() : this.highscores;

                    // attempt to add our score
                    this.highscores.AddOrUpdate(highscoreName, this.playerManager.lastScore,
                        // if the score is bigger than last time, update it to the bigger one, else just leave it as is
                        (key, oldValue) => oldValue < this.playerManager.lastScore
                        ? this.playerManager.lastScore
                        : oldValue);

                    // sort the highscores by value
                    List<KeyValuePair<string, int>> orderedHighscores = this.highscores.OrderByDescending(keyValuePair => keyValuePair.Value).ToList();
                    if (orderedHighscores.Count() <= 10)
                    {
                        // if the highscore list is not full (<=10), just send the new list to db
                        this.firebase.put(Constants.FIREBASE_HIGHSCORES_KEY, this.highscores);
                    }
                    else if (orderedHighscores.ElementAt(this.highscores.Count - 1).Key != highscoreName)
                    {
                        // otherwise we need to add the highscores to the database
                        orderedHighscores.RemoveAt(this.highscores.Count - 1);
                        this.firebase.put(Constants.FIREBASE_HIGHSCORES_KEY, orderedHighscores.ToDictionary(z => z.Key, y => y.Value));
                    }
                }).Start();
            }
        }

        /// <summary>
        /// called when the retry button is clicked or when 'ENTER' or 'R' is pressed
        /// </summary>
        public void onRetry()
        {
            // if the player isn't alive, restart
            if (this.isGameEnded)
            {
                lbScore.Hide();
                btnRetry.Hide();
                btnHighscores.Hide();
                this.startGame();
            }
        }

        public void onKeyDown(Keys key)
        {
            this.playerManager.onKeyDown(key);
        }

        /// <summary>
        /// send an info to the status display
        /// </summary>
        /// <param name="str">the string to send</param>
        /// <param name="duration">the duration the string should be shown</param>
        /// <param name="color">the color of the string shown</param>
        /// <returns></returns>
        public async Task sendInfo(string str, int duration, string color = "Black")
        {
            // set text and color
            lbStatus.ForeColor = Color.FromName(color);
            lbStatus.Text = str;

            await Task.Delay(duration);

            // hide, if the innerHTML is still the given string and color
            if (lbStatus.Text.Equals(str) && lbStatus.ForeColor.Name.Equals(Color.FromName(color).Name))
            {
                lbStatus.Text = "";
            }
        }

        // update side status bar
        public void updateSideBar()
        {
            // if we don't have a database connection, we tell the player in the sidebar
            if (this.firebase.isOffline())
            {
                lbSidebar.Text = "<span style=\"color: red\">Offline Mode</span><br><br>"
                    + (this.playerManager.snake.Any()
                    ? ("Current score: " + (this.playerManager.snake.Count - 5))
                    : "");
                return;
            }

            Dictionary<string, int> scores = new Dictionary<string, int>();
            string formattedScore = "";

            int i = 0;
            // put all the scores and the player name in array
            foreach (string playerName in this.playerManager.allSnakes.Keys)
            {
                this.playerManager.allSnakes.TryGetValue(playerName, out PlayerData playerData);
                if (playerData == null)
                {
                    continue;
                }

                int score = playerData.pos == null ? -5 : playerData.pos.Count() - 5;

                scores.Add(playerName, score);
                i++;
            }
            // loop threw the descending sorted array
            foreach (KeyValuePair<string, int> scoreData in scores.OrderByDescending(keyValuePair => keyValuePair.Value))
            {

                // and getting player name, score and the snake data
                int score = scoreData.Value;
                string playerName = scoreData.Key;
                PlayerData snakeData = this.playerManager.allSnakes[playerName];

                // add the data as html to the score string
                formattedScore += "<div style=\"position: absolute;margin-top: 7px;margin-left: 10px\"><b>" + Utils.htmlEntities(playerName) + "</b>: " + (score < -4 ? "Spectator" : score.ToString()) + "</div><div style=\"position: relative\"><div style=\"font-size: 20px;position: absolute;margin-top: 3.5px;margin-left: 1px;color: " + snakeData.color + ";\">&#x25CF;</div><div style=\"font-size: 26.5px;color: Black;\">&#x25CF;</div></div>";
            }

            lbSidebar.Text = "Online: " + this.playerManager.getOnlinePlayers() + "/15<br><br>" + formattedScore;
        } 

        private void btnRetry_Click(object sender, EventArgs e)
        {
            this.onRetry();
        }

        private void btnHighscores_Click(object sender, EventArgs e)
        {
            Dictionary<string, int> highscores = new Dictionary<string, int>(
                this.highscores == null ? new ConcurrentDictionary<string, int>() : this.highscores
            );

            if (this.playerManager.lastScore > 0 && !this.firebase.isOffline())
            {
                highscores.Add("[Your Score]", this.playerManager.lastScore);
            }
            highscores = highscores.OrderByDescending(keyValuePair => keyValuePair.Value).ToDictionary(z => z.Key, y => y.Value);

            HighscoresForm highscoresForm = new HighscoresForm(highscores);
            highscoresForm.Show();
        }
    }
}
