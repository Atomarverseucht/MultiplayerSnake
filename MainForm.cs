using MultiplayerSnake.Database;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace MultiplayerSnake
{
    public partial class MainForm : Form
    {
        // class to controll going in fullscreen with F11
        private FullScreen fullScreen;

        // the database
        private Firebase firebase;

        private string name;

        public MainForm()
        {
            InitializeComponent();
            // allow to go in fullscreen with f11
            fullScreen = new FullScreen(this);
            KeyPreview = true;

            this.resizeSnakeboard(this);
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
            int width50Percent = control.Size.Width - 300;
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
            //pbGame.Scale((float)snakeboardCalculatedWidth / snakeboardMaxX, (float)snakeboardCalculatedHeight / snakeboardMaxY);
            pbGame.BackColor = Color.Black;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // connect to database
            this.firebase = new Firebase();

            // check database version
            if (!this.firebase.checkVersion())
            {
                Application.Exit();
            }

            //this.firebase.registerFireBaseListeners();

            DialogResult res = DialogResult.None;
            while (res == DialogResult.None)
            {
                res = InputBox.ShowDialog("Type name", "Name", InputBox.Icon.Question, InputBox.Buttons.Ok, InputBox.Type.TextBox);
                name = InputBox.ResultValue;

            }
            return;
        }

        public void drawBarChart(Graphics g)
        {
            SolidBrush blue = new SolidBrush(Color.Blue);
            int score = 100;
            int score2 = 50;
            g.FillRectangle(blue, 0,50,calculateBar(score),30);
            g.DrawString(name,new Font("Arial",12),Brushes.LightGray,10,55);
            g.FillRectangle(Brushes.Red, 0, 100, calculateBar(score2), 30);


        }

        int firstScore;
        public int calculateBar(int score)
        {
            if (firstScore < score)
            {
                firstScore = score;
            }
            
            return score * 200/firstScore;
        }

       

        private void LbSidebar_Paint(object sender, PaintEventArgs e)
        {
            drawBarChart(panel1.CreateGraphics());
        }
    }
}
