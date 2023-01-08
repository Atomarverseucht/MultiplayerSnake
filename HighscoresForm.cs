using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MultiplayerSnake
{
    public partial class HighscoresForm : Form
    {
        private Dictionary<string, int> scores;

        private readonly string[] colors = new string[] { "gold", "silver", "chocolate", "black" };

        // max. Score
        int firstScore = 100;

        public HighscoresForm(Dictionary<string, int> scores)
        {
            this.scores = scores;
            InitializeComponent();
            Resize += HighscoresForm_Resize;
            this.HighscoresForm_Resize(null, null);
        }
    

        private void HighscoresForm_Resize(object sender, EventArgs e)
        {
            pnScroll.Width = this.Width - 16;
            pbHighscores.Width = pnScroll.Size.Width - 17;
            pnScroll.Height = this.Height - 114;
            pbHighscores.Height = this.calculatePbHeight();

            pbHighscores.Invalidate();
        }


        public void drawBarChart(Graphics g, int score, int yposition, string p_name, Brush b)
        {
            Font drawFont = new Font("Arial", 12);
            Font drawFontBold = new Font("Arial", 12, FontStyle.Bold);
            float widthFirst = g.MeasureString(p_name, drawFontBold).Width;
            // displays the name
            g.DrawString(p_name + ": ", drawFontBold, Brushes.Black, 10, yposition);
            g.DrawString(score.ToString(), drawFont, Brushes.Black, 20 + widthFirst, yposition);
            // Bargraph
            g.FillRectangle(b, 0, yposition+20, calculateBar(score), 5);
        }

        public int calculateBar(int score)
        {
            // Score * max. length / max. score
            return score * (Width-100) / firstScore;
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pbHighscores_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(SystemColors.Control);
            // if there are no scores, tell the player by writing it on the center of the PB
            if (!scores.Any())
            {
                StringFormat sf = new StringFormat();
                sf.LineAlignment = StringAlignment.Center;
                sf.Alignment = StringAlignment.Center;
                e.Graphics.DrawString("No Highscores found.", new Font("Arial", 12), Brushes.Red, e.ClipRectangle, sf);
                return;
            }

            firstScore = scores.ElementAt(0).Value;
            // Calls the function drawBarChart()
            int i = 0;
            int color = -1;
            int playerScoreBefore = 0;
            foreach (KeyValuePair<string, int> score in scores)
            {
                // this replace is needed, because some names may get interpreted as a number and not string by the database
                string playerName = score.Key.Replace("<>", "");
                int playerScore = score.Value;

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

        private int calculatePbHeight()
        {
            if (!scores.Any())
            {
                return pnScroll.Height;
            }

            return 2 * 20 + (this.scores.Count - 1) * 50 + 12;
        }
    }
}
