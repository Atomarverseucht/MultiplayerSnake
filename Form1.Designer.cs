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
            this.pbGame = new System.Windows.Forms.PictureBox();
            this.lbStatus = new System.Windows.Forms.Label();
            this.lbSidebar = new System.Windows.Forms.Label();
            this.btnRetry = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pbGame)).BeginInit();
            this.SuspendLayout();
            // 
            // pbGame
            // 
            this.pbGame.Location = new System.Drawing.Point(312, 113);
            this.pbGame.Name = "pbGame";
            this.pbGame.Size = new System.Drawing.Size(477, 269);
            this.pbGame.TabIndex = 1;
            this.pbGame.TabStop = false;
            // 
            // lbStatus
            // 
            this.lbStatus.AutoSize = true;
            this.lbStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 35F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbStatus.Location = new System.Drawing.Point(0, 0);
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.Size = new System.Drawing.Size(228, 54);
            this.lbStatus.TabIndex = 2;
            this.lbStatus.Text = "Loading...";
            // 
            // lbSidebar
            // 
            this.lbSidebar.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbSidebar.Location = new System.Drawing.Point(1, 54);
            this.lbSidebar.Name = "lbSidebar";
            this.lbSidebar.Padding = new System.Windows.Forms.Padding(0, 35, 0, 0);
            this.lbSidebar.Size = new System.Drawing.Size(282, 864);
            this.lbSidebar.TabIndex = 3;
            this.lbSidebar.Text = "Online: 1/15\r\n\r\ntest: 23";
            this.lbSidebar.Paint += new System.Windows.Forms.PaintEventHandler(this.LbSidebar_Paint);
            // 
            // btnRetry
            // 
            this.btnRetry.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRetry.Location = new System.Drawing.Point(12, 426);
            this.btnRetry.Name = "btnRetry";
            this.btnRetry.Size = new System.Drawing.Size(75, 23);
            this.btnRetry.TabIndex = 4;
            this.btnRetry.Text = "Retry";
            this.btnRetry.UseVisualStyleBackColor = true;
            this.btnRetry.Visible = false;
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(0, 54);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(228, 328);
            this.panel1.TabIndex = 5;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(834, 461);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnRetry);
            this.Controls.Add(this.lbSidebar);
            this.Controls.Add(this.pbGame);
            this.Controls.Add(this.lbStatus);
            this.MinimumSize = new System.Drawing.Size(850, 500);
            this.Name = "MainForm";
            this.Text = "Multiplayer Snake";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pbGame)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbGame;
        private System.Windows.Forms.Label lbStatus;
        private System.Windows.Forms.Label lbSidebar;
        private System.Windows.Forms.Button btnRetry;
        private System.Windows.Forms.Panel panel1;
    }
}

