namespace ArkanoidGame
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
            top = new PictureBox();
            panel1 = new Panel();
            timer1 = new System.Windows.Forms.Timer(components);
            ((System.ComponentModel.ISupportInitialize)top).BeginInit();
            SuspendLayout();
            // 
            // top
            // 
            top.BackColor = Color.Transparent;
            top.Image = (Image)resources.GetObject("top.Image");
            top.Location = new Point(370, 22);
            top.Margin = new Padding(3, 2, 3, 2);
            top.Name = "top";
            top.Size = new Size(61, 52);
            top.SizeMode = PictureBoxSizeMode.StretchImage;
            top.TabIndex = 0;
            top.TabStop = false;
            top.Click += top_Click;
            // 
            // panel1
            // 
            panel1.BackColor = Color.Lime;
            panel1.Location = new Point(329, 362);
            panel1.Margin = new Padding(3, 2, 3, 2);
            panel1.Name = "panel1";
            panel1.Size = new Size(164, 18);
            panel1.TabIndex = 1;
            panel1.Paint += panel1_Paint;
            // 
            // timer1
            // 
            timer1.Interval = 10;
            timer1.Tick += timer1_Tick;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            ClientSize = new Size(831, 464);
            Controls.Add(panel1);
            Controls.Add(top);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(3, 2, 3, 2);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Arkanoid Game";
            Load += Form1_Load;
            KeyDown += Form1_KeyDown;
            KeyUp += Form1_KeyUp;
            ((System.ComponentModel.ISupportInitialize)top).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox top;
        private Panel panel1;
        private System.Windows.Forms.Timer timer1;
    }
}
