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

        private void InitializeComponent()
        {
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            helpToolStripMenuItem = new ToolStripMenuItem();
            aboutToolStripMenuItem = new ToolStripMenuItem();
            lblTitle = new Label();
            lblSubtitle = new Label();
            lblWelcome = new Label();
            pnlHistory = new Panel();
            lblHistoryIcon = new Label();
            lblHistoryTitle = new Label();
            lblHistoryDesc = new Label();
            pnlCalorie = new Panel();
            lblCalorieIcon = new Label();
            lblCalorieTitle = new Label();
            lblCalorieDesc = new Label();
            pnlWorkout = new Panel();
            lblWorkoutIcon = new Label();
            lblWorkoutTitle = new Label();
            lblWorkoutDesc = new Label();
            pnlSettings = new Panel();
            lblSettingsIcon = new Label();
            lblSettingsTitle = new Label();
            lblSettingsDesc = new Label();
            pnlStats = new Panel();
            lblStatsHeader = new Label();
            lblTotalRecords = new Label();
            lblLastUpdate = new Label();
            lblLastScore = new Label();
            btnQuickStart = new Button();
            lblFooter = new Label();
            menuStrip1.SuspendLayout();
            pnlHistory.SuspendLayout();
            pnlCalorie.SuspendLayout();
            pnlWorkout.SuspendLayout();
            pnlSettings.SuspendLayout();
            pnlStats.SuspendLayout();
            SuspendLayout();

            // 
            // menuStrip1
            // 
            menuStrip1.BackColor = Color.White;
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, helpToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1000, 24);
            menuStrip1.TabIndex = 0;

            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";

            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(93, 22);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;

            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { aboutToolStripMenuItem });
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new Size(44, 20);
            helpToolStripMenuItem.Text = "Help";

            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new Size(107, 22);
            aboutToolStripMenuItem.Text = "About";
            aboutToolStripMenuItem.Click += aboutToolStripMenuItem_Click;

            // 
            // lblTitle
            // 
            lblTitle.Font = new Font("Segoe UI", 32F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(26, 54, 93);
            lblTitle.Location = new Point(50, 50);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(900, 60);
            lblTitle.TabIndex = 1;
            lblTitle.Text = "🏥 Daily Health Tracker";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;

            // 
            // lblSubtitle
            // 
            lblSubtitle.Font = new Font("Segoe UI", 12F);
            lblSubtitle.ForeColor = Color.FromArgb(74, 85, 104);
            lblSubtitle.Location = new Point(50, 110);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Size = new Size(900, 30);
            lblSubtitle.TabIndex = 2;
            lblSubtitle.Text = "Your personal assistant for a healthy lifestyle";
            lblSubtitle.TextAlign = ContentAlignment.MiddleCenter;

            // 
            // lblWelcome
            // 
            lblWelcome.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblWelcome.ForeColor = Color.FromArgb(45, 55, 72);
            lblWelcome.Location = new Point(50, 170);
            lblWelcome.Name = "lblWelcome";
            lblWelcome.Size = new Size(900, 35);
            lblWelcome.TabIndex = 3;
            lblWelcome.Text = "Select a section to get started";
            lblWelcome.TextAlign = ContentAlignment.MiddleCenter;

            // 
            // pnlHistory
            // 
            pnlHistory.BackColor = Color.FromArgb(49, 130, 206);
            pnlHistory.Controls.Add(lblHistoryIcon);
            pnlHistory.Controls.Add(lblHistoryTitle);
            pnlHistory.Controls.Add(lblHistoryDesc);
            pnlHistory.Cursor = Cursors.Hand;
            pnlHistory.Location = new Point(80, 230);
            pnlHistory.Name = "pnlHistory";
            pnlHistory.Size = new Size(400, 140);
            pnlHistory.TabIndex = 4;
            pnlHistory.Click += pnlHistory_Click;

            // 
            // lblHistoryIcon
            // 
            lblHistoryIcon.Font = new Font("Segoe UI", 36F);
            lblHistoryIcon.ForeColor = Color.White;
            lblHistoryIcon.Location = new Point(20, 20);
            lblHistoryIcon.Name = "lblHistoryIcon";
            lblHistoryIcon.Size = new Size(80, 80);
            lblHistoryIcon.TabIndex = 0;
            lblHistoryIcon.Text = "📊";
            lblHistoryIcon.TextAlign = ContentAlignment.MiddleCenter;

            // 
            // lblHistoryTitle
            // 
            lblHistoryTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblHistoryTitle.ForeColor = Color.White;
            lblHistoryTitle.Location = new Point(110, 30);
            lblHistoryTitle.Name = "lblHistoryTitle";
            lblHistoryTitle.Size = new Size(270, 35);
            lblHistoryTitle.TabIndex = 1;
            lblHistoryTitle.Text = "History & Statistics";

            // 
            // lblHistoryDesc
            // 
            lblHistoryDesc.Font = new Font("Segoe UI", 10F);
            lblHistoryDesc.ForeColor = Color.FromArgb(226, 232, 240);
            lblHistoryDesc.Location = new Point(110, 65);
            lblHistoryDesc.Name = "lblHistoryDesc";
            lblHistoryDesc.Size = new Size(270, 50);
            lblHistoryDesc.TabIndex = 2;
            lblHistoryDesc.Text = "View your record history and analyze statistics";

            // 
            // pnlCalorie
            // 
            pnlCalorie.BackColor = Color.FromArgb(49, 130, 206);
            pnlCalorie.Controls.Add(lblCalorieIcon);
            pnlCalorie.Controls.Add(lblCalorieTitle);
            pnlCalorie.Controls.Add(lblCalorieDesc);
            pnlCalorie.Cursor = Cursors.Hand;
            pnlCalorie.Location = new Point(520, 230);
            pnlCalorie.Name = "pnlCalorie";
            pnlCalorie.Size = new Size(400, 140);
            pnlCalorie.TabIndex = 5;
            pnlCalorie.Click += pnlCalorie_Click;

            // 
            // lblCalorieIcon
            // 
            lblCalorieIcon.Font = new Font("Segoe UI", 36F);
            lblCalorieIcon.ForeColor = Color.White;
            lblCalorieIcon.Location = new Point(20, 20);
            lblCalorieIcon.Name = "lblCalorieIcon";
            lblCalorieIcon.Size = new Size(80, 80);
            lblCalorieIcon.TabIndex = 0;
            lblCalorieIcon.Text = "🍽️";
            lblCalorieIcon.TextAlign = ContentAlignment.MiddleCenter;

            // 
            // lblCalorieTitle
            // 
            lblCalorieTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblCalorieTitle.ForeColor = Color.White;
            lblCalorieTitle.Location = new Point(110, 30);
            lblCalorieTitle.Name = "lblCalorieTitle";
            lblCalorieTitle.Size = new Size(270, 35);
            lblCalorieTitle.TabIndex = 1;
            lblCalorieTitle.Text = "Calorie Calculator";

            // 
            // lblCalorieDesc
            // 
            lblCalorieDesc.Font = new Font("Segoe UI", 10F);
            lblCalorieDesc.ForeColor = Color.FromArgb(226, 232, 240);
            lblCalorieDesc.Location = new Point(110, 65);
            lblCalorieDesc.Name = "lblCalorieDesc";
            lblCalorieDesc.Size = new Size(270, 50);
            lblCalorieDesc.TabIndex = 2;
            lblCalorieDesc.Text = "Calculate your calorie and macronutrient needs";

            // 
            // pnlWorkout
            // 
            pnlWorkout.BackColor = Color.FromArgb(49, 130, 206);
            pnlWorkout.Controls.Add(lblWorkoutIcon);
            pnlWorkout.Controls.Add(lblWorkoutTitle);
            pnlWorkout.Controls.Add(lblWorkoutDesc);
            pnlWorkout.Cursor = Cursors.Hand;
            pnlWorkout.Location = new Point(80, 390);
            pnlWorkout.Name = "pnlWorkout";
            pnlWorkout.Size = new Size(400, 140);
            pnlWorkout.TabIndex = 6;
            pnlWorkout.Click += pnlWorkout_Click;

            // 
            // lblWorkoutIcon
            // 
            lblWorkoutIcon.Font = new Font("Segoe UI", 36F);
            lblWorkoutIcon.ForeColor = Color.White;
            lblWorkoutIcon.Location = new Point(20, 20);
            lblWorkoutIcon.Name = "lblWorkoutIcon";
            lblWorkoutIcon.Size = new Size(80, 80);
            lblWorkoutIcon.TabIndex = 0;
            lblWorkoutIcon.Text = "💪";
            lblWorkoutIcon.TextAlign = ContentAlignment.MiddleCenter;

            // 
            // lblWorkoutTitle
            // 
            lblWorkoutTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblWorkoutTitle.ForeColor = Color.White;
            lblWorkoutTitle.Location = new Point(110, 30);
            lblWorkoutTitle.Name = "lblWorkoutTitle";
            lblWorkoutTitle.Size = new Size(270, 35);
            lblWorkoutTitle.TabIndex = 1;
            lblWorkoutTitle.Text = "Workout Plan";

            // 
            // lblWorkoutDesc
            // 
            lblWorkoutDesc.Font = new Font("Segoe UI", 10F);
            lblWorkoutDesc.ForeColor = Color.FromArgb(226, 232, 240);
            lblWorkoutDesc.Location = new Point(110, 65);
            lblWorkoutDesc.Name = "lblWorkoutDesc";
            lblWorkoutDesc.Size = new Size(270, 50);
            lblWorkoutDesc.TabIndex = 2;
            lblWorkoutDesc.Text = "Create your personalized workout schedule";

            // 
            // pnlSettings
            // 
            pnlSettings.BackColor = Color.FromArgb(49, 130, 206);
            pnlSettings.Controls.Add(lblSettingsIcon);
            pnlSettings.Controls.Add(lblSettingsTitle);
            pnlSettings.Controls.Add(lblSettingsDesc);
            pnlSettings.Cursor = Cursors.Hand;
            pnlSettings.Location = new Point(520, 390);
            pnlSettings.Name = "pnlSettings";
            pnlSettings.Size = new Size(400, 140);
            pnlSettings.TabIndex = 7;
            pnlSettings.Click += pnlSettings_Click;

            // 
            // lblSettingsIcon
            // 
            lblSettingsIcon.Font = new Font("Segoe UI", 36F);
            lblSettingsIcon.ForeColor = Color.White;
            lblSettingsIcon.Location = new Point(20, 20);
            lblSettingsIcon.Name = "lblSettingsIcon";
            lblSettingsIcon.Size = new Size(80, 80);
            lblSettingsIcon.TabIndex = 0;
            lblSettingsIcon.Text = "⚙️";
            lblSettingsIcon.TextAlign = ContentAlignment.MiddleCenter;

            // 
            // lblSettingsTitle
            // 
            lblSettingsTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblSettingsTitle.ForeColor = Color.White;
            lblSettingsTitle.Location = new Point(110, 30);
            lblSettingsTitle.Name = "lblSettingsTitle";
            lblSettingsTitle.Size = new Size(270, 35);
            lblSettingsTitle.TabIndex = 1;
            lblSettingsTitle.Text = "Settings & Goals";

            // 
            // lblSettingsDesc
            // 
            lblSettingsDesc.Font = new Font("Segoe UI", 10F);
            lblSettingsDesc.ForeColor = Color.FromArgb(226, 232, 240);
            lblSettingsDesc.Location = new Point(110, 65);
            lblSettingsDesc.Name = "lblSettingsDesc";
            lblSettingsDesc.Size = new Size(270, 50);
            lblSettingsDesc.TabIndex = 2;
            lblSettingsDesc.Text = "Customize your personal health goals";

            // 
            // pnlStats
            // 
            pnlStats.BackColor = Color.White;
            pnlStats.BorderStyle = BorderStyle.FixedSingle;
            pnlStats.Controls.Add(lblStatsHeader);
            pnlStats.Controls.Add(lblTotalRecords);
            pnlStats.Controls.Add(lblLastUpdate);
            pnlStats.Controls.Add(lblLastScore);
            pnlStats.Location = new Point(80, 550);
            pnlStats.Name = "pnlStats";
            pnlStats.Size = new Size(640, 100);
            pnlStats.TabIndex = 8;

            // 
            // lblStatsHeader
            // 
            lblStatsHeader.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblStatsHeader.ForeColor = Color.FromArgb(45, 55, 72);
            lblStatsHeader.Location = new Point(10, 10);
            lblStatsHeader.Name = "lblStatsHeader";
            lblStatsHeader.Size = new Size(620, 25);
            lblStatsHeader.TabIndex = 0;
            lblStatsHeader.Text = "📈 Quick Statistics";

            // 
            // lblTotalRecords
            // 
            lblTotalRecords.Font = new Font("Segoe UI", 10F);
            lblTotalRecords.ForeColor = Color.FromArgb(74, 85, 104);
            lblTotalRecords.Location = new Point(20, 40);
            lblTotalRecords.Name = "lblTotalRecords";
            lblTotalRecords.Size = new Size(200, 25);
            lblTotalRecords.TabIndex = 1;
            lblTotalRecords.Text = "Records: 0";

            // 
            // lblLastUpdate
            // 
            lblLastUpdate.Font = new Font("Segoe UI", 10F);
            lblLastUpdate.ForeColor = Color.FromArgb(74, 85, 104);
            lblLastUpdate.Location = new Point(220, 40);
            lblLastUpdate.Name = "lblLastUpdate";
            lblLastUpdate.Size = new Size(200, 25);
            lblLastUpdate.TabIndex = 2;
            lblLastUpdate.Text = "No data";

            // 
            // lblLastScore
            // 
            lblLastScore.Font = new Font("Segoe UI", 10F);
            lblLastScore.ForeColor = Color.FromArgb(74, 85, 104);
            lblLastScore.Location = new Point(20, 65);
            lblLastScore.Name = "lblLastScore";
            lblLastScore.Size = new Size(200, 25);
            lblLastScore.TabIndex = 3;
            lblLastScore.Text = "Score: -";

            // 
            // btnQuickStart
            // 
            btnQuickStart.BackColor = Color.FromArgb(72, 187, 120);
            btnQuickStart.Cursor = Cursors.Hand;
            btnQuickStart.FlatAppearance.BorderSize = 0;
            btnQuickStart.FlatStyle = FlatStyle.Flat;
            btnQuickStart.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnQuickStart.ForeColor = Color.White;
            btnQuickStart.Location = new Point(740, 550);
            btnQuickStart.Name = "btnQuickStart";
            btnQuickStart.Size = new Size(180, 100);
            btnQuickStart.TabIndex = 9;
            btnQuickStart.Text = "🚀 Quick\nStart";
            btnQuickStart.UseVisualStyleBackColor = false;
            btnQuickStart.Click += btnQuickStart_Click;

            // 
            // lblFooter
            // 
            lblFooter.Font = new Font("Segoe UI", 9F);
            lblFooter.ForeColor = Color.FromArgb(113, 128, 150);
            lblFooter.Location = new Point(50, 670);
            lblFooter.Name = "lblFooter";
            lblFooter.Size = new Size(900, 30);
            lblFooter.TabIndex = 10;
            lblFooter.Text = "© 2024 Daily Health Tracker v2.0 | Track your health every day";
            lblFooter.TextAlign = ContentAlignment.MiddleCenter;

            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1000, 720);
            Controls.Add(lblFooter);
            Controls.Add(btnQuickStart);
            Controls.Add(pnlStats);
            Controls.Add(pnlSettings);
            Controls.Add(pnlWorkout);
            Controls.Add(pnlCalorie);
            Controls.Add(pnlHistory);
            Controls.Add(lblWelcome);
            Controls.Add(lblSubtitle);
            Controls.Add(lblTitle);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Daily Health Tracker - Main Menu";
            Load += Form1_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            pnlHistory.ResumeLayout(false);
            pnlCalorie.ResumeLayout(false);
            pnlWorkout.ResumeLayout(false);
            pnlSettings.ResumeLayout(false);
            pnlStats.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.Label lblWelcome;

        private System.Windows.Forms.Panel pnlHistory;
        private System.Windows.Forms.Label lblHistoryIcon;
        private System.Windows.Forms.Label lblHistoryTitle;
        private System.Windows.Forms.Label lblHistoryDesc;

        private System.Windows.Forms.Panel pnlCalorie;
        private System.Windows.Forms.Label lblCalorieIcon;
        private System.Windows.Forms.Label lblCalorieTitle;
        private System.Windows.Forms.Label lblCalorieDesc;

        private System.Windows.Forms.Panel pnlWorkout;
        private System.Windows.Forms.Label lblWorkoutIcon;
        private System.Windows.Forms.Label lblWorkoutTitle;
        private System.Windows.Forms.Label lblWorkoutDesc;

        private System.Windows.Forms.Panel pnlSettings;
        private System.Windows.Forms.Label lblSettingsIcon;
        private System.Windows.Forms.Label lblSettingsTitle;
        private System.Windows.Forms.Label lblSettingsDesc;

        private System.Windows.Forms.Panel pnlStats;
        private System.Windows.Forms.Label lblStatsHeader;
        private System.Windows.Forms.Label lblTotalRecords;
        private System.Windows.Forms.Label lblLastUpdate;
        private System.Windows.Forms.Label lblLastScore;

        private System.Windows.Forms.Button btnQuickStart;
        private System.Windows.Forms.Label lblFooter;
    }
}