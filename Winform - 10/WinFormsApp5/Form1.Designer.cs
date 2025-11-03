namespace WinFormsApp5
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
            car = new PictureBox();
            panel1 = new Panel();
            panel2 = new Panel();
            panel3 = new Panel();
            groupBox1 = new GroupBox();
            btnBack = new Button();
            btnRight = new Button();
            btnLeft = new Button();
            btnGas = new Button();
            btnBreak = new Button();
            timer1 = new System.Windows.Forms.Timer(components);
            ((System.ComponentModel.ISupportInitialize)car).BeginInit();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // car
            // 
            car.Image = (Image)resources.GetObject("car.Image");
            car.Location = new Point(190, 344);
            car.Name = "car";
            car.Size = new Size(106, 94);
            car.SizeMode = PictureBoxSizeMode.Zoom;
            car.TabIndex = 0;
            car.TabStop = false;
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.ButtonHighlight;
            panel1.Location = new Point(139, 12);
            panel1.Name = "panel1";
            panel1.Size = new Size(18, 127);
            panel1.TabIndex = 1;
            // 
            // panel2
            // 
            panel2.BackColor = SystemColors.ButtonHighlight;
            panel2.Location = new Point(139, 177);
            panel2.Name = "panel2";
            panel2.Size = new Size(18, 127);
            panel2.TabIndex = 2;
            // 
            // panel3
            // 
            panel3.BackColor = SystemColors.ButtonHighlight;
            panel3.Location = new Point(139, 331);
            panel3.Name = "panel3";
            panel3.Size = new Size(18, 127);
            panel3.TabIndex = 3;
            // 
            // groupBox1
            // 
            groupBox1.BackColor = SystemColors.ButtonHighlight;
            groupBox1.Controls.Add(btnBack);
            groupBox1.Controls.Add(btnRight);
            groupBox1.Controls.Add(btnLeft);
            groupBox1.Controls.Add(btnGas);
            groupBox1.Controls.Add(btnBreak);
            groupBox1.Location = new Point(310, -2);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(200, 450);
            groupBox1.TabIndex = 4;
            groupBox1.TabStop = false;
            groupBox1.Text = "groupBox1";
            // 
            // btnBack
            // 
            btnBack.BackColor = SystemColors.ActiveCaption;
            btnBack.Font = new Font("Segoe UI", 20F);
            btnBack.Location = new Point(71, 374);
            btnBack.Name = "btnBack";
            btnBack.Size = new Size(63, 47);
            btnBack.TabIndex = 4;
            btnBack.Text = "↓";
            btnBack.UseVisualStyleBackColor = false;
            btnBack.Click += btnBack_Click;
            // 
            // btnRight
            // 
            btnRight.BackColor = SystemColors.ActiveCaption;
            btnRight.Font = new Font("Segoe UI", 20F);
            btnRight.Location = new Point(122, 302);
            btnRight.Name = "btnRight";
            btnRight.Size = new Size(63, 47);
            btnRight.TabIndex = 3;
            btnRight.Text = "→";
            btnRight.UseVisualStyleBackColor = false;
            btnRight.Click += btnRight_Click;
            // 
            // btnLeft
            // 
            btnLeft.BackColor = SystemColors.ActiveCaption;
            btnLeft.Font = new Font("Segoe UI", 20F);
            btnLeft.Location = new Point(18, 302);
            btnLeft.Name = "btnLeft";
            btnLeft.Size = new Size(63, 47);
            btnLeft.TabIndex = 2;
            btnLeft.Text = "←\r\n\r\n";
            btnLeft.UseVisualStyleBackColor = false;
            btnLeft.Click += btnLeft_Click;
            // 
            // btnGas
            // 
            btnGas.BackColor = Color.LimeGreen;
            btnGas.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnGas.Location = new Point(44, 179);
            btnGas.Name = "btnGas";
            btnGas.Size = new Size(129, 72);
            btnGas.TabIndex = 1;
            btnGas.Text = "Gas";
            btnGas.UseVisualStyleBackColor = false;
            btnGas.Click += btnGas_Click;
            // 
            // btnBreak
            // 
            btnBreak.BackColor = Color.Red;
            btnBreak.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnBreak.Location = new Point(44, 62);
            btnBreak.Name = "btnBreak";
            btnBreak.Size = new Size(129, 72);
            btnBreak.TabIndex = 0;
            btnBreak.Text = "Break";
            btnBreak.UseVisualStyleBackColor = false;
            btnBreak.Click += btnBreak_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveBorder;
            ClientSize = new Size(507, 450);
            Controls.Add(groupBox1);
            Controls.Add(panel3);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Controls.Add(car);
            Name = "Form1";
            Text = "Car Game";
            ((System.ComponentModel.ISupportInitialize)car).EndInit();
            groupBox1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private PictureBox car;
        private Panel panel1;
        private Panel panel2;
        private Panel panel3;
        private GroupBox groupBox1;
        private Button btnGas;
        private Button btnBreak;
        private Button btnRight;
        private Button btnLeft;
        private Button btnBack;
        private System.Windows.Forms.Timer timer1;
    }
}
