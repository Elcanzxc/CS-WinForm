using System;
using System.Drawing;
using System.Windows.Forms;

namespace dailyhealth
{
    public partial class Form3 : Form
    {
        private double calculatedCalories = 0;

        public Form3()
        {
            InitializeComponent();
            cmbGender.SelectedIndex = 0;
            cmbActivityLevel.SelectedIndex = 1;
            cmbGoal.SelectedIndex = 1;
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtAge.Text) ||
                    string.IsNullOrWhiteSpace(txtWeight.Text) ||
                    string.IsNullOrWhiteSpace(txtHeight.Text))
                {
                    MessageBox.Show("Please fill in all fields!", "Input Required",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int age = int.Parse(txtAge.Text);
                double weight = double.Parse(txtWeight.Text);
                double height = double.Parse(txtHeight.Text);
                string gender = cmbGender.SelectedItem.ToString();
                string activityLevel = cmbActivityLevel.SelectedItem.ToString();
                string goal = cmbGoal.SelectedItem.ToString();

                double bmr;
                if (gender == "Male")
                {
                    bmr = (10 * weight) + (6.25 * height) - (5 * age) + 5;
                }
                else
                {
                    bmr = (10 * weight) + (6.25 * height) - (5 * age) - 161;
                }

                double activityFactor = 1.2;
                switch (cmbActivityLevel.SelectedIndex)
                {
                    case 0: activityFactor = 1.2; break;
                    case 1: activityFactor = 1.375; break;
                    case 2: activityFactor = 1.55; break;
                    case 3: activityFactor = 1.725; break;
                    case 4: activityFactor = 1.9; break;
                }

                double tdee = bmr * activityFactor;

                calculatedCalories = tdee;
                switch (cmbGoal.SelectedIndex)
                {
                    case 0:
                        calculatedCalories = tdee - 500;
                        break;
                    case 1:
                        calculatedCalories = tdee;
                        break;
                    case 2:
                        calculatedCalories = tdee + 500;
                        break;
                }

                lblBMR.Text = $"Basal Metabolic Rate (BMR): {bmr:F0} kcal/day";
                lblTDEE.Text = $"Total Daily Energy Expenditure (TDEE): {tdee:F0} kcal/day";
                lblRecommended.Text = $"Recommended intake: {calculatedCalories:F0} kcal/day";
                lblRecommended.ForeColor = Color.DarkGreen;

                CalculateMacros();

                Form1.Goals.CaloriesGoal = (int)calculatedCalories;
            }
            catch (FormatException)
            {
                MessageBox.Show("Please enter valid numerical values!", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CalculateMacros()
        {
            if (calculatedCalories == 0) return;

            double proteinPercent = 0.30;
            double carbsPercent = 0.40;
            double fatPercent = 0.30;

            switch (cmbGoal.SelectedIndex)
            {
                case 0:
                    proteinPercent = 0.35;
                    carbsPercent = 0.35;
                    fatPercent = 0.30;
                    break;
                case 2:
                    proteinPercent = 0.30;
                    carbsPercent = 0.45;
                    fatPercent = 0.25;
                    break;
            }

            double proteinCalories = calculatedCalories * proteinPercent;
            double carbsCalories = calculatedCalories * carbsPercent;
            double fatCalories = calculatedCalories * fatPercent;

            double proteinGrams = proteinCalories / 4;
            double carbsGrams = carbsCalories / 4;
            double fatGrams = fatCalories / 9;

            txtMacros.Clear();
            txtMacros.AppendText("===== MACRONUTRIENTS =====\r\n\r\n");
            txtMacros.AppendText($"Protein: {proteinGrams:F0}g ({proteinPercent * 100:F0}%)\r\n");
            txtMacros.AppendText($"  = {proteinCalories:F0} kcal\r\n\r\n");
            txtMacros.AppendText($"Carbohydrates: {carbsGrams:F0}g ({carbsPercent * 100:F0}%)\r\n");
            txtMacros.AppendText($"  = {carbsCalories:F0} kcal\r\n\r\n");
            txtMacros.AppendText($"Fats: {fatGrams:F0}g ({fatPercent * 100:F0}%)\r\n");
            txtMacros.AppendText($"  = {fatCalories:F0} kcal\r\n\r\n");

            txtRecommendations.Clear();
            txtRecommendations.AppendText("===== RECOMMENDATIONS =====\r\n\r\n");

            switch (cmbGoal.SelectedIndex)
            {
                case 0:
                    txtRecommendations.AppendText("For WEIGHT LOSS:\r\n\r\n");
                    txtRecommendations.AppendText("• A calorie deficit of (-500 kcal)\r\n");
                    txtRecommendations.AppendText("• Increased protein intake to preserve muscle mass\r\n");
                    txtRecommendations.AppendText("• Drink 2-3 liters of water daily\r\n");
                    txtRecommendations.AppendText("• Eat 4-5 small meals per day\r\n");
                    txtRecommendations.AppendText("• Avoid fast carbs and fried foods\r\n");
                    txtRecommendations.AppendText("• Recommended weight loss: 0.5-1 kg/week\r\n");
                    break;

                case 1:
                    txtRecommendations.AppendText("For MAINTAINING WEIGHT:\r\n\r\n");
                    txtRecommendations.AppendText("• Balanced diet\r\n");
                    txtRecommendations.AppendText("• Maintain current activity level\r\n");
                    txtRecommendations.AppendText("• Regular meals\r\n");
                    txtRecommendations.AppendText("• Varied diet\r\n");
                    txtRecommendations.AppendText("• Portion control\r\n");
                    break;

                case 2:
                    txtRecommendations.AppendText("For WEIGHT GAIN:\r\n\r\n");
                    txtRecommendations.AppendText("• A calorie surplus (+500 kcal)\r\n");
                    txtRecommendations.AppendText("• Increased carbohydrate intake for energy\r\n");
                    txtRecommendations.AppendText("• Sufficient protein for muscle growth\r\n");
                    txtRecommendations.AppendText("• Eat 5-6 meals a day\r\n");
                    txtRecommendations.AppendText("• Combine with strength training\r\n");
                    txtRecommendations.AppendText("• Recommended weight gain: 0.5-1 kg/week\r\n");
                    break;
            }

            txtRecommendations.AppendText("\r\n===== EXAMPLES OF FOODS =====\r\n\r\n");
            txtRecommendations.AppendText("Protein sources:\r\n");
            txtRecommendations.AppendText("• Chicken, turkey, fish\r\n");
            txtRecommendations.AppendText("• Eggs, cottage cheese\r\n");
            txtRecommendations.AppendText("• Legumes, tofu\r\n\r\n");

            txtRecommendations.AppendText("Carbohydrate sources:\r\n");
            txtRecommendations.AppendText("• Grains (buckwheat, oatmeal, rice)\r\n");
            txtRecommendations.AppendText("• Whole grain bread\r\n");
            txtRecommendations.AppendText("• Vegetables and fruits\r\n\r\n");

            txtRecommendations.AppendText("Fat sources:\r\n");
            txtRecommendations.AppendText("• Nuts and seeds\r\n");
            txtRecommendations.AppendText("• Avocado, olive oil\r\n");
            txtRecommendations.AppendText("• Fatty fish\r\n");
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmbGoal_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (calculatedCalories > 0)
            {
                btnCalculate_Click(sender, e);
            }
        }
    }
}
