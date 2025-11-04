namespace AvoidGrenade
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            btnNewGame = new Button();
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            panel1 = new Panel();
            grenade = new PictureBox();
            btnCreatePanel = new Button();
            label1 = new Label();
            textBox1 = new TextBox();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)grenade).BeginInit();
            SuspendLayout();
            // 
            // btnNewGame
            // 
            btnNewGame.BackColor = Color.Green;
            btnNewGame.FlatAppearance.BorderSize = 0;
            btnNewGame.FlatAppearance.MouseOverBackColor = Color.Lime;
            btnNewGame.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnNewGame.ForeColor = Color.White;
            btnNewGame.Location = new Point(26, 454);
            btnNewGame.Name = "btnNewGame";
            btnNewGame.Size = new Size(152, 54);
            btnNewGame.TabIndex = 0;
            btnNewGame.Text = "Yeni Oyun";
            btnNewGame.UseVisualStyleBackColor = false;
            btnNewGame.Click += btnNewGame_Click;
            // 
            // panel1
            // 
            panel1.BackColor = Color.Silver;
            panel1.Controls.Add(grenade);
            panel1.Location = new Point(26, 27);
            panel1.Name = "panel1";
            panel1.Size = new Size(606, 405);
            panel1.TabIndex = 1;
            // 
            // grenade
            // 
            grenade.Image = (Image)resources.GetObject("grenade.Image");
            grenade.Location = new Point(181, 144);
            grenade.Name = "grenade";
            grenade.Size = new Size(60, 60);
            grenade.SizeMode = PictureBoxSizeMode.StretchImage;
            grenade.TabIndex = 0;
            grenade.TabStop = false;
           
            // 
            // btnCreatePanel
            // 
            btnCreatePanel.BackColor = Color.FromArgb(0, 0, 192);
            btnCreatePanel.FlatAppearance.MouseOverBackColor = Color.Cyan;
            btnCreatePanel.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCreatePanel.ForeColor = Color.White;
            btnCreatePanel.Location = new Point(184, 454);
            btnCreatePanel.Name = "btnCreatePanel";
            btnCreatePanel.Size = new Size(152, 54);
            btnCreatePanel.TabIndex = 2;
            btnCreatePanel.Text = "Panel Yarat";
            btnCreatePanel.UseVisualStyleBackColor = false;
            btnCreatePanel.Click += btnCreatePanel_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 162);
            label1.ForeColor = Color.White;
            label1.Location = new Point(389, 467);
            label1.Name = "label1";
            label1.Size = new Size(120, 28);
            label1.TabIndex = 3;
            label1.Text = "Panel Sayı :";
            // 
            // textBox1
            // 
            textBox1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 162);
            textBox1.Location = new Point(515, 467);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(117, 34);
            textBox1.TabIndex = 4;
            textBox1.Text = "0";
            textBox1.TextAlign = HorizontalAlignment.Right;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            ClientSize = new Size(656, 527);
            Controls.Add(textBox1);
            Controls.Add(label1);
            Controls.Add(btnCreatePanel);
            Controls.Add(btnNewGame);
            Controls.Add(panel1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Grenade";
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)grenade).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnNewGame;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Panel panel1;
        private Button btnCreatePanel;
        private Label label1;
        private TextBox textBox1;
        private PictureBox grenade;
    }
}
