﻿namespace MultiplayerSnake
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
            this.components = new System.ComponentModel.Container();
            this.pbHighscores = new System.Windows.Forms.PictureBox();
            this.btBack = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.pnScroll = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pbHighscores)).BeginInit();
            this.pnScroll.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbHighscores
            // 
            this.pbHighscores.Location = new System.Drawing.Point(0, 0);
            this.pbHighscores.Name = "pbHighscores";
            this.pbHighscores.Size = new System.Drawing.Size(368, 834);
            this.pbHighscores.TabIndex = 0;
            this.pbHighscores.TabStop = false;
            this.pbHighscores.Paint += new System.Windows.Forms.PaintEventHandler(this.pbHighscores_Paint);
            // 
            // btBack
            // 
            this.btBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btBack.Location = new System.Drawing.Point(12, 318);
            this.btBack.Name = "btBack";
            this.btBack.Size = new System.Drawing.Size(75, 23);
            this.btBack.TabIndex = 1;
            this.btBack.Text = "Back";
            this.btBack.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btBack.UseVisualStyleBackColor = true;
            this.btBack.Click += new System.EventHandler(this.btBack_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(161, 31);
            this.label1.TabIndex = 2;
            this.label1.Text = "Highscores\r\n";
            // 
            // timer
            // 
            this.timer.Enabled = true;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
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
            this.Controls.Add(this.btBack);
            this.MinimumSize = new System.Drawing.Size(550, 392);
            this.Name = "HighscoresForm";
            this.Text = "Highscores";
            this.Load += new System.EventHandler(this.HighscoresForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbHighscores)).EndInit();
            this.pnScroll.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbHighscores;
        private System.Windows.Forms.Button btBack;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Panel pnScroll;
    }
}