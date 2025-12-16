using System;
using System.Drawing;
using System.Windows.Forms;

namespace dailyhealth
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
            LoadCurrentGoals();
        }

        private void LoadCurrentGoals()
        {
            txtWaterGoal.Text = Form1.Goals.WaterGoal.ToString();
            txtSleepGoal.Text = Form1.Goals.SleepGoal.ToString("F1");
            txtWeightGoal.Text = Form1.Goals.WeightGoal.ToString("F1");
            txtCaloriesGoal.Text = Form1.Goals.CaloriesGoal.ToString();
            txtStepsGoal.Text = Form1.Goals.StepsGoal.ToString();

            UpdateGoalsDisplay();
        }

        private void UpdateGoalsDisplay()
        {
            txtCurrentGoals.Clear();
            txtCurrentGoals.AppendText("===== CURRENT GOALS =====\r\n\r\n");
            txtCurrentGoals.AppendText($"💧 Water: {Form1.Goals.WaterGoal} glasses/day\r\n");
            txtCurrentGoals.AppendText($"😴 Sleep: {Form1.Goals.SleepGoal:F1} hours/day\r\n");
            txtCurrentGoals.AppendText($"⚖️ Target weight: {Form1.Goals.WeightGoal:F1} kg\r\n");
            txtCurrentGoals.AppendText($"🍽️ Calories: {Form1.Goals.CaloriesGoal} kcal/day\r\n");
            txtCurrentGoals.AppendText($"👟 Steps: {Form1.Goals.StepsGoal} steps/day\r\n");

            if (Form1.AllRecords.Count > 0)
            {
                var lastRecord = Form1.AllRecords[Form1.AllRecords.Count - 1];

                txtCurrentGoals.AppendText("\r\n===== LAST RECORDS =====\r\n\r\n");
                txtCurrentGoals.AppendText($"Water: {lastRecord.WaterGlasses}/{Form1.Goals.WaterGoal} glasses ");
                txtCurrentGoals.AppendText($"({(lastRecord.WaterGlasses * 100.0 / Form1.Goals.WaterGoal):F0}%)\r\n");

                txtCurrentGoals.AppendText($"Sleep: {lastRecord.SleepHours:F1}/{Form1.Goals.SleepGoal:F1} hours ");
                txtCurrentGoals.AppendText($"({(lastRecord.SleepHours * 100.0 / Form1.Goals.SleepGoal):F0}%)\r\n");

                txtCurrentGoals.AppendText($"Weight: {lastRecord.Weight:F1} kg ");
                double weightDiff = Form1.Goals.WeightGoal - lastRecord.Weight;
                if (Math.Abs(weightDiff) < 0.5)
                    txtCurrentGoals.AppendText("(Goal achieved! ✓)\r\n");
                else if (weightDiff > 0)
                    txtCurrentGoals.AppendText($"(Need to gain: {weightDiff:F1} kg)\r\n");
                else
                    txtCurrentGoals.AppendText($"(Need to lose: {Math.Abs(weightDiff):F1} kg)\r\n");
            }

            txtCurrentGoals.AppendText("\r\n===== RECOMMENDATIONS =====\r\n\r\n");

            if (Form1.Goals.WaterGoal < 8)
                txtCurrentGoals.AppendText("⚠️ At least 8 glasses of water recommended\r\n");

            if (Form1.Goals.SleepGoal < 7)
                txtCurrentGoals.AppendText("⚠️ At least 7-8 hours of sleep recommended\r\n");
            else if (Form1.Goals.SleepGoal > 9)
                txtCurrentGoals.AppendText("⚠️ Too much sleep can be harmful\r\n");

            if (Form1.Goals.CaloriesGoal < 1200)
                txtCurrentGoals.AppendText("⚠️ Too low calorie intake\r\n");
            else if (Form1.Goals.CaloriesGoal > 3500)
                txtCurrentGoals.AppendText("ℹ️ High intake - suitable for active people\r\n");

            if (Form1.Goals.StepsGoal < 7000)
                txtCurrentGoals.AppendText("ℹ️ At least 7000-10000 steps recommended\r\n");
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                int waterGoal = int.Parse(txtWaterGoal.Text);
                double sleepGoal = double.Parse(txtSleepGoal.Text);
                double weightGoal = double.Parse(txtWeightGoal.Text);
                int caloriesGoal = int.Parse(txtCaloriesGoal.Text);
                int stepsGoal = int.Parse(txtStepsGoal.Text);

                if (waterGoal < 1 || waterGoal > 20)
                {
                    MessageBox.Show("Water goal should be between 1 and 20 glasses!",
                        "Invalid value", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (sleepGoal < 4 || sleepGoal > 12)
                {
                    MessageBox.Show("Sleep goal should be between 4 and 12 hours!",
                        "Invalid value", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (weightGoal < 30 || weightGoal > 200)
                {
                    MessageBox.Show("Target weight should be between 30 and 200 kg!",
                        "Invalid value", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (caloriesGoal < 800 || caloriesGoal > 5000)
                {
                    MessageBox.Show("Calories should be between 800 and 5000 kcal!",
                        "Invalid value", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (stepsGoal < 1000 || stepsGoal > 50000)
                {
                    MessageBox.Show("Step goal should be between 1000 and 50000!",
                        "Invalid value", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Form1.Goals.WaterGoal = waterGoal;
                Form1.Goals.SleepGoal = sleepGoal;
                Form1.Goals.WeightGoal = weightGoal;
                Form1.Goals.CaloriesGoal = caloriesGoal;
                Form1.Goals.StepsGoal = stepsGoal;

                UpdateGoalsDisplay();

                MessageBox.Show("Goals saved successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
            }
            catch (FormatException)
            {
                MessageBox.Show("Please enter valid numerical values!", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Reset all goals to default values?",
                "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Form1.Goals.WaterGoal = 8;
                Form1.Goals.SleepGoal = 8;
                Form1.Goals.WeightGoal = 70;
                Form1.Goals.CaloriesGoal = 2000;
                Form1.Goals.StepsGoal = 10000;

                LoadCurrentGoals();

                MessageBox.Show("Goals have been reset to default values!", "Reset",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCalculateIdealWeight_Click(object sender, EventArgs e)
        {
            string info = "===== IDEAL WEIGHT FORMULAS =====\r\n\r\n";
            info += "Enter your height in the field below, and the program\r\n";
            info += "will calculate your ideal weight using different formulas:\r\n\r\n";
            info += "• Devine Formula\r\n";
            info += "• Robinson Formula\r\n";
            info += "• Miller Formula\r\n";
            info += "• BMI Index (18.5-24.9)\r\n";

            string input = Microsoft.VisualBasic.Interaction.InputBox(
                info + "\r\nEnter your height in cm:",
                "Ideal Weight Calculator",
                "170");

            if (!string.IsNullOrEmpty(input))
            {
                try
                {
                    double height = double.Parse(input);
                    if (height < 120 || height > 220)
                    {
                        MessageBox.Show("Height should be between 120 and 220 cm!",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string results = "===== IDEAL WEIGHT CALCULATION =====\r\n\r\n";
                    results += $"Height: {height} cm\r\n\r\n";

                    double heightInInches = height / 2.54;
                    double devineMale = 50 + 2.3 * (heightInInches - 60);
                    double devineFemale = 45.5 + 2.3 * (heightInInches - 60);

                    results += $"Devine Formula:\r\n";
                    results += $"  Men: {devineMale:F1} kg\r\n";
                    results += $"  Women: {devineFemale:F1} kg\r\n\r\n";

                    double heightInMeters = height / 100.0;
                    double minWeight = 18.5 * heightInMeters * heightInMeters;
                    double maxWeight = 24.9 * heightInMeters * heightInMeters;

                    results += $"Healthy range (BMI 18.5-24.9):\r\n";
                    results += $"  From {minWeight:F1} to {maxWeight:F1} kg\r\n\r\n";

                    results += $"Recommended target weight:\r\n";
                    double recommendedWeight = (minWeight + maxWeight) / 2;
                    results += $"  {recommendedWeight:F1} kg (middle of the range)\r\n";

                    if (MessageBox.Show(results + "\r\n\r\nSet the recommended weight as goal?",
                        "Results", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        txtWeightGoal.Text = recommendedWeight.ToString("F1");
                    }
                }
                catch
                {
                    MessageBox.Show("Please enter a valid numerical value!",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void txtWaterGoal_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtWaterGoal.Text))
            {
                try
                {
                    int value = int.Parse(txtWaterGoal.Text);
                    if (value < 6)
                        lblWaterHint.Text = "⚠️ Too low";
                    else if (value >= 8 && value <= 12)
                        lblWaterHint.Text = "✓ Perfect";
                    else if (value > 12)
                        lblWaterHint.Text = "ℹ️ Too much";
                    else
                        lblWaterHint.Text = "ℹ️ Normal";
                }
                catch { lblWaterHint.Text = ""; }
            }
        }

        private void txtSleepGoal_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtSleepGoal.Text))
            {
                try
                {
                    double value = double.Parse(txtSleepGoal.Text);
                    if (value < 6)
                        lblSleepHint.Text = "⚠️ Too little";
                    else if (value >= 7 && value <= 9)
                        lblSleepHint.Text = "✓ Perfect";
                    else if (value > 9)
                        lblSleepHint.Text = "⚠️ Too much";
                    else
                        lblSleepHint.Text = "ℹ️ Normal";
                }
                catch { lblSleepHint.Text = ""; }
            }
        }
    }
}
