namespace MyForm
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
            InputPassword = new TextBox();
            CheckPassword = new Button();
            EnterPassword = new Label();
            SuspendLayout();
            // 
            // InputPassword
            // 
            InputPassword.Location = new Point(228, 192);
            InputPassword.Name = "InputPassword";
            InputPassword.PasswordChar = '*';
            InputPassword.Size = new Size(278, 23);
            InputPassword.TabIndex = 0;
            InputPassword.TextChanged += textBox1_TextChanged;
            // 
            // CheckPassword
            // 
            CheckPassword.Location = new Point(325, 238);
            CheckPassword.Name = "CheckPassword";
            CheckPassword.Size = new Size(94, 67);
            CheckPassword.TabIndex = 1;
            CheckPassword.Text = "Check";
            CheckPassword.UseVisualStyleBackColor = true;
            CheckPassword.Click += CheckPassword_Click;
            // 
            // EnterPassword
            // 
            EnterPassword.AutoSize = true;
            EnterPassword.Font = new Font("Gadugi", 24F, FontStyle.Bold, GraphicsUnit.Point, 0);
            EnterPassword.Location = new Point(254, 122);
            EnterPassword.Name = "EnterPassword";
            EnterPassword.Size = new Size(239, 38);
            EnterPassword.TabIndex = 2;
            EnterPassword.Text = "EnterPassword";
            EnterPassword.Click += label1_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.BlanchedAlmond;
            ClientSize = new Size(800, 450);
            Controls.Add(EnterPassword);
            Controls.Add(CheckPassword);
            Controls.Add(InputPassword);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox InputPassword;
        private Button CheckPassword;
        private Label EnterPassword;
    }
}
