using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultiplayerSnake.utils
{
    public class KeyMessageFilter : IMessageFilter
    {
        public MainForm mainForm;
        private const int WM_KEYDOWN = 0x0100;

        public KeyMessageFilter(MainForm mainForm)
        {
            this.mainForm = mainForm;
        }

        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == WM_KEYDOWN)
            {
                this.mainForm.onKeyDown((Keys)m.WParam);
            }

            return false;
        }
    }
}
