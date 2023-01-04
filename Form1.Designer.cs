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
            this.btnRetry = new System.Windows.Forms.Button();
            this.lbUhr = new System.Windows.Forms.Label();
            this.lbSidebar = new TheArtOfDev.HtmlRenderer.WinForms.HtmlLabel();
            this.lbScore = new System.Windows.Forms.Label();
            this.btnHighscores = new System.Windows.Forms.Button();
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
            this.pbGame.WaitOnLoad = true;
            this.pbGame.Paint += new System.Windows.Forms.PaintEventHandler(this.pbGame_Paint);
            // 
            // lbStatus
            // 
            this.lbStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 35F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbStatus.Location = new System.Drawing.Point(0, 0);
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.Size = new System.Drawing.Size(280, 54);
            this.lbStatus.TabIndex = 2;
            this.lbStatus.Text = "Loading...";
            this.lbStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnRetry
            // 
            this.btnRetry.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRetry.Location = new System.Drawing.Point(12, 426);
            this.btnRetry.Name = "btnRetry";
            this.btnRetry.Size = new System.Drawing.Size(75, 23);
            this.btnRetry.TabIndex = 4;
            this.btnRetry.Text = "Retry (r)";
            this.btnRetry.UseVisualStyleBackColor = true;
            this.btnRetry.Visible = false;
            this.btnRetry.Click += new System.EventHandler(this.btnRetry_Click);
            // 
            // lbUhr
            // 
            this.lbUhr.AutoSize = true;
            this.lbUhr.Location = new System.Drawing.Point(711, 26);
            this.lbUhr.Name = "lbUhr";
            this.lbUhr.Size = new System.Drawing.Size(0, 13);
            this.lbUhr.TabIndex = 6;
            // 
            // lbSidebar
            // 
            this.lbSidebar.AutoSize = false;
            this.lbSidebar.AutoSizeHeightOnly = true;
            this.lbSidebar.BackColor = System.Drawing.Color.Transparent;
            this.lbSidebar.BaseStylesheet = null;
            this.lbSidebar.Location = new System.Drawing.Point(12, 86);
            this.lbSidebar.Name = "lbSidebar";
            this.lbSidebar.Size = new System.Drawing.Size(268, 20);
            this.lbSidebar.TabIndex = 7;
            this.lbSidebar.TabStop = false;
            this.lbSidebar.Text = "Loading...";
            // 
            // lbScore
            // 
            this.lbScore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbScore.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbScore.Location = new System.Drawing.Point(93, 397);
            this.lbScore.Name = "lbScore";
            this.lbScore.Size = new System.Drawing.Size(187, 50);
            this.lbScore.TabIndex = 8;
            this.lbScore.Text = "own score";
            this.lbScore.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lbScore.Visible = false;
            // 
            // btnHighscores
            // 
            this.btnHighscores.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnHighscores.Location = new System.Drawing.Point(12, 397);
            this.btnHighscores.Name = "btnHighscores";
            this.btnHighscores.Size = new System.Drawing.Size(75, 23);
            this.btnHighscores.TabIndex = 9;
            this.btnHighscores.Text = "Highscores";
            this.btnHighscores.UseVisualStyleBackColor = true;
            this.btnHighscores.Visible = false;
            this.btnHighscores.Click += new System.EventHandler(this.btnHighscores_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(834, 461);
            this.Controls.Add(this.btnHighscores);
            this.Controls.Add(this.lbScore);
            this.Controls.Add(this.lbSidebar);
            this.Controls.Add(this.lbUhr);
            this.Controls.Add(this.btnRetry);
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
        private System.Windows.Forms.Button btnRetry;
        private System.Windows.Forms.Label lbUhr;
        private TheArtOfDev.HtmlRenderer.WinForms.HtmlLabel lbSidebar;
        private System.Windows.Forms.Label lbScore;
        private System.Windows.Forms.Button btnHighscores;
    }
}

