namespace calculator4
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
            textBox1 = new RichTextBox();
            buttonce = new Button();
            buttonc = new Button();
            buttonsqrt = new Button();
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            buttonplus = new Button();
            button4 = new Button();
            button5 = new Button();
            button6 = new Button();
            buttonminus = new Button();
            button7 = new Button();
            button8 = new Button();
            button9 = new Button();
            buttonx = new Button();
            buttondot = new Button();
            button0 = new Button();
            buttondraw = new Button();
            buttondivide = new Button();
            buttonpercent = new Button();
            SuspendLayout();
            // 
            // textBox1
            // 
            textBox1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBox1.Location = new Point(12, 12);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(297, 96);
            textBox1.TabIndex = 0;
            textBox1.Text = "0";
            // 
            // buttonce
            // 
            buttonce.Location = new Point(12, 139);
            buttonce.Name = "buttonce";
            buttonce.Size = new Size(69, 47);
            buttonce.TabIndex = 1;
            buttonce.Text = "CE";
            buttonce.UseVisualStyleBackColor = true;
            buttonce.Click += CE_Click;
            // 
            // buttonc
            // 
            buttonc.Location = new Point(87, 139);
            buttonc.Name = "buttonc";
            buttonc.Size = new Size(69, 47);
            buttonc.TabIndex = 2;
            buttonc.Text = "C";
            buttonc.UseVisualStyleBackColor = true;
            buttonc.Click += C_Click;
            // 
            // buttonsqrt
            // 
            buttonsqrt.Location = new Point(162, 139);
            buttonsqrt.Name = "buttonsqrt";
            buttonsqrt.Size = new Size(69, 47);
            buttonsqrt.TabIndex = 3;
            buttonsqrt.Text = "Sqrt";
            buttonsqrt.UseVisualStyleBackColor = true;
            buttonsqrt.Click += Sqrt_Click;
            // 
            // button1
            // 
            button1.Cursor = Cursors.SizeAll;
            button1.Location = new Point(12, 203);
            button1.Name = "button1";
            button1.Size = new Size(69, 47);
            button1.TabIndex = 5;
            button1.Text = "1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += Number_Click;
            // 
            // button2
            // 
            button2.Location = new Point(87, 203);
            button2.Name = "button2";
            button2.Size = new Size(69, 47);
            button2.TabIndex = 6;
            button2.Text = "2";
            button2.UseVisualStyleBackColor = true;
            button2.Click += Number_Click;
            // 
            // button3
            // 
            button3.Location = new Point(162, 203);
            button3.Name = "button3";
            button3.Size = new Size(69, 47);
            button3.TabIndex = 7;
            button3.Text = "3";
            button3.UseVisualStyleBackColor = true;
            button3.Click += Number_Click;
            // 
            // buttonplus
            // 
            buttonplus.Location = new Point(237, 203);
            buttonplus.Name = "buttonplus";
            buttonplus.Size = new Size(69, 47);
            buttonplus.TabIndex = 8;
            buttonplus.Text = "+";
            buttonplus.UseVisualStyleBackColor = true;
            buttonplus.Click += Op_Click;
            // 
            // button4
            // 
            button4.Location = new Point(12, 268);
            button4.Name = "button4";
            button4.Size = new Size(69, 47);
            button4.TabIndex = 9;
            button4.Text = "4";
            button4.UseVisualStyleBackColor = true;
            button4.Click += Number_Click;
            // 
            // button5
            // 
            button5.Location = new Point(87, 268);
            button5.Name = "button5";
            button5.Size = new Size(69, 47);
            button5.TabIndex = 10;
            button5.Text = "5";
            button5.UseVisualStyleBackColor = true;
            button5.Click += Number_Click;
            // 
            // button6
            // 
            button6.Location = new Point(162, 268);
            button6.Name = "button6";
            button6.Size = new Size(69, 47);
            button6.TabIndex = 11;
            button6.Text = "6";
            button6.UseVisualStyleBackColor = true;
            button6.Click += Number_Click;
            // 
            // buttonminus
            // 
            buttonminus.Location = new Point(240, 268);
            buttonminus.Name = "buttonminus";
            buttonminus.Size = new Size(69, 47);
            buttonminus.TabIndex = 12;
            buttonminus.Text = "-";
            buttonminus.UseVisualStyleBackColor = true;
            buttonminus.Click += Op_Click;
            // 
            // button7
            // 
            button7.Location = new Point(12, 330);
            button7.Name = "button7";
            button7.Size = new Size(69, 47);
            button7.TabIndex = 13;
            button7.Text = "7";
            button7.UseVisualStyleBackColor = true;
            button7.Click += Number_Click;
            // 
            // button8
            // 
            button8.Location = new Point(87, 330);
            button8.Name = "button8";
            button8.Size = new Size(69, 47);
            button8.TabIndex = 14;
            button8.Text = "8";
            button8.UseVisualStyleBackColor = true;
            button8.Click += Number_Click;
            // 
            // button9
            // 
            button9.Location = new Point(162, 330);
            button9.Name = "button9";
            button9.Size = new Size(69, 47);
            button9.TabIndex = 15;
            button9.Text = "9";
            button9.UseVisualStyleBackColor = true;
            button9.Click += Number_Click;
            // 
            // buttonx
            // 
            buttonx.Location = new Point(240, 330);
            buttonx.Name = "buttonx";
            buttonx.Size = new Size(69, 47);
            buttonx.TabIndex = 16;
            buttonx.Text = "x";
            buttonx.UseVisualStyleBackColor = true;
            buttonx.Click += Op_Click;
            // 
            // buttondot
            // 
            buttondot.Location = new Point(12, 392);
            buttondot.Name = "buttondot";
            buttondot.Size = new Size(69, 47);
            buttondot.TabIndex = 17;
            buttondot.Text = ".";
            buttondot.UseVisualStyleBackColor = true;
            buttondot.Click += Number_Click;
            // 
            // button0
            // 
            button0.Location = new Point(87, 392);
            button0.Name = "button0";
            button0.Size = new Size(69, 47);
            button0.TabIndex = 18;
            button0.Text = "0";
            button0.UseVisualStyleBackColor = true;
            button0.Click += Number_Click;
            // 
            // buttondraw
            // 
            buttondraw.Location = new Point(162, 392);
            buttondraw.Name = "buttondraw";
            buttondraw.Size = new Size(69, 47);
            buttondraw.TabIndex = 19;
            buttondraw.Text = "=";
            buttondraw.UseVisualStyleBackColor = true;
            buttondraw.Click += Equals_Click;
            // 
            // buttondivide
            // 
            buttondivide.Location = new Point(240, 392);
            buttondivide.Name = "buttondivide";
            buttondivide.Size = new Size(69, 47);
            buttondivide.TabIndex = 20;
            buttondivide.Text = "/";
            buttondivide.UseVisualStyleBackColor = true;
            buttondivide.Click += Op_Click;
            // 
            // buttonpercent
            // 
            buttonpercent.Location = new Point(240, 139);
            buttonpercent.Name = "buttonpercent";
            buttonpercent.Size = new Size(69, 47);
            buttonpercent.TabIndex = 4;
            buttonpercent.Text = "%";
            buttonpercent.UseVisualStyleBackColor = true;
            buttonpercent.Click += Percent_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Maroon;
            ClientSize = new Size(321, 462);
            Controls.Add(buttondivide);
            Controls.Add(buttondraw);
            Controls.Add(button0);
            Controls.Add(buttondot);
            Controls.Add(buttonx);
            Controls.Add(button9);
            Controls.Add(button8);
            Controls.Add(button7);
            Controls.Add(buttonminus);
            Controls.Add(button6);
            Controls.Add(button5);
            Controls.Add(button4);
            Controls.Add(buttonplus);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(buttonpercent);
            Controls.Add(buttonsqrt);
            Controls.Add(buttonc);
            Controls.Add(buttonce);
            Controls.Add(textBox1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            Text = "Calculator";
            ResumeLayout(false);
        }

        #endregion

        private RichTextBox textBox1;
        private Button buttonce;
        private Button buttonc;
        private Button buttonsqrt;
        private Button button1;
        private Button button2;
        private Button button3;
        private Button buttonplus;
        private Button button4;
        private Button button5;
        private Button button6;
        private Button buttonminus;
        private Button button7;
        private Button button8;
        private Button button9;
        private Button buttonx;
        private Button buttondot;
        private Button button0;
        private Button buttondraw;
        private Button buttondivide;
        private Button buttonpercent;
    }
}
