namespace Bomb_Rain
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            bomb1 = new PictureBox();
            bomb2 = new PictureBox();
            timer1 = new System.Windows.Forms.Timer(components);
            right = new PictureBox();
            left = new PictureBox();
            man = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)bomb1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)bomb2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)right).BeginInit();
            ((System.ComponentModel.ISupportInitialize)left).BeginInit();
            ((System.ComponentModel.ISupportInitialize)man).BeginInit();
            SuspendLayout();
            // 
            // bomb1
            // 
            bomb1.Image = (Image)resources.GetObject("bomb1.Image");
            bomb1.Location = new Point(367, 0);
            bomb1.Margin = new Padding(3, 2, 3, 2);
            bomb1.Name = "bomb1";
            bomb1.Size = new Size(61, 45);
            bomb1.SizeMode = PictureBoxSizeMode.Zoom;
            bomb1.TabIndex = 1;
            bomb1.TabStop = false;
            // 
            // bomb2
            // 
            bomb2.Image = (Image)resources.GetObject("bomb2.Image");
            bomb2.Location = new Point(140, 0);
            bomb2.Margin = new Padding(3, 2, 3, 2);
            bomb2.Name = "bomb2";
            bomb2.Size = new Size(61, 45);
            bomb2.SizeMode = PictureBoxSizeMode.Zoom;
            bomb2.TabIndex = 2;
            bomb2.TabStop = false;
            // 
            // timer1
            // 
            timer1.Interval = 200;
            timer1.Tick += timer1_Tick;
            // 
            // right
            // 
            right.Image = (Image)resources.GetObject("right.Image");
            right.Location = new Point(224, 304);
            right.Margin = new Padding(3, 2, 3, 2);
            right.Name = "right";
            right.Size = new Size(110, 119);
            right.SizeMode = PictureBoxSizeMode.StretchImage;
            right.TabIndex = 7;
            right.TabStop = false;
            // 
            // left
            // 
            left.Image = (Image)resources.GetObject("left.Image");
            left.Location = new Point(224, 304);
            left.Margin = new Padding(3, 2, 3, 2);
            left.Name = "left";
            left.Size = new Size(110, 119);
            left.SizeMode = PictureBoxSizeMode.StretchImage;
            left.TabIndex = 8;
            left.TabStop = false;
            // 
            // man
            // 
            man.Image = (Image)resources.GetObject("man.Image");
            man.Location = new Point(224, 293);
            man.Margin = new Padding(3, 2, 3, 2);
            man.Name = "man";
            man.Size = new Size(110, 130);
            man.SizeMode = PictureBoxSizeMode.StretchImage;
            man.TabIndex = 9;
            man.TabStop = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaption;
            ClientSize = new Size(575, 423);
            Controls.Add(man);
            Controls.Add(left);
            Controls.Add(right);
            Controls.Add(bomb2);
            Controls.Add(bomb1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(3, 2, 3, 2);
            MaximumSize = new Size(591, 462);
            MinimumSize = new Size(591, 462);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Bomb rain";
            Load += Form1_Load;
            KeyDown += Form1_KeyDown;
            KeyUp += Form1_KeyUp;
            ((System.ComponentModel.ISupportInitialize)bomb1).EndInit();
            ((System.ComponentModel.ISupportInitialize)bomb2).EndInit();
            ((System.ComponentModel.ISupportInitialize)right).EndInit();
            ((System.ComponentModel.ISupportInitialize)left).EndInit();
            ((System.ComponentModel.ISupportInitialize)man).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox bomb1;
        private PictureBox bomb2;
        private System.Windows.Forms.Timer timer1;
        private PictureBox right;
        private PictureBox left;
        private PictureBox man;
    }
}
