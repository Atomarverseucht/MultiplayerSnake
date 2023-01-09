using System.Windows.Forms;

namespace MultiplayerSnake
{
    class FullScreen
    {
        private Form mainForm;
        private FormWindowState previousWindowState;
        private bool fullScreenEnabled = false;

        public FullScreen(Form targetForm)
        {
            this.mainForm = targetForm;
            this.mainForm.KeyDown += this.TargetForm_KeyDown;
        }

        private void TargetForm_KeyDown(object sender, KeyEventArgs e)
        {
            // if F11 is pressed, then we enter the fullscreen mode
            if (e.KeyData == Keys.F11)
            {
                this.Toggle();
            }
        }

        /// <summary>
        /// This toggles the fullscreen mode.
        /// </summary>
        public void Toggle()
        {
            if (this.fullScreenEnabled)
            {
                this.Leave();
            }
            else
            {
                this.Enter();
            }
        }

        /// <summary>
        /// This enters the fullscreen mode, if the window was not in fullscreen
        /// </summary>
        public void Enter()
        {
            if (!this.fullScreenEnabled)
            {
                this.previousWindowState = this.mainForm.WindowState;
                this.mainForm.WindowState = FormWindowState.Normal;
                this.mainForm.FormBorderStyle = FormBorderStyle.None;
                this.mainForm.WindowState = FormWindowState.Maximized;
                this.fullScreenEnabled = true;
            }
        }

        /// <summary>
        /// This leaves the fullscreen mode, if the window was in fullscreen
        /// </summary>
        public void Leave()
        {
            if (this.mainForm.WindowState == FormWindowState.Maximized && this.fullScreenEnabled)
            {
                this.mainForm.FormBorderStyle = FormBorderStyle.Sizable;
                this.mainForm.WindowState = this.previousWindowState;
                this.fullScreenEnabled = false;
            }
        }
    }
}
