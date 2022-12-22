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
            this.components = new System.ComponentModel.Container();
            this.pbGame = new System.Windows.Forms.PictureBox();
            this.lbStatus = new System.Windows.Forms.Label();
            this.btnRetry = new System.Windows.Forms.Button();
            this.pnSidebar = new System.Windows.Forms.Panel();
            this.tmUpdate = new System.Windows.Forms.Timer(this.components);
            this.lbUhr = new System.Windows.Forms.Label();
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
            // btnRetry
            // 
            this.btnRetry.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRetry.Location = new System.Drawing.Point(9, 426);
            this.btnRetry.Name = "btnRetry";
            this.btnRetry.Size = new System.Drawing.Size(75, 23);
            this.btnRetry.TabIndex = 4;
            this.btnRetry.Text = "Retry";
            this.btnRetry.UseVisualStyleBackColor = true;
            this.btnRetry.Visible = false;
            // 
            // pnSidebar
            // 
            this.pnSidebar.Location = new System.Drawing.Point(0, 57);
            this.pnSidebar.Name = "pnSidebar";
            this.pnSidebar.Size = new System.Drawing.Size(272, 363);
            this.pnSidebar.TabIndex = 5;
            // 
            // tmUpdate
            // 
            this.tmUpdate.Tick += new System.EventHandler(this.tmUpdate_Tick);
            // 
            // lbUhr
            // 
            this.lbUhr.AutoSize = true;
            this.lbUhr.Location = new System.Drawing.Point(711, 26);
            this.lbUhr.Name = "lbUhr";
            this.lbUhr.Size = new System.Drawing.Size(0, 13);
            this.lbUhr.TabIndex = 6;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(834, 461);
            this.Controls.Add(this.lbUhr);
            this.Controls.Add(this.pnSidebar);
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
        private System.Windows.Forms.Panel pnSidebar;
        private System.Windows.Forms.Timer tmUpdate;
        private System.Windows.Forms.Label lbUhr;
    }
}

