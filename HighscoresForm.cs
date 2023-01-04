﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultiplayerSnake
{
    public partial class HighscoresForm : Form
    {
        public HighscoresForm()
        {
            InitializeComponent();
            Resize += HighscoresForm_Resize;
            this.HighscoresForm_Resize(null, null);
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
            g.FillRectangle(b, 0, yposition, calculateBar(score), 30);

            //writing
            if (calculateBar(score) < 10 + p_name.Length * 5)
            {
                g.DrawString(p_name, new Font("Arial", 12), Brushes.Black, 10, yposition + 5);
            }
            else
            {
                g.DrawString(p_name, new Font("Arial", 12), Brushes.White, 10, yposition + 5);
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

        private void timer_Tick(object sender, EventArgs e)
        {
        }

        private void HighscoresForm_Load(object sender, EventArgs e)
        {

        }

        private void pbHighscores_Paint(object sender, PaintEventArgs e)
        {
            int score = 100;
            string color = "red";
            e.Graphics.Clear(SystemColors.Control);
            for (int i = 0; i < 10; i++)
            {
                drawBarChart(e.Graphics, score, i * 50 + 20, Name, new SolidBrush(Color.FromName(color)));
            }
        }
    }
}