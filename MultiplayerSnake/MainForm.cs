using System;
using System.Drawing;
using System.Windows.Forms;

namespace MultiplayerSnake
{
    public partial class MainForm : Form
    {
        FullScreen fullScreen;

        // set the coordinate system dimensions (always 16:9)
        private const int snakeboardMaxX = 1280;
        private const int snakeboardMaxY = 720;

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
            double aspectRatio = (double)snakeboardMaxX / snakeboardMaxY;

            // only allow the smallest width, to be full size
            double widthMax = (double)snakeboardMaxX / width50Percent;
            double heightMax = (double)snakeboardMaxY / height75Percent;

            // the other size (which is too big to fit on the monitor) will shrink,
            // but it will also consider the aspect ratio
            if (widthMax > heightMax)
            {
                snakeboardCalculatedWidth = width50Percent;
                snakeboardCalculatedHeight = (int) Math.Round(width50Percent / aspectRatio);
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
    }
}
