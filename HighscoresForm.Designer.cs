namespace MultiplayerSnake
{
    partial class HighscoresForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pbHighscores = new System.Windows.Forms.PictureBox();
            this.btClose = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.pnScroll = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pbHighscores)).BeginInit();
            this.pnScroll.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbHighscores
            // 
            this.pbHighscores.Location = new System.Drawing.Point(0, 0);
            this.pbHighscores.Name = "pbHighscores";
            this.pbHighscores.Size = new System.Drawing.Size(514, 100);
            this.pbHighscores.TabIndex = 0;
            this.pbHighscores.TabStop = false;
            this.pbHighscores.Paint += new System.Windows.Forms.PaintEventHandler(this.pbHighscores_Paint);
            // 
            // btClose
            // 
            this.btClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btClose.Location = new System.Drawing.Point(447, 318);
            this.btClose.Name = "btClose";
            this.btClose.Size = new System.Drawing.Size(75, 23);
            this.btClose.TabIndex = 1;
            this.btClose.Text = "Close";
            this.btClose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btClose.UseVisualStyleBackColor = true;
            this.btClose.Click += new System.EventHandler(this.btClose_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 31);
            this.label1.TabIndex = 2;
            this.label1.Text = "Top 10";
            // 
            // pnScroll
            // 
            this.pnScroll.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnScroll.AutoScroll = true;
            this.pnScroll.BackColor = System.Drawing.SystemColors.Control;
            this.pnScroll.Controls.Add(this.pbHighscores);
            this.pnScroll.Location = new System.Drawing.Point(0, 34);
            this.pnScroll.Name = "pnScroll";
            this.pnScroll.Size = new System.Drawing.Size(534, 278);
            this.pnScroll.TabIndex = 3;
            // 
            // HighscoresForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(534, 353);
            this.Controls.Add(this.pnScroll);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btClose);
            this.MinimumSize = new System.Drawing.Size(550, 392);
            this.Name = "HighscoresForm";
            this.Text = "Highscores";
            ((System.ComponentModel.ISupportInitialize)(this.pbHighscores)).EndInit();
            this.pnScroll.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbHighscores;
        private System.Windows.Forms.Button btClose;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnScroll;
    }
}