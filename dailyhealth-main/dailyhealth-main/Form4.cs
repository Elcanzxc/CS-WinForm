namespace dailyhealth
{
    public partial class Form4 : Form
    {
        private Dictionary<string, List<string>> workoutPlans = new Dictionary<string, List<string>>();

        public Form4()
        {
            InitializeComponent();
            InitializeWorkoutPlans();
            cmbLevel.SelectedIndex = 1;
            cmbGoal.SelectedIndex = 0;
        }

        private void InitializeWorkoutPlans()
        {
            workoutPlans["beginner_weight_loss"] = new List<string>
            {
                "MONDAY - Cardio",
                "• Brisk walking - 30 minutes",
                "• Light jumping in place - 2x15",
                "• Stretching - 10 minutes",
                "",
                "WEDNESDAY - Strength training",
                "• Squats - 3x10",
                "• Knee push-ups - 3x8",
                "• Plank - 3x20 seconds",
                "• Lunges - 3x10 per leg",
                "",
                "FRIDAY - Combined",
                "• Jogging - 20 minutes",
                "• Jump squats - 3x10",
                "• Crunches - 3x15",
                "• Stretching - 10 minutes"
            };

            workoutPlans["intermediate_weight_loss"] = new List<string>
            {
                "MONDAY - Interval cardio",
                "• Running 2 min / Walking 1 min - 30 minutes",
                "• Jump rope - 3x1 minute",
                "• Burpees - 3x10",
                "",
                "TUESDAY - Strength (upper body)",
                "• Push-ups - 4x12",
                "• Pull-ups - 4x8",
                "• Dumbbell press - 4x12",
                "• Plank - 3x45 seconds",
                "",
                "THURSDAY - Strength (lower body)",
                "• Squats - 4x15",
                "• Dumbbell lunges - 4x12",
                "• Deadlift - 4x12",
                "• Calf raises - 3x20",
                "",
                "SATURDAY - HIIT workout",
                "• Burpees - 4x15",
                "• Jumping lunges - 4x12",
                "• Mountain climbers - 4x20",
                "• Rest between exercises 30 sec"
            };

            workoutPlans["beginner_muscle"] = new List<string>
            {
                "MONDAY - Chest and triceps",
                "• Bench press - 4x10",
                "• Push-ups - 3x12",
                "• Dumbbell flyes - 3x12",
                "• French press - 3x12",
                "",
                "WEDNESDAY - Back and biceps",
                "• Pull-ups - 4x8",
                "• Barbell row - 4x10",
                "• Lat pulldown - 3x12",
                "• Dumbbell curls - 3x12",
                "",
                "FRIDAY - Legs and shoulders",
                "• Barbell squats - 4x10",
                "• Leg press - 3x12",
                "• Dumbbell press - 4x10",
                "• Dumbbell lateral raises - 3x12"
            };

            workoutPlans["intermediate_muscle"] = new List<string>
            {
                "MONDAY - Chest",
                "• Bench press - 4x8-10",
                "• Incline dumbbell press - 4x10",
                "• Dips - 3x12",
                "• Dumbbell flyes - 3x12",
                "• Cable crossovers - 3x15",
                "",
                "TUESDAY - Back",
                "• Deadlift - 4x6",
                "• Wide-grip pull-ups - 4x10",
                "• Bent-over barbell row - 4x10",
                "• T-bar row - 3x12",
                "• Shrugs - 3x15",
                "",
                "THURSDAY - Legs",
                "• Squats - 5x8",
                "• Leg press - 4x12",
                "• Romanian deadlift - 4x10",
                "• Leg curls - 3x15",
                "• Leg extensions - 3x15",
                "",
                "FRIDAY - Shoulders and arms",
                "• Barbell overhead press - 4x10",
                "• Dumbbell lateral raises - 4x12",
                "• Barbell curls - 4x10",
                "• French press - 4x10",
                "• Hammer curls - 3x12"
            };

            workoutPlans["beginner_maintenance"] = new List<string>
            {
                "MONDAY - Full body",
                "• Squats - 3x12",
                "• Push-ups - 3x10",
                "• Dumbbell row - 3x12",
                "• Plank - 3x30 seconds",
                "",
                "THURSDAY - Cardio and toning",
                "• Running/Cycling - 30 minutes",
                "• Lunges - 3x10",
                "• Crunches - 3x15",
                "• Stretching - 10 minutes"
            };

            workoutPlans["intermediate_maintenance"] = new List<string>
            {
                "MONDAY/THURSDAY - Strength",
                "• Squats - 4x12",
                "• Bench press/Push-ups - 4x12",
                "• Pull-ups/Rows - 4x10",
                "• Dumbbell shoulder press - 3x12",
                "• Plank - 3x1 minute",
                "",
                "TUESDAY/SATURDAY - Cardio",
                "• Running/Swimming - 40 minutes",
                "• High-intensity intervals - 10 minutes",
                "• Stretching - 15 minutes"
            };
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            string level = cmbLevel.SelectedItem.ToString();
            string goal = cmbGoal.SelectedItem.ToString();

            string levelKey = level == "Beginner" ? "beginner" : "intermediate";
            string goalKey = "";

            if (goal == "Weight Loss")
                goalKey = "weight_loss";
            else if (goal == "Muscle Gain")
                goalKey = "muscle";
            else
                goalKey = "maintenance";

            string planKey = $"{levelKey}_{goalKey}";

            txtWorkoutPlan.Clear();

            if (workoutPlans.ContainsKey(planKey))
            {
                txtWorkoutPlan.AppendText($"===== WORKOUT PLAN =====\r\n");
                txtWorkoutPlan.AppendText($"Level: {level}\r\n");
                txtWorkoutPlan.AppendText($"Goal: {goal}\r\n");
                txtWorkoutPlan.AppendText($"==============================\r\n\r\n");

                foreach (string line in workoutPlans[planKey])
                {
                    txtWorkoutPlan.AppendText(line + "\r\n");
                }

                txtWorkoutPlan.AppendText("\r\n===== IMPORTANT RECOMMENDATIONS =====\r\n\r\n");
                txtWorkoutPlan.AppendText("• Warm-up 5-10 minutes before each workout\r\n");
                txtWorkoutPlan.AppendText("• Rest between sets: 60-90 seconds\r\n");
                txtWorkoutPlan.AppendText("• Rest between workouts: at least 1 day\r\n");
                txtWorkoutPlan.AppendText("• Drink water during workouts\r\n");
                txtWorkoutPlan.AppendText("• Maintain proper form\r\n");
                txtWorkoutPlan.AppendText("• Gradually increase intensity\r\n");

                if (goal == "Muscle Gain")
                {
                    txtWorkoutPlan.AppendText("\r\nFOR MUSCLE GROWTH:\r\n");
                    txtWorkoutPlan.AppendText("• Calorie surplus diet\r\n");
                    txtWorkoutPlan.AppendText("• 1.6-2.2g of protein per kg of body weight\r\n");
                    txtWorkoutPlan.AppendText("• Progressive overload\r\n");
                    txtWorkoutPlan.AppendText("• 8-9 hours of sleep\r\n");
                }
                else if (goal == "Weight Loss")
                {
                    txtWorkoutPlan.AppendText("\r\nFOR FAT LOSS:\r\n");
                    txtWorkoutPlan.AppendText("• 300-500 kcal calorie deficit\r\n");
                    txtWorkoutPlan.AppendText("• High-intensity cardio 3-4 times/week\r\n");
                    txtWorkoutPlan.AppendText("• Preserve muscle mass with strength training\r\n");
                    txtWorkoutPlan.AppendText("• Nutrition control is more important than workouts\r\n");
                }
            }

            GenerateExerciseTips();
        }

        private void GenerateExerciseTips()
        {
            txtTips.Clear();
            txtTips.AppendText("===== EXERCISE TIPS =====\r\n\r\n");

            txtTips.AppendText("SQUATS:\r\n");
            txtTips.AppendText("• Keep your back straight, look ahead\r\n");
            txtTips.AppendText("• Knees should not go past toes\r\n");
            txtTips.AppendText("• Lower until thighs are parallel to the floor\r\n\r\n");

            txtTips.AppendText("PUSH-UPS:\r\n");
            txtTips.AppendText("• Keep your body in a straight line\r\n");
            txtTips.AppendText("• Elbows at 45° angle\r\n");
            txtTips.AppendText("• Full range of motion\r\n\r\n");

            txtTips.AppendText("PLANK:\r\n");
            txtTips.AppendText("• Do not arch your back\r\n");
            txtTips.AppendText("• Keep your core tight\r\n");
            txtTips.AppendText("• Breathe steadily\r\n\r\n");

            txtTips.AppendText("LUNGES:\r\n");
            txtTips.AppendText("• Front knee should be aligned with the foot\r\n");
            txtTips.AppendText("• Back knee nearly touches the floor\r\n");
            txtTips.AppendText("• Keep your torso upright\r\n\r\n");

            txtTips.AppendText("===== INJURY PREVENTION =====\r\n\r\n");
            txtTips.AppendText("✓ Always warm-up\r\n");
            txtTips.AppendText("✓ Gradually increase intensity\r\n");
            txtTips.AppendText("✓ Listen to your body\r\n");
            txtTips.AppendText("✓ Rest and recover properly\r\n");
            txtTips.AppendText("✓ Stop if you feel pain\r\n");
        }

        private void btnSavePlan_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtWorkoutPlan.Text))
            {
                MessageBox.Show("Please generate a workout plan first!", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Text files (*.txt)|*.txt";
            saveDialog.FileName = $"workout_plan_{DateTime.Now:yyyyMMdd}.txt";

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    System.IO.File.WriteAllText(saveDialog.FileName, txtWorkoutPlan.Text);
                    MessageBox.Show("Plan saved successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving plan: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
