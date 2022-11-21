namespace MultiplayerSnake
{
    partial class MainForm
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnSpielfeld = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // pnSpielfeld
            // 
            this.pnSpielfeld.Location = new System.Drawing.Point(0, 0);
            this.pnSpielfeld.Name = "pnSpielfeld";
            this.pnSpielfeld.Size = new System.Drawing.Size(775, 403);
            this.pnSpielfeld.TabIndex = 0;
            // 
            // MultiplayerSnake
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pnSpielfeld);
            this.Name = "MultiplayerSnake";
            this.Text = "Multiplayer Snake";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnSpielfeld;
    }
}

