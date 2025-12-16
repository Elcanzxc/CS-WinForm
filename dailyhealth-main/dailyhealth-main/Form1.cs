using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using WinFormsTimer = System.Windows.Forms.Timer;

namespace dailyhealth
{
    public partial class Form1 : Form
    {
        public static List<HealthRecord> AllRecords = new List<HealthRecord>();
        public static UserGoals Goals = new UserGoals();

        private Panel selectedPanel = null;

        public Form1()
        {
            InitializeComponent();
            InitializeDefaultGoals();
            ApplyModernDesign();
        }

        private void InitializeDefaultGoals()
        {
            if (Goals.WaterGoal == 0)
            {
                Goals.WaterGoal = 8;
                Goals.SleepGoal = 8;
                Goals.WeightGoal = 70;
                Goals.CaloriesGoal = 2000;
                Goals.StepsGoal = 10000;
            }
        }

        private void ApplyModernDesign()
        {
            this.BackColor = ColorTranslator.FromHtml("#F0F4F8");

            this.Opacity = 0;
            WinFormsTimer fadeInTimer = new WinFormsTimer();
            fadeInTimer.Interval = 20;
            fadeInTimer.Tick += (s, e) =>
            {
                if (this.Opacity < 1)
                    this.Opacity += 0.05;
                else
                    fadeInTimer.Stop();
            };
            fadeInTimer.Start();
        }

        private void UpdateStatistics()
        {
            if (AllRecords.Count > 0)
            {
                var lastRecord = AllRecords[AllRecords.Count - 1];
                lblLastUpdate.Text = $"Last update: {lastRecord.Date:MM/dd/yyyy HH:mm}";
                lblTotalRecords.Text = $"Total records: {AllRecords.Count}";
                lblLastScore.Text = $"Last score: {lastRecord.HealthScore}%";
            }
            else
            {
                lblLastUpdate.Text = "No data";
                lblTotalRecords.Text = "Records: 0";
                lblLastScore.Text = "Score: -";
            }
        }

        private void pnlHistory_Click(object sender, EventArgs e)
        {
            HighlightPanel(pnlHistory);
            Form2 historyForm = new Form2();
            historyForm.ShowDialog();
            UpdateStatistics();
        }

        private void pnlCalorie_Click(object sender, EventArgs e)
        {
            HighlightPanel(pnlCalorie);
            Form3 calorieForm = new Form3();
            calorieForm.ShowDialog();
        }

        private void pnlWorkout_Click(object sender, EventArgs e)
        {
            HighlightPanel(pnlWorkout);
            Form4 workoutForm = new Form4();
            workoutForm.ShowDialog();
        }

        private void pnlSettings_Click(object sender, EventArgs e)
        {
            HighlightPanel(pnlSettings);
            Form5 settingsForm = new Form5();
            if (settingsForm.ShowDialog() == DialogResult.OK)
            {
                UpdateStatistics();
            }
        }

        private void HighlightPanel(Panel panel)
        {
            panel.BackColor = ColorTranslator.FromHtml("#2C5282");
            WinFormsTimer resetTimer = new WinFormsTimer();
            resetTimer.Interval = 200;
            resetTimer.Tick += (s, e) =>
            {
                panel.BackColor = ColorTranslator.FromHtml("#3182CE");
                resetTimer.Stop();
            };
            resetTimer.Start();
        }

        private void AddHoverEffect(Panel panel)
        {
            Color originalColor = panel.BackColor;
            Color hoverColor = ColorTranslator.FromHtml("#2B6CB0");

            panel.MouseEnter += (s, e) =>
            {
                panel.BackColor = hoverColor;
                panel.Cursor = Cursors.Hand;
            };

            panel.MouseLeave += (s, e) =>
            {
                if (panel != selectedPanel)
                    panel.BackColor = originalColor;
            };

            foreach (Control ctrl in panel.Controls)
            {
                ctrl.MouseEnter += (s, e) =>
                {
                    panel.BackColor = hoverColor;
                    ctrl.Cursor = Cursors.Hand;
                };

                ctrl.MouseLeave += (s, e) =>
                {
                    if (panel != selectedPanel)
                        panel.BackColor = originalColor;
                };

                ctrl.Click += (s, e) =>
                {
                    panel_Click(panel, e);
                };
            }
        }

        private void panel_Click(Panel panel, EventArgs e)
        {
            if (panel == pnlHistory)
                pnlHistory_Click(panel, e);
            else if (panel == pnlCalorie)
                pnlCalorie_Click(panel, e);
            else if (panel == pnlWorkout)
                pnlWorkout_Click(panel, e);
            else if (panel == pnlSettings)
                pnlSettings_Click(panel, e);
        }

        private void btnQuickStart_Click(object sender, EventArgs e)
        {
            HighlightPanel(pnlHistory);
            Form2 historyForm = new Form2();
            historyForm.ShowDialog();
            UpdateStatistics();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string aboutText = "╔═══════════════════════════════════╗\n" +
                               "║    DAILY HEALTH TRACKER v2.0      ║\n" +
                               "╚═══════════════════════════════════╝\n\n" +
                               "🏥 Comprehensive tracking system for\n" +
                               "   your health and fitness\n\n" +
                               "📊 FEATURES:\n" +
                               "  • BMI Calculation & Monitoring\n" +
                               "  • Water and Sleep Tracking\n" +
                               "  • History and Statistics\n" +
                               "  • Calorie Calculator\n" +
                               "  • Workout Plans\n" +
                               "  • Personal Goals\n" +
                               "  • Data Export\n\n" +
                               "💡 TIP: Update your data regularly\n" +
                               "   for accurate statistics!\n\n" +
                               "© 2024 Daily Health Program\n" +
                               "Version 2.0 - Updated Design";

            MessageBox.Show(aboutText, "About Program",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to exit the program?",
                "Exit Confirmation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes)
            {
                WinFormsTimer fadeOutTimer = new WinFormsTimer();
                fadeOutTimer.Interval = 20;
                fadeOutTimer.Tick += (s, ev) =>
                {
                    if (this.Opacity > 0)
                        this.Opacity -= 0.1;
                    else
                    {
                        fadeOutTimer.Stop();
                        Application.Exit();
                    }
                };
                fadeOutTimer.Start();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            UpdateStatistics();

            AddHoverEffect(pnlHistory);
            AddHoverEffect(pnlCalorie);
            AddHoverEffect(pnlWorkout);
            AddHoverEffect(pnlSettings);
        }

        private void lblCalorieIcon_Click(object sender, EventArgs e) { }
    }

    public class HealthRecord
    {
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public double BMI { get; set; }
        public string BMICategory { get; set; }
        public int WaterGlasses { get; set; }
        public double SleepHours { get; set; }
        public string ActivityLevel { get; set; }
        public bool HadBreakfast { get; set; }
        public bool HadLunch { get; set; }
        public bool HadDinner { get; set; }
        public int HealthScore { get; set; }
    }

    public class UserGoals
    {
        public int WaterGoal { get; set; } = 8;
        public double SleepGoal { get; set; } = 8;
        public double WeightGoal { get; set; } = 70;
        public int CaloriesGoal { get; set; } = 2000;
        public int StepsGoal { get; set; } = 10000;
    }
}