namespace dailyhealth
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            lblTitle = new Label();
            grpUserInfo = new GroupBox();
            lblWeight = new Label();
            lblHeight = new Label();
            lblGender = new Label();
            lblAge = new Label();
            lblName = new Label();
            cmbGender = new ComboBox();
            txtWeight = new TextBox();
            txtHeight = new TextBox();
            txtAge = new TextBox();
            txtName = new TextBox();
            grpDailyTracking = new GroupBox();
            lblMeals = new Label();
            lblActivity = new Label();
            lblSleep = new Label();
            lblWater = new Label();
            chkDinner = new CheckBox();
            chkLunch = new CheckBox();
            chkBreakfast = new CheckBox();
            cmbActivity = new ComboBox();
            txtSleep = new TextBox();
            txtWater = new TextBox();
            grpResults = new GroupBox();
            lblProgress = new Label();
            progressBar = new ProgressBar();
            lblHealthFeedback = new Label();
            lblBMIResult = new Label();
            btnCalculateBMI = new Button();
            btnSaveData = new Button();
            btnClear = new Button();
            btnShowSummary = new Button();
            lstSummary = new ListBox();
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            toolsToolStripMenuItem = new ToolStripMenuItem();
            historyToolStripMenuItem = new ToolStripMenuItem();
            calorieCalculatorToolStripMenuItem = new ToolStripMenuItem();
            workoutPlanToolStripMenuItem = new ToolStripMenuItem();
            settingsToolStripMenuItem = new ToolStripMenuItem();
            helpToolStripMenuItem = new ToolStripMenuItem();
            aboutToolStripMenuItem = new ToolStripMenuItem();
            grpUserInfo.SuspendLayout();
            grpDailyTracking.SuspendLayout();
            grpResults.SuspendLayout();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitle.ForeColor = Color.DarkBlue;
            lblTitle.Location = new Point(200, 30);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(449, 32);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Программа отслеживания здоровья";
            // 
            // grpUserInfo
            // 
            grpUserInfo.Controls.Add(lblWeight);
            grpUserInfo.Controls.Add(lblHeight);
            grpUserInfo.Controls.Add(lblGender);
            grpUserInfo.Controls.Add(lblAge);
            grpUserInfo.Controls.Add(lblName);
            grpUserInfo.Controls.Add(cmbGender);
            grpUserInfo.Controls.Add(txtWeight);
            grpUserInfo.Controls.Add(txtHeight);
            grpUserInfo.Controls.Add(txtAge);
            grpUserInfo.Controls.Add(txtName);
            grpUserInfo.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            grpUserInfo.Location = new Point(20, 75);
            grpUserInfo.Name = "grpUserInfo";
            grpUserInfo.Size = new Size(350, 200);
            grpUserInfo.TabIndex = 1;
            grpUserInfo.TabStop = false;
            grpUserInfo.Text = "Информация пользователя";
            // 
            // lblWeight
            // 
            lblWeight.AutoSize = true;
            lblWeight.Font = new Font("Segoe UI", 9F);
            lblWeight.Location = new Point(20, 153);
            lblWeight.Name = "lblWeight";
            lblWeight.Size = new Size(51, 15);
            lblWeight.TabIndex = 9;
            lblWeight.Text = "Вес (кг):";
            // 
            // lblHeight
            // 
            lblHeight.AutoSize = true;
            lblHeight.Font = new Font("Segoe UI", 9F);
            lblHeight.Location = new Point(20, 123);
            lblHeight.Name = "lblHeight";
            lblHeight.Size = new Size(61, 15);
            lblHeight.TabIndex = 8;
            lblHeight.Text = "Рост (см):";
            // 
            // lblGender
            // 
            lblGender.AutoSize = true;
            lblGender.Font = new Font("Segoe UI", 9F);
            lblGender.Location = new Point(20, 93);
            lblGender.Name = "lblGender";
            lblGender.Size = new Size(33, 15);
            lblGender.TabIndex = 7;
            lblGender.Text = "Пол:";
            // 
            // lblAge
            // 
            lblAge.AutoSize = true;
            lblAge.Font = new Font("Segoe UI", 9F);
            lblAge.Location = new Point(20, 63);
            lblAge.Name = "lblAge";
            lblAge.Size = new Size(53, 15);
            lblAge.TabIndex = 6;
            lblAge.Text = "Возраст:";
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.Font = new Font("Segoe UI", 9F);
            lblName.Location = new Point(20, 33);
            lblName.Name = "lblName";
            lblName.Size = new Size(34, 15);
            lblName.TabIndex = 5;
            lblName.Text = "Имя:";
            // 
            // cmbGender
            // 
            cmbGender.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbGender.Font = new Font("Segoe UI", 9F);
            cmbGender.FormattingEnabled = true;
            cmbGender.Items.AddRange(new object[] { "Мужской", "Женский", "Другой" });
            cmbGender.Location = new Point(120, 90);
            cmbGender.Name = "cmbGender";
            cmbGender.Size = new Size(200, 23);
            cmbGender.TabIndex = 2;
            // 
            // txtWeight
            // 
            txtWeight.Font = new Font("Segoe UI", 9F);
            txtWeight.Location = new Point(120, 150);
            txtWeight.Name = "txtWeight";
            txtWeight.Size = new Size(200, 23);
            txtWeight.TabIndex = 4;
            // 
            // txtHeight
            // 
            txtHeight.Font = new Font("Segoe UI", 9F);
            txtHeight.Location = new Point(120, 120);
            txtHeight.Name = "txtHeight";
            txtHeight.Size = new Size(200, 23);
            txtHeight.TabIndex = 3;
            // 
            // txtAge
            // 
            txtAge.Font = new Font("Segoe UI", 9F);
            txtAge.Location = new Point(120, 60);
            txtAge.Name = "txtAge";
            txtAge.Size = new Size(200, 23);
            txtAge.TabIndex = 1;
            // 
            // txtName
            // 
            txtName.Font = new Font("Segoe UI", 9F);
            txtName.Location = new Point(120, 30);
            txtName.Name = "txtName";
            txtName.Size = new Size(200, 23);
            txtName.TabIndex = 0;
            txtName.TextChanged += txtName_TextChanged;
            // 
            // grpDailyTracking
            // 
            grpDailyTracking.Controls.Add(lblMeals);
            grpDailyTracking.Controls.Add(lblActivity);
            grpDailyTracking.Controls.Add(lblSleep);
            grpDailyTracking.Controls.Add(lblWater);
            grpDailyTracking.Controls.Add(chkDinner);
            grpDailyTracking.Controls.Add(chkLunch);
            grpDailyTracking.Controls.Add(chkBreakfast);
            grpDailyTracking.Controls.Add(cmbActivity);
            grpDailyTracking.Controls.Add(txtSleep);
            grpDailyTracking.Controls.Add(txtWater);
            grpDailyTracking.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            grpDailyTracking.Location = new Point(390, 75);
            grpDailyTracking.Name = "grpDailyTracking";
            grpDailyTracking.Size = new Size(380, 200);
            grpDailyTracking.TabIndex = 2;
            grpDailyTracking.TabStop = false;
            grpDailyTracking.Text = "Отслеживание дня";
            // 
            // lblMeals
            // 
            lblMeals.AutoSize = true;
            lblMeals.Font = new Font("Segoe UI", 9F);
            lblMeals.Location = new Point(20, 126);
            lblMeals.Name = "lblMeals";
            lblMeals.Size = new Size(92, 15);
            lblMeals.TabIndex = 9;
            lblMeals.Text = "Приемы пищи:";
            // 
            // lblActivity
            // 
            lblActivity.AutoSize = true;
            lblActivity.Font = new Font("Segoe UI", 9F);
            lblActivity.Location = new Point(20, 93);
            lblActivity.Name = "lblActivity";
            lblActivity.Size = new Size(73, 15);
            lblActivity.TabIndex = 8;
            lblActivity.Text = "Активность:";
            // 
            // lblSleep
            // 
            lblSleep.AutoSize = true;
            lblSleep.Font = new Font("Segoe UI", 9F);
            lblSleep.Location = new Point(20, 63);
            lblSleep.Name = "lblSleep";
            lblSleep.Size = new Size(75, 15);
            lblSleep.TabIndex = 7;
            lblSleep.Text = "Сон (часов):";
            // 
            // lblWater
            // 
            lblWater.AutoSize = true;
            lblWater.Font = new Font("Segoe UI", 9F);
            lblWater.Location = new Point(20, 33);
            lblWater.Name = "lblWater";
            lblWater.Size = new Size(96, 15);
            lblWater.TabIndex = 6;
            lblWater.Text = "Вода (стаканов):";
            // 
            // chkDinner
            // 
            chkDinner.AutoSize = true;
            chkDinner.Font = new Font("Segoe UI", 9F);
            chkDinner.Location = new Point(180, 175);
            chkDinner.Name = "chkDinner";
            chkDinner.Size = new Size(56, 19);
            chkDinner.TabIndex = 5;
            chkDinner.Text = "Ужин";
            chkDinner.UseVisualStyleBackColor = true;
            // 
            // chkLunch
            // 
            chkLunch.AutoSize = true;
            chkLunch.Font = new Font("Segoe UI", 9F);
            chkLunch.Location = new Point(180, 150);
            chkLunch.Name = "chkLunch";
            chkLunch.Size = new Size(54, 19);
            chkLunch.TabIndex = 4;
            chkLunch.Text = "Обед";
            chkLunch.UseVisualStyleBackColor = true;
            // 
            // chkBreakfast
            // 
            chkBreakfast.AutoSize = true;
            chkBreakfast.Font = new Font("Segoe UI", 9F);
            chkBreakfast.Location = new Point(180, 125);
            chkBreakfast.Name = "chkBreakfast";
            chkBreakfast.Size = new Size(69, 19);
            chkBreakfast.TabIndex = 3;
            chkBreakfast.Text = "Завтрак";
            chkBreakfast.UseVisualStyleBackColor = true;
            // 
            // cmbActivity
            // 
            cmbActivity.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbActivity.Font = new Font("Segoe UI", 9F);
            cmbActivity.FormattingEnabled = true;
            cmbActivity.Items.AddRange(new object[] { "Нет", "Легкая", "Умеренная", "Интенсивная" });
            cmbActivity.Location = new Point(180, 90);
            cmbActivity.Name = "cmbActivity";
            cmbActivity.Size = new Size(180, 23);
            cmbActivity.TabIndex = 2;
            // 
            // txtSleep
            // 
            txtSleep.Font = new Font("Segoe UI", 9F);
            txtSleep.Location = new Point(180, 60);
            txtSleep.Name = "txtSleep";
            txtSleep.Size = new Size(180, 23);
            txtSleep.TabIndex = 1;
            // 
            // txtWater
            // 
            txtWater.Font = new Font("Segoe UI", 9F);
            txtWater.Location = new Point(180, 30);
            txtWater.Name = "txtWater";
            txtWater.Size = new Size(180, 23);
            txtWater.TabIndex = 0;
            // 
            // grpResults
            // 
            grpResults.Controls.Add(lblProgress);
            grpResults.Controls.Add(progressBar);
            grpResults.Controls.Add(lblHealthFeedback);
            grpResults.Controls.Add(lblBMIResult);
            grpResults.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            grpResults.Location = new Point(20, 290);
            grpResults.Name = "grpResults";
            grpResults.Size = new Size(350, 180);
            grpResults.TabIndex = 3;
            grpResults.TabStop = false;
            grpResults.Text = "Результаты и обратная связь";
            // 
            // lblProgress
            // 
            lblProgress.AutoSize = true;
            lblProgress.Font = new Font("Segoe UI", 9F);
            lblProgress.Location = new Point(20, 120);
            lblProgress.Name = "lblProgress";
            lblProgress.Size = new Size(135, 15);
            lblProgress.TabIndex = 3;
            lblProgress.Text = "Прогресс здоровья: 0%";
            // 
            // progressBar
            // 
            progressBar.Location = new Point(20, 140);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(310, 25);
            progressBar.TabIndex = 2;
            // 
            // lblHealthFeedback
            // 
            lblHealthFeedback.Font = new Font("Segoe UI", 9F);
            lblHealthFeedback.Location = new Point(20, 60);
            lblHealthFeedback.Name = "lblHealthFeedback";
            lblHealthFeedback.Size = new Size(310, 50);
            lblHealthFeedback.TabIndex = 1;
            lblHealthFeedback.Text = "Здесь появится обратная связь...";
            // 
            // lblBMIResult
            // 
            lblBMIResult.AutoSize = true;
            lblBMIResult.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblBMIResult.ForeColor = Color.DarkGreen;
            lblBMIResult.Location = new Point(20, 30);
            lblBMIResult.Name = "lblBMIResult";
            lblBMIResult.Size = new Size(173, 19);
            lblBMIResult.TabIndex = 0;
            lblBMIResult.Text = "ИМТ: Еще не рассчитан";
            // 
            // btnCalculateBMI
            // 
            btnCalculateBMI.Click += new System.EventHandler(this.btnCalculateBMI_Click);

            btnCalculateBMI.BackColor = Color.LightBlue;
            btnCalculateBMI.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnCalculateBMI.Location = new Point(20, 490);
            btnCalculateBMI.Name = "btnCalculateBMI";
            btnCalculateBMI.Size = new Size(150, 40);
            btnCalculateBMI.TabIndex = 4;
            btnCalculateBMI.Text = "Рассчитать ИМТ";
            btnCalculateBMI.UseVisualStyleBackColor = false;
            btnCalculateBMI.Click += btnCalculateBMI_Click;
            // 
            // btnSaveData
            // 


            btnSaveData.Click += new System.EventHandler(this.btnSaveData_Click);

            btnSaveData.BackColor = Color.LightGreen;
            btnSaveData.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnSaveData.Location = new Point(190, 490);
            btnSaveData.Name = "btnSaveData";
            btnSaveData.Size = new Size(150, 40);
            btnSaveData.TabIndex = 5;
            btnSaveData.Text = "Сохранить данные";
            btnSaveData.UseVisualStyleBackColor = false;
            btnSaveData.Click += btnSaveData_Click;
            // 
            // btnClear
            // 
            btnClear.Click += new System.EventHandler(this.btnClear_Click);

            btnClear.BackColor = Color.LightCoral;
            btnClear.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnClear.Location = new Point(360, 490);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(150, 40);
            btnClear.TabIndex = 6;
            btnClear.Text = "Очистить форму";
            btnClear.UseVisualStyleBackColor = false;
            btnClear.Click += btnClear_Click;
            // 
            // btnShowSummary
            // 
            btnShowSummary.Click += new System.EventHandler(this.btnShowSummary_Click);

            btnShowSummary.BackColor = Color.LightYellow;
            btnShowSummary.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnShowSummary.Location = new Point(530, 490);
            btnShowSummary.Name = "btnShowSummary";
            btnShowSummary.Size = new Size(150, 40);
            btnShowSummary.TabIndex = 7;
            btnShowSummary.Text = "Показать сводку";
            btnShowSummary.UseVisualStyleBackColor = false;
            btnShowSummary.Click += btnShowSummary_Click;
            // 
            // lstSummary
            // 
            lstSummary.Font = new Font("Courier New", 9F);
            lstSummary.FormattingEnabled = true;
            lstSummary.ItemHeight = 15;
            lstSummary.Location = new Point(390, 290);
            lstSummary.Name = "lstSummary";
            lstSummary.Size = new Size(380, 184);
            lstSummary.TabIndex = 8;
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, toolsToolStripMenuItem, helpToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(800, 24);
            menuStrip1.TabIndex = 9;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(48, 20);
            fileToolStripMenuItem.Text = "Файл";
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(108, 22);
            exitToolStripMenuItem.Text = "Выход";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // toolsToolStripMenuItem
            // 
            toolsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { historyToolStripMenuItem, calorieCalculatorToolStripMenuItem, workoutPlanToolStripMenuItem, settingsToolStripMenuItem });
            toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            toolsToolStripMenuItem.Size = new Size(95, 20);
            toolsToolStripMenuItem.Text = "Инструменты";
            // 
            // historyToolStripMenuItem
            // 
            historyToolStripMenuItem.Name = "historyToolStripMenuItem";
            historyToolStripMenuItem.Size = new Size(194, 22);
            historyToolStripMenuItem.Text = "История и статистика";
            historyToolStripMenuItem.Click += historyToolStripMenuItem_Click;
            // 
            // calorieCalculatorToolStripMenuItem
            // 
            calorieCalculatorToolStripMenuItem.Name = "calorieCalculatorToolStripMenuItem";
            calorieCalculatorToolStripMenuItem.Size = new Size(194, 22);
            calorieCalculatorToolStripMenuItem.Text = "Калькулятор калорий";
            calorieCalculatorToolStripMenuItem.Click += calorieCalculatorToolStripMenuItem_Click;
            // 
            // workoutPlanToolStripMenuItem
            // 
            workoutPlanToolStripMenuItem.Name = "workoutPlanToolStripMenuItem";
            workoutPlanToolStripMenuItem.Size = new Size(194, 22);
            workoutPlanToolStripMenuItem.Text = "План тренировок";
            workoutPlanToolStripMenuItem.Click += workoutPlanToolStripMenuItem_Click;
            // 
            // settingsToolStripMenuItem
            // 
            settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            settingsToolStripMenuItem.Size = new Size(194, 22);
            settingsToolStripMenuItem.Text = "Настройки и цели";
            settingsToolStripMenuItem.Click += settingsToolStripMenuItem_Click;
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { aboutToolStripMenuItem });
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new Size(68, 20);
            helpToolStripMenuItem.Text = "Помощь";
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new Size(149, 22);
            aboutToolStripMenuItem.Text = "О программе";
            aboutToolStripMenuItem.Click += aboutToolStripMenuItem_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 550);
            Controls.Add(grpResults);
            Controls.Add(grpDailyTracking);
            Controls.Add(grpUserInfo);
            Controls.Add(lblTitle);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Программа отслеживания здоровья";
            grpUserInfo.ResumeLayout(false);
            grpUserInfo.PerformLayout();
            grpDailyTracking.ResumeLayout(false);
            grpDailyTracking.PerformLayout();
            grpResults.ResumeLayout(false);
            grpResults.PerformLayout();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.GroupBox grpUserInfo;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtAge;
        private System.Windows.Forms.TextBox txtHeight;
        private System.Windows.Forms.TextBox txtWeight;
        private System.Windows.Forms.ComboBox cmbGender;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblAge;
        private System.Windows.Forms.Label lblGender;
        private System.Windows.Forms.Label lblHeight;
        private System.Windows.Forms.Label lblWeight;
        private System.Windows.Forms.GroupBox grpDailyTracking;
        private System.Windows.Forms.TextBox txtWater;
        private System.Windows.Forms.TextBox txtSleep;
        private System.Windows.Forms.ComboBox cmbActivity;
        private System.Windows.Forms.CheckBox chkBreakfast;
        private System.Windows.Forms.CheckBox chkLunch;
        private System.Windows.Forms.CheckBox chkDinner;
        private System.Windows.Forms.Label lblWater;
        private System.Windows.Forms.Label lblSleep;
        private System.Windows.Forms.Label lblActivity;
        private System.Windows.Forms.Label lblMeals;
        private System.Windows.Forms.GroupBox grpResults;
        private System.Windows.Forms.Label lblBMIResult;
        private System.Windows.Forms.Label lblHealthFeedback;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.Button btnCalculateBMI;
        private System.Windows.Forms.Button btnSaveData;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnShowSummary;
        private System.Windows.Forms.ListBox lstSummary;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem historyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem calorieCalculatorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem workoutPlanToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
    }
}