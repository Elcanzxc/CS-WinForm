using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.DataFormats;

namespace dailyhealth
{
    public partial class Form1 : Form
    {
        private string userName = "";
        private int userAge = 0;
        private string userGender = "";
        private double userHeight = 0.0;
        private double userWeight = 0.0;

        private int waterGlasses = 0;
        private double sleepHours = 0.0;
        private string activityLevel = "";
        private bool hadBreakfast = false;
        private bool hadLunch = false;
        private bool hadDinner = false;

        private double bmiValue = 0.0;
        private string bmiCategory = "";

        // Статические данные для обмена между формами
        public static List<HealthRecord> AllRecords = new List<HealthRecord>();
        public static UserGoals Goals = new UserGoals();

        public Form1()
        {
            InitializeComponent();
            cmbGender.SelectedIndex = 0;
            cmbActivity.SelectedIndex = 0;

            // Инициализация целей по умолчанию
            if (Goals.WaterGoal == 0)
            {
                Goals.WaterGoal = 8;
                Goals.SleepGoal = 8;
                Goals.WeightGoal = 70;
                Goals.CaloriesGoal = 2000;
            }
        }

        private void btnCalculateBMI_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtHeight.Text) || string.IsNullOrWhiteSpace(txtWeight.Text))
                {
                    MessageBox.Show("Пожалуйста, введите рост и вес!", "Требуется ввод",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                userHeight = double.Parse(txtHeight.Text);
                userWeight = double.Parse(txtWeight.Text);

                if (userHeight <= 0 || userWeight <= 0)
                {
                    MessageBox.Show("Рост и вес должны быть положительными!", "Неверный ввод",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                double heightInMeters = userHeight / 100.0;
                bmiValue = userWeight / (heightInMeters * heightInMeters);

                if (bmiValue < 18.5)
                {
                    bmiCategory = "Недостаточный вес";
                    lblBMIResult.ForeColor = Color.Orange;
                }
                else if (bmiValue >= 18.5 && bmiValue < 25)
                {
                    bmiCategory = "Нормальный вес";
                    lblBMIResult.ForeColor = Color.Green;
                }
                else if (bmiValue >= 25 && bmiValue < 30)
                {
                    bmiCategory = "Избыточный вес";
                    lblBMIResult.ForeColor = Color.Orange;
                }
                else
                {
                    bmiCategory = "Ожирение";
                    lblBMIResult.ForeColor = Color.Red;
                }

                lblBMIResult.Text = $"ИМТ: {bmiValue:F2} ({bmiCategory})";
                GenerateHealthFeedback();
            }
            catch (FormatException)
            {
                MessageBox.Show("Введите корректные числовые значения!",
                    "Неверный формат", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GenerateHealthFeedback()
        {
            string feedback = "";

            if (bmiCategory == "Недостаточный вес")
                feedback += "Рекомендуется консультация диетолога. ";
            else if (bmiCategory == "Нормальный вес")
                feedback += "Ваш ИМТ в норме! ";
            else if (bmiCategory == "Избыточный вес")
                feedback += "Увеличьте физическую активность. ";
            else if (bmiCategory == "Ожирение")
                feedback += "Рекомендуется консультация врача. ";

            if (!string.IsNullOrWhiteSpace(txtWater.Text))
            {
                waterGlasses = int.Parse(txtWater.Text);
                if (waterGlasses < Goals.WaterGoal)
                    feedback += $"Пейте больше воды (цель: {Goals.WaterGoal} стаканов). ";
                else
                    feedback += "Отличное потребление воды! ";
            }

            if (!string.IsNullOrWhiteSpace(txtSleep.Text))
            {
                sleepHours = double.Parse(txtSleep.Text);
                if (sleepHours < 7)
                    feedback += "Старайтесь спать 7-9 часов. ";
                else if (sleepHours > 9)
                    feedback += "Слишком много сна может указывать на проблемы. ";
                else
                    feedback += "Хорошая продолжительность сна! ";
            }

            if (cmbActivity.SelectedIndex == 0)
                feedback += "Добавьте физическую активность. ";

            lblHealthFeedback.Text = feedback;
            UpdateProgressBar();
        }

        private void UpdateProgressBar()
        {
            int score = 0;

            if (bmiCategory == "Нормальный вес")
                score += 25;
            else if (bmiCategory == "Недостаточный вес" || bmiCategory == "Избыточный вес")
                score += 15;
            else
                score += 5;

            if (!string.IsNullOrWhiteSpace(txtWater.Text))
            {
                waterGlasses = int.Parse(txtWater.Text);
                if (waterGlasses >= Goals.WaterGoal)
                    score += 25;
                else if (waterGlasses >= Goals.WaterGoal - 3)
                    score += 15;
                else
                    score += 5;
            }

            if (!string.IsNullOrWhiteSpace(txtSleep.Text))
            {
                sleepHours = double.Parse(txtSleep.Text);
                if (sleepHours >= 7 && sleepHours <= 9)
                    score += 25;
                else if (sleepHours >= 6 && sleepHours <= 10)
                    score += 15;
                else
                    score += 5;
            }

            if (cmbActivity.SelectedIndex == 3)
                score += 15;
            else if (cmbActivity.SelectedIndex == 2)
                score += 10;
            else if (cmbActivity.SelectedIndex == 1)
                score += 5;

            int mealsCount = 0;
            if (chkBreakfast.Checked) mealsCount++;
            if (chkLunch.Checked) mealsCount++;
            if (chkDinner.Checked) mealsCount++;

            if (mealsCount == 3)
                score += 10;
            else if (mealsCount == 2)
                score += 7;
            else if (mealsCount == 1)
                score += 3;

            progressBar.Value = score;
            lblProgress.Text = $"Прогресс здоровья: {score}%";

            if (score >= 80)
                progressBar.ForeColor = Color.Green;
            else if (score >= 50)
                progressBar.ForeColor = Color.Orange;
            else
                progressBar.ForeColor = Color.Red;
        }

        private void btnSaveData_Click(object sender, EventArgs e)
        {
            try
            {
                userName = txtName.Text;

                if (!string.IsNullOrWhiteSpace(txtAge.Text))
                    userAge = int.Parse(txtAge.Text);

                userGender = cmbGender.SelectedItem?.ToString() ?? "";

                if (!string.IsNullOrWhiteSpace(txtHeight.Text))
                    userHeight = double.Parse(txtHeight.Text);

                if (!string.IsNullOrWhiteSpace(txtWeight.Text))
                    userWeight = double.Parse(txtWeight.Text);

                if (!string.IsNullOrWhiteSpace(txtWater.Text))
                    waterGlasses = int.Parse(txtWater.Text);

                if (!string.IsNullOrWhiteSpace(txtSleep.Text))
                    sleepHours = double.Parse(txtSleep.Text);

                activityLevel = cmbActivity.SelectedItem?.ToString() ?? "Нет";
                hadBreakfast = chkBreakfast.Checked;
                hadLunch = chkLunch.Checked;
                hadDinner = chkDinner.Checked;

                // Создаем запись и добавляем в глобальный список
                HealthRecord record = new HealthRecord
                {
                    Date = DateTime.Now,
                    Name = userName,
                    Age = userAge,
                    Gender = userGender,
                    Height = userHeight,
                    Weight = userWeight,
                    BMI = bmiValue,
                    BMICategory = bmiCategory,
                    WaterGlasses = waterGlasses,
                    SleepHours = sleepHours,
                    ActivityLevel = activityLevel,
                    HadBreakfast = hadBreakfast,
                    HadLunch = hadLunch,
                    HadDinner = hadDinner,
                    HealthScore = progressBar.Value
                };

                AllRecords.Add(record);

                MessageBox.Show($"Данные успешно сохранены!\nВсего записей: {AllRecords.Count}",
                    "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (FormatException)
            {
                MessageBox.Show("Убедитесь, что все числовые поля заполнены корректно!",
                    "Неверный формат", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtName.Clear();
            txtAge.Clear();
            txtHeight.Clear();
            txtWeight.Clear();
            txtWater.Clear();
            txtSleep.Clear();

            cmbGender.SelectedIndex = 0;
            cmbActivity.SelectedIndex = 0;

            chkBreakfast.Checked = false;
            chkLunch.Checked = false;
            chkDinner.Checked = false;

            lblBMIResult.Text = "ИМТ: Еще не рассчитан";
            lblBMIResult.ForeColor = Color.DarkGreen;
            lblHealthFeedback.Text = "Здесь появится обратная связь...";
            lblProgress.Text = "Прогресс здоровья: 0%";
            progressBar.Value = 0;

            lstSummary.Items.Clear();

            MessageBox.Show("Форма очищена!", "Очищено",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnShowSummary_Click(object sender, EventArgs e)
        {
            lstSummary.Items.Clear();

            lstSummary.Items.Add("========== СВОДКА ЗДОРОВЬЯ ==========");
            lstSummary.Items.Add("");

            lstSummary.Items.Add("--- Информация пользователя ---");
            lstSummary.Items.Add($"Имя: {(string.IsNullOrWhiteSpace(txtName.Text) ? "Не указано" : txtName.Text)}");
            lstSummary.Items.Add($"Возраст: {(string.IsNullOrWhiteSpace(txtAge.Text) ? "Не указан" : txtAge.Text)}");
            lstSummary.Items.Add($"Пол: {(cmbGender.SelectedItem == null ? "Не выбран" : cmbGender.SelectedItem.ToString())}");
            lstSummary.Items.Add($"Рост: {(string.IsNullOrWhiteSpace(txtHeight.Text) ? "Не указан" : txtHeight.Text + " см")}");
            lstSummary.Items.Add($"Вес: {(string.IsNullOrWhiteSpace(txtWeight.Text) ? "Не указан" : txtWeight.Text + " кг")}");
            lstSummary.Items.Add("");

            lstSummary.Items.Add("--- Отслеживание дня ---");
            lstSummary.Items.Add($"Вода: {(string.IsNullOrWhiteSpace(txtWater.Text) ? "0" : txtWater.Text)} стаканов");
            lstSummary.Items.Add($"Сон: {(string.IsNullOrWhiteSpace(txtSleep.Text) ? "0" : txtSleep.Text)} часов");
            lstSummary.Items.Add($"Активность: {(cmbActivity.SelectedItem == null ? "Нет" : cmbActivity.SelectedItem.ToString())}");

            string meals = "";
            if (chkBreakfast.Checked) meals += "Завтрак ";
            if (chkLunch.Checked) meals += "Обед ";
            if (chkDinner.Checked) meals += "Ужин";
            lstSummary.Items.Add($"Приемы пищи: {(string.IsNullOrWhiteSpace(meals) ? "Нет" : meals)}");
            lstSummary.Items.Add("");

            lstSummary.Items.Add("--- Показатели здоровья ---");
            lstSummary.Items.Add(lblBMIResult.Text);
            lstSummary.Items.Add($"Оценка здоровья: {progressBar.Value}%");
            lstSummary.Items.Add("");

            if (AllRecords.Count > 0)
            {
                lstSummary.Items.Add($"--- Последние 5 записей (всего: {AllRecords.Count}) ---");
                int count = 0;
                for (int i = AllRecords.Count - 1; i >= 0 && count < 5; i--, count++)
                {
                    var r = AllRecords[i];
                    lstSummary.Items.Add($"[{r.Date:dd.MM.yyyy HH:mm}] {r.Name}, ИМТ: {r.BMI:F1}, Балл: {r.HealthScore}%");
                }
            }
            else
            {
                lstSummary.Items.Add("--- Нет сохраненных записей ---");
            }

            lstSummary.Items.Add("");
            lstSummary.Items.Add("====================================");
        }

        // Обработчики меню
        private void historyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 historyForm = new Form2();
            historyForm.ShowDialog();
        }

        private void calorieCalculatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 calorieForm = new Form3();
            calorieForm.ShowDialog();
        }

        private void workoutPlanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form4 workoutForm = new Form4();
            workoutForm.ShowDialog();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form5 settingsForm = new Form5();
            if (settingsForm.ShowDialog() == DialogResult.OK)
            {
                // Обновляем отображение после изменения целей
                GenerateHealthFeedback();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите выйти?", "Выход",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Программа отслеживания здоровья v2.0\n\n" +
                           "Функции:\n" +
                           "• Расчет ИМТ\n" +
                           "• Отслеживание воды, сна, питания\n" +
                           "• История и статистика\n" +
                           "• Калькулятор калорий\n" +
                           "• План тренировок\n" +
                           "• Персональные цели\n\n" +
                           "© 2024 Daily Health Program",
                           "О программе", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {

        }
    }

    // Классы данных
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