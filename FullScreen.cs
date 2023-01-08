using System.Windows.Forms;

namespace MultiplayerSnake
{
    class FullScreen
    {
        Form TargetForm;

        FormWindowState PreviousWindowState;

        bool fullScreenEnabled = false;

        public FullScreen(Form targetForm)
        {
            TargetForm = targetForm;
            TargetForm.KeyDown += TargetForm_KeyDown;
        }

        private void TargetForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.F11)
            {
                Toggle();
            }
        }

        public void Toggle()
        {
            if (fullScreenEnabled)
            {
                Leave();
            }
            else
            {
                Enter();
            }
        }

        public void Enter()
        {
            if (!fullScreenEnabled)
            {
                PreviousWindowState = TargetForm.WindowState;
                TargetForm.WindowState = FormWindowState.Normal;
                TargetForm.FormBorderStyle = FormBorderStyle.None;
                TargetForm.WindowState = FormWindowState.Maximized;
                fullScreenEnabled = true;
            }
        }

        public void Leave()
        {
            if (TargetForm.WindowState == FormWindowState.Maximized && fullScreenEnabled)
            {
                TargetForm.FormBorderStyle = FormBorderStyle.Sizable;
                TargetForm.WindowState = PreviousWindowState;
                fullScreenEnabled = false;
            }
        }
    }
}
