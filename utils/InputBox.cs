// 
// library from https://www.codeproject.com/Tips/822756/Input-Box-in-Csharp-for-WindowsForms
// license: The Code Project Open License (CPOL) 1.02 (https://www.codeproject.com/info/cpol10.aspx)
// author: Member 11100697
//

using System;
using System.Windows.Forms;
using System.Drawing;
using System.Reflection;

namespace MultiplayerSnake
{
    public static class InputBox
    {
        private static Form frm = new Form();
        public static string ResultValue;
        private static DialogResult DialogRes;
        private static string[] buttonTextArray = new string[4];
        public enum Icon
        {
            Error,
            Exclamation,
            Information,
            Question,
            Nothing
        }
        public enum Type
        {
            ComboBox,
            TextBox,
            Nothing
        }
        public enum Buttons
        {
            Ok,
            OkCancel,
            YesNo,
            YesNoCancel
        }
        public enum Language
        {
            Czech,
            English,
            German,
            Slovakian,
            Spanish
        }

        static InputBox() {
            SetLanguage(Language.English);
        }

        /// <summary>
        /// This form is like a MessageBox, but you can select type of controls on it. 
        /// This form returning a DialogResult value.
        /// </summary>
        /// <param name="Message">Message in dialog(as System.String)</param>
        /// <param name="Title">Title of dialog (as System.String)</param>
        /// <param name="icon">Select icon (as InputBox.Icon)</param>
        /// <param name="buttons">Select icon (as InputBox.Buttons)</param>
        /// <param name="type">Type of control in Input box (as InputBox.Type)</param>
        /// <param name="ListItems">Array of ComboBox items (as System.String[])</param>
        /// <param name="FormFont">Font in form (as System.Drawing.Font)</param>
        /// <returns></returns>
        /// 
        public static DialogResult ShowDialog(string Message, string Title = "",
        Icon icon = Icon.Information, Buttons buttons = Buttons.Ok, Type type = Type.Nothing,
        string[] ListItems = null, bool ShowInTaskBar = true, Font FormFont = null)
        {
            DialogRes = DialogResult.None;
            frm.Controls.Clear();
            ResultValue = "";
            //Form definition
            frm.MaximizeBox = false;
            frm.MinimizeBox = false;
            frm.FormBorderStyle = FormBorderStyle.FixedDialog;
            frm.Size = new Size(350, 170);
            frm.Text = Title;
            frm.ShowIcon = false;
            frm.ShowInTaskbar = ShowInTaskBar;
            frm.FormClosing += new FormClosingEventHandler(frm_FormClosing);
            frm.StartPosition = FormStartPosition.CenterParent;
            //Panel definition
            Panel panel = new Panel();
            panel.Location = new Point(0, 0);
            panel.Size = new Size(340, 97);
            panel.BackColor = Color.White;
            frm.Controls.Add(panel);
            //Add icon in to panel
            panel.Controls.Add(Picture(icon));
            //Label definition (message)
            Label label = new Label();
            label.Text = Message;
            label.Size = new Size(245, 60);
            label.Location = new Point(90, 10);
            label.TextAlign = ContentAlignment.MiddleLeft;
            panel.Controls.Add(label);
            //Add buttons to the form
            foreach (Button btn in Btns(buttons))
                frm.Controls.Add(btn);
            //Add ComboBox or TextBox to the form
            Control ctrl = Cntrl(type, ListItems);
            panel.Controls.Add(ctrl);
            //Get automatically cursor to the TextBox
            if (ctrl.Name == "textBox")
                frm.ActiveControl = ctrl;
            //Set label font
            if (FormFont != null)
                label.Font = FormFont;
            frm.ShowDialog();
            //Return text value
            switch (type)
            {
                case Type.Nothing:
                    break;
                default:
                    if (DialogRes == DialogResult.OK || DialogRes == DialogResult.Yes)
                    { ResultValue = ctrl.Text; }
                    else ResultValue = "";
                    break;
            }
            return DialogRes;
        }
        private static void button_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            switch (button.Name)
            {
                case "Yes":
                    DialogRes = DialogResult.Yes;
                    break;
                case "No":
                    DialogRes = DialogResult.No;
                    break;
                case "Cancel":
                    DialogRes = DialogResult.Cancel;
                    break;
                case "OK":
                    DialogRes = DialogResult.OK;
                    break;
                default:
                    DialogRes = DialogResult.None;
                    break;
            }
            frm.Close();
        }
        private static void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DialogRes = DialogResult.OK;
                frm.Close();
            }
        }
        private static void frm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogRes != null) { }
            else DialogRes = DialogResult.None;
        }
        private static PictureBox Picture(Icon icon)
        {
            PictureBox picture = new PictureBox();
            var assembly = Assembly.GetExecutingAssembly();     //Get integrated sources
            System.IO.Stream stream = null;
            //Set icon
            switch (icon)
            {
                case Icon.Error:
                    stream = assembly.GetManifestResourceStream(@"MultiplayerSnake.error.png");
                    break;
                case Icon.Exclamation:
                    stream = assembly.GetManifestResourceStream(@"MultiplayerSnake.exclamation.png");
                    break;
                case Icon.Information:
                    stream = assembly.GetManifestResourceStream(@"MultiplayerSnake.information.png");
                    break;
                case Icon.Question:
                    stream = assembly.GetManifestResourceStream(@"MultiplayerSnake.question.png");
                    break;
                case Icon.Nothing:
                    stream = assembly.GetManifestResourceStream(@"MultiplayerSnake.nic80x80.png");
                    break;
            }
            picture.Image = Image.FromStream(stream);
            picture.SizeMode = PictureBoxSizeMode.StretchImage;
            picture.Size = new Size(60, 60);
            picture.Location = new Point(10, 10);
            return picture;
        }
        private static Button[] Btns(Buttons button, Language lang = Language.English)
        {
            //Buttons field for return
            Button[] returnButtons = new Button[3];
            //Buttons instances
            Button OkButton = new Button();
            Button StornoButton = new Button();
            Button AnoButton = new Button();
            Button NeButton = new Button();
            //Set buttons names and text
            OkButton.Text = buttonTextArray[0];
            OkButton.Name = "OK";
            AnoButton.Text = buttonTextArray[1];
            AnoButton.Name = "Yes";
            NeButton.Text = buttonTextArray[2];
            NeButton.Name = "No";
            StornoButton.Text = buttonTextArray[3];
            StornoButton.Name = "Cancel";
            //Set buttons position
            switch (button)
            {
                case Buttons.Ok:
                    OkButton.Location = new Point(250, 101);
                    returnButtons[0] = OkButton;
                    break;
                case Buttons.OkCancel:
                    OkButton.Location = new Point(170, 101);
                    returnButtons[0] = OkButton;
                    StornoButton.Location = new Point(250, 101);
                    returnButtons[1] = StornoButton;
                    break;
                case Buttons.YesNo:
                    AnoButton.Location = new Point(170, 101);
                    returnButtons[0] = AnoButton;
                    NeButton.Location = new Point(250, 101);
                    returnButtons[1] = NeButton;
                    break;
                case Buttons.YesNoCancel:
                    AnoButton.Location = new Point(90, 101);
                    returnButtons[0] = AnoButton;
                    NeButton.Location = new Point(170, 101);
                    returnButtons[1] = NeButton;
                    StornoButton.Location = new Point(250, 101);
                    returnButtons[2] = StornoButton;
                    break;
            }
            //Set size and event for all used buttons
            foreach (Button btn in returnButtons)
            {
                if (btn != null)
                {
                    btn.Size = new Size(75, 23);
                    btn.Click += new EventHandler(button_Click);
                }
            }
            return returnButtons;
        }
        private static Control Cntrl(Type type, string[] ListItems)
        {
            //ComboBox
            ComboBox comboBox = new ComboBox();
            comboBox.Size = new Size(180, 22);
            comboBox.Location = new Point(90, 70);
            comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox.Name = "comboBox";
            if (ListItems != null)
            {
                foreach (string item in ListItems)
                    comboBox.Items.Add(item);
                comboBox.SelectedIndex = 0;
            }
            //Textbox
            TextBox textBox = new TextBox();
            textBox.Size = new Size(180, 23);
            textBox.Location = new Point(90, 70);
            textBox.KeyDown += new KeyEventHandler(textBox_KeyDown);
            textBox.Name = "textBox";
            //Set returned Control
            Control returnControl = new Control();
            switch (type)
            {
                case Type.ComboBox:
                    returnControl = comboBox;
                    break;
                case Type.TextBox:
                    returnControl = textBox;
                    break;
            }
            return returnControl;
        }
        public static void SetLanguage(Language lang)
        {
            switch (lang)
            {
                case Language.Czech:
                    buttonTextArray = "OK,Ano,Ne,Storno".Split(',');
                    break;
                case Language.German:
                    buttonTextArray = "OK,Ja,Nein,Stornieren".Split(',');
                    break;
                case Language.Spanish:
                    buttonTextArray = "OK,Sí,No,Cancelar".Split(',');
                    break;
                case Language.Slovakian:
                    buttonTextArray = "OK,Áno,Nie,Zrušit".Split(',');
                    break;
                default:
                    buttonTextArray = "OK,Yes,No,Cancel".Split(',');
                    break;
            }
        }
    }
}