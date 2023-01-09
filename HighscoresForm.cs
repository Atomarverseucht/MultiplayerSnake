using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MultiplayerSnake
{
    public partial class HighscoresForm : Form
    {
        private MainForm mainForm;

        // all the highscores to render
        private Dictionary<string, int> scores;

        // the colors of the bars in the highscore form. 1st place to 4th place. every other place gets the same color as 4th place
        private readonly string[] colors = new string[] { "gold", "silver", "chocolate", "black" };

        // max. Score
        private int firstScore = 100;

        // this timer updates the highscores at regular intervals
        private Timer timer;

        public HighscoresForm(MainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;

            this.updateHighscores();

            // add the resize listener
            Resize += HighscoresForm_Resize;
            this.HighscoresForm_Resize(null, null);
            FormClosed += HighscoresForm_FormClosed;

            // start the timer every second
            this.timer = new Timer();
            this.timer.Tick += Timer_Tick;
            this.timer.Interval = 1000;
            this.timer.Start();
        }

        /// <summary>
        /// Updates all highscores with the current ones from db
        /// </summary>
        private void updateHighscores()
        {
            // get the current highscores
            Dictionary<string, int> highscores = new Dictionary<string, int>(
                this.mainForm.highscores == null ? new ConcurrentDictionary<string, int>() : this.mainForm.highscores
            );

            // only add our score, if it is not null and if it is an online game
            if (this.mainForm.playerManager.lastScore > 0 && !this.mainForm.firebase.isOffline())
            {
                highscores.Add("[Your Score]", this.mainForm.playerManager.lastScore);
            }

            // sort the scores correctly
            this.scores = highscores.OrderByDescending(keyValuePair => keyValuePair.Value).ToDictionary(z => z.Key, y => y.Value);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            this.updateHighscores();
            pbHighscores.Invalidate();
        }

        private void HighscoresForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // stop the update timer
            this.timer.Stop();
        }

        private void HighscoresForm_Resize(object sender, EventArgs e)
        {
            // calculate the correct size for the scroll panal and picturebox
            pnScroll.Width = this.Width - 16;
            pbHighscores.Width = pnScroll.Size.Width - 17;
            pnScroll.Height = this.Height - 114;
            pbHighscores.Height = this.calculatePbHeight();

            pbHighscores.Invalidate();
        }

        /// <summary>
        /// Calculates the hight of the picturebox
        /// </summary>
        /// <returns></returns>
        private int calculatePbHeight()
        {
            if (!this.scores.Any())
            {
                return pnScroll.Height;
            }

            return 2 * 20 + (this.scores.Count - 1) * 50 + 12;
        }

        public void drawBarChart(Graphics g, int score, int yposition, string p_name, Brush b)
        {
            Font drawFont = new Font("Arial", 12);
            Font drawFontBold = new Font("Arial", 12, FontStyle.Bold);
            float widthFirst = g.MeasureString(p_name, drawFontBold).Width;
            // displays the name in bold and score in normal
            g.DrawString(p_name + ": ", drawFontBold, Brushes.Black, 10, yposition);
            g.DrawString(score.ToString(), drawFont, Brushes.Black, 20 + widthFirst, yposition);
            // draw the bar
            g.FillRectangle(b, 8, yposition+20, calculateBar(score), 5);
        }

        public int calculateBar(int score)
        {
            // score * max. length / max. score
            return score * (pbHighscores.Width - 8) / firstScore;
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pbHighscores_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(SystemColors.Control);
            // if there are no scores, tell the player by writing it on the center of the picturebox
            if (!this.scores.Any())
            {
                StringFormat sf = new StringFormat();
                sf.LineAlignment = StringAlignment.Center;
                sf.Alignment = StringAlignment.Center;
                e.Graphics.DrawString("No Highscores found.", new Font("Arial", 12), Brushes.Red, e.ClipRectangle, sf);
                return;
            }

            this.firstScore = this.scores.ElementAt(0).Value;
            // Calls the function drawBarChart()
            int i = 0;
            int color = -1;
            int playerScoreBefore = 0;
            foreach (KeyValuePair<string, int> score in this.scores)
            {
                // this replace is needed, because some names may get interpreted as a number and not string by the database
                string playerName = score.Key.Replace("<>", "");
                int playerScore = score.Value;

                // this makes sure, if there are multiple players per place, every player will be in the same color at this place
                if (playerScoreBefore != playerScore)
                {
                    color++;
                }
                playerScoreBefore = playerScore;

                // add the specific highscore + player name
                drawBarChart(e.Graphics, playerScore, i * 50 + 20, playerName, new SolidBrush(Color.FromName(colors[Math.Min(color, 3)])));

                i++;
            }
        }
    }
}
