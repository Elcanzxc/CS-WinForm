namespace calculator
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
            num1 = new MaskedTextBox();
            num2 = new MaskedTextBox();
            comboop = new ComboBox();
            history = new ListBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            button1 = new Button();
            SuspendLayout();
            // 
            // num1
            // 
            num1.Location = new Point(73, 67);
            num1.Mask = "00000000";
            num1.Name = "num1";
            num1.Size = new Size(158, 23);
            num1.TabIndex = 0;
            // 
            // num2
            // 
            num2.Location = new Point(73, 150);
            num2.Mask = "00000000";
            num2.Name = "num2";
            num2.Size = new Size(158, 23);
            num2.TabIndex = 1;
            // 
            // comboop
            // 
            comboop.FormattingEnabled = true;
            comboop.Items.AddRange(new object[] { "+", "-", "*", "/" });
            comboop.Location = new Point(117, 227);
            comboop.Name = "comboop";
            comboop.Size = new Size(76, 23);
            comboop.TabIndex = 2;
            comboop.SelectedIndexChanged += comboop_SelectedIndexChanged;
            // 
            // history
            // 
            history.FormattingEnabled = true;
            history.ItemHeight = 15;
            history.Location = new Point(73, 361);
            history.Name = "history";
            history.Size = new Size(158, 94);
            history.TabIndex = 3;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(73, 26);
            label1.Name = "label1";
            label1.Size = new Size(92, 20);
            label1.TabIndex = 4;
            label1.Text = "Number one";
            label1.Click += label1_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(73, 113);
            label2.Name = "label2";
            label2.Size = new Size(92, 20);
            label2.TabIndex = 5;
            label2.Text = "Number two";
            label2.Click += label2_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(117, 190);
            label3.Name = "label3";
            label3.Size = new Size(76, 20);
            label3.TabIndex = 6;
            label3.Text = "Operation";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.Location = new Point(128, 327);
            label4.Name = "label4";
            label4.Size = new Size(56, 20);
            label4.TabIndex = 7;
            label4.Text = "History";
            label4.Click += label4_Click;
            // 
            // button1
            // 
            button1.BackColor = Color.FromArgb(128, 255, 128);
            button1.Location = new Point(109, 285);
            button1.Name = "button1";
            button1.Size = new Size(101, 23);
            button1.TabIndex = 8;
            button1.Text = "calculate";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(128, 255, 128);
            ClientSize = new Size(304, 479);
            Controls.Add(button1);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(history);
            Controls.Add(comboop);
            Controls.Add(num2);
            Controls.Add(num1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            Text = "Simple Calculator";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MaskedTextBox num1;
        private MaskedTextBox num2;
        private ComboBox comboop;
        private ListBox history;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Button button1;
    }
}
