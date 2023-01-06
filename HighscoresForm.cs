using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MultiplayerSnake
{
    public partial class HighscoresForm : Form
    {
        private Dictionary<int, string> scores;

        public HighscoresForm(Dictionary<int, string> scores)
        {
            InitializeComponent();
            Resize += HighscoresForm_Resize;
            this.HighscoresForm_Resize(null, null);
            this.scores = scores;  
        }

        private void HighscoresForm_Resize(object sender, EventArgs e)
        {
            pnScroll.Width = this.Width - 16;
            pbHighscores.Width = pnScroll.Size.Width - 17;
            pnScroll.Height = this.Height - 114;

            pbHighscores.Invalidate();
        }


        // Methods for the BarGraph()
        public void drawBarChart(Graphics g, int score, int yposition, string p_name, Brush b)
        {
            g.DrawString(p_name, new Font("Arial", 12), Brushes.Black, 10, yposition);                                              // displays the name
            g.FillRectangle(b, 0, yposition+20, calculateBar(score), 5);                                                            // Bargraph
            if (calculateBar(score) > 100)
            {
                g.DrawString(score.ToString(), new Font("Arial", 12), Brushes.DarkGray, calculateBar(score) + 10, yposition + 12);      // displays the score
            }
            else
            {
                g.DrawString(score.ToString(), new Font("Arial", 12), Brushes.DarkGray, 110, yposition + 12);
            }

        }

        int firstScore = 100; // max. Score

        public int calculateBar(int score)
        {
            return score * (Width-100) / firstScore;   // Score * max. length / max. score
        }


        private void btBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pbHighscores_Paint(object sender, PaintEventArgs e)
        {   
            firstScore = scores.ElementAt(0).Key;
            e.Graphics.Clear(SystemColors.Control);

            // Calls the function drawBarChart()
            int i = 0;
            foreach (KeyValuePair<int, string> score in scores)
            {
                int scorePlayer = score.Key;
                string namePlayer = score.Value;
                drawBarChart(e.Graphics, scorePlayer, i * 50 + 20, namePlayer, new SolidBrush(Color.FromName(colors[Math.Min(i, 3)])));
                i++;
            }
        }
        private readonly string[] colors = new string[] { "gold", "silver", "chocolate", "black" };
    }
}
