namespace WinForm
{
    partial class Form
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
            groupBox = new GroupBox();
            btnShow = new Button();
            University = new Label();
            InputUniversity = new TextBox();
            label1 = new Label();
            InputLiveCity = new TextBox();
            ProgrammingLanguage = new Label();
            checkedListBox = new CheckedListBox();
            Language = new Label();
            InputLanguage = new ComboBox();
            Gender = new Label();
            radioFemale = new RadioButton();
            radioMale = new RadioButton();
            groupBox.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox
            // 
            groupBox.BackColor = Color.Gainsboro;
            groupBox.Controls.Add(btnShow);
            groupBox.Controls.Add(University);
            groupBox.Controls.Add(InputUniversity);
            groupBox.Controls.Add(label1);
            groupBox.Controls.Add(InputLiveCity);
            groupBox.Controls.Add(ProgrammingLanguage);
            groupBox.Controls.Add(checkedListBox);
            groupBox.Controls.Add(Language);
            groupBox.Controls.Add(InputLanguage);
            groupBox.Controls.Add(Gender);
            groupBox.Controls.Add(radioFemale);
            groupBox.Controls.Add(radioMale);
            groupBox.Dock = DockStyle.Fill;
            groupBox.Location = new Point(0, 0);
            groupBox.Name = "groupBox";
            groupBox.Size = new Size(784, 545);
            groupBox.TabIndex = 0;
            groupBox.TabStop = false;
            groupBox.Text = "Anketa";
            groupBox.Enter += groupBox_Enter;
            // 
            // btnShow
            // 
            btnShow.BackColor = Color.Silver;
            btnShow.Font = new Font("Gadugi", 12F);
            btnShow.Location = new Point(300, 445);
            btnShow.Name = "btnShow";
            btnShow.Size = new Size(209, 66);
            btnShow.TabIndex = 11;
            btnShow.Text = "Show Info";
            btnShow.UseVisualStyleBackColor = false;
            btnShow.Click += btnShow_Click;
            // 
            // University
            // 
            University.AutoSize = true;
            University.Font = new Font("Gadugi", 20.25F, FontStyle.Bold);
            University.Location = new Point(540, 258);
            University.Name = "University";
            University.Size = new Size(145, 32);
            University.TabIndex = 10;
            University.Text = "University";
            // 
            // InputUniversity
            // 
            InputUniversity.Location = new Point(521, 293);
            InputUniversity.Name = "InputUniversity";
            InputUniversity.Size = new Size(220, 25);
            InputUniversity.TabIndex = 9;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Gadugi", 20.25F, FontStyle.Bold);
            label1.Location = new Point(540, 73);
            label1.Name = "label1";
            label1.Size = new Size(172, 32);
            label1.TabIndex = 8;
            label1.Text = "Yaşadığı yer";
            // 
            // InputLiveCity
            // 
            InputLiveCity.Location = new Point(521, 108);
            InputLiveCity.Name = "InputLiveCity";
            InputLiveCity.Size = new Size(212, 25);
            InputLiveCity.TabIndex = 7;
            // 
            // ProgrammingLanguage
            // 
            ProgrammingLanguage.AutoSize = true;
            ProgrammingLanguage.Font = new Font("Gadugi", 20.25F, FontStyle.Bold);
            ProgrammingLanguage.Location = new Point(21, 258);
            ProgrammingLanguage.Name = "ProgrammingLanguage";
            ProgrammingLanguage.Size = new Size(420, 32);
            ProgrammingLanguage.TabIndex = 6;
            ProgrammingLanguage.Text = "Choose programming language";
            // 
            // checkedListBox
            // 
            checkedListBox.Font = new Font("Gadugi", 12F);
            checkedListBox.FormattingEnabled = true;
            checkedListBox.Items.AddRange(new object[] { "C#", "Java", "Python", "C++" });
            checkedListBox.Location = new Point(127, 293);
            checkedListBox.Name = "checkedListBox";
            checkedListBox.Size = new Size(137, 100);
            checkedListBox.TabIndex = 5;
            // 
            // Language
            // 
            Language.AutoSize = true;
            Language.Font = new Font("Gadugi", 20.25F, FontStyle.Bold);
            Language.Location = new Point(201, 73);
            Language.Name = "Language";
            Language.Size = new Size(240, 32);
            Language.TabIndex = 4;
            Language.Text = "Choose Language";
            // 
            // InputLanguage
            // 
            InputLanguage.FormattingEnabled = true;
            InputLanguage.Items.AddRange(new object[] { "Russian", "Azerbaijan", "English" });
            InputLanguage.Location = new Point(227, 108);
            InputLanguage.Name = "InputLanguage";
            InputLanguage.Size = new Size(176, 25);
            InputLanguage.TabIndex = 3;
            // 
            // Gender
            // 
            Gender.AutoSize = true;
            Gender.Font = new Font("Gadugi", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            Gender.Location = new Point(38, 73);
            Gender.Name = "Gender";
            Gender.Size = new Size(107, 32);
            Gender.TabIndex = 2;
            Gender.Text = "Gender";
            Gender.Click += label1_Click;
            // 
            // radioFemale
            // 
            radioFemale.AutoSize = true;
            radioFemale.Font = new Font("Gadugi", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            radioFemale.Location = new Point(38, 137);
            radioFemale.Name = "radioFemale";
            radioFemale.Size = new Size(77, 23);
            radioFemale.TabIndex = 1;
            radioFemale.TabStop = true;
            radioFemale.Text = "Female";
            radioFemale.UseVisualStyleBackColor = true;
            // 
            // radioMale
            // 
            radioMale.AutoSize = true;
            radioMale.Font = new Font("Gadugi", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            radioMale.Location = new Point(38, 108);
            radioMale.Name = "radioMale";
            radioMale.Size = new Size(61, 23);
            radioMale.TabIndex = 0;
            radioMale.TabStop = true;
            radioMale.Text = "Male";
            radioMale.UseVisualStyleBackColor = true;
            radioMale.CheckedChanged += radioMale_CheckedChanged;
            // 
            // Form
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(784, 545);
            Controls.Add(groupBox);
            Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 204);
            Name = "Form";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Anket";
            groupBox.ResumeLayout(false);
            groupBox.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox;
        private Label Gender;
        private RadioButton radioFemale;
        private RadioButton radioMale;
        private Label Language;
        private ComboBox InputLanguage;
        private Label ProgrammingLanguage;
        private CheckedListBox checkedListBox;
        private Label University;
        private TextBox InputUniversity;
        private Label label1;
        private TextBox InputLiveCity;
        private Button btnShow;
    }
}
