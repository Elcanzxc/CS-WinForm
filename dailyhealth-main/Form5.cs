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
            // Загружаем текущие цели
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
            txtCurrentGoals.AppendText("===== ТЕКУЩИЕ ЦЕЛИ =====\r\n\r\n");
            txtCurrentGoals.AppendText($"💧 Вода: {Form1.Goals.WaterGoal} стаканов/день\r\n");
            txtCurrentGoals.AppendText($"😴 Сон: {Form1.Goals.SleepGoal:F1} часов/день\r\n");
            txtCurrentGoals.AppendText($"⚖️ Целевой вес: {Form1.Goals.WeightGoal:F1} кг\r\n");
            txtCurrentGoals.AppendText($"🍽️ Калории: {Form1.Goals.CaloriesGoal} ккал/день\r\n");
            txtCurrentGoals.AppendText($"👟 Шаги: {Form1.Goals.StepsGoal} шагов/день\r\n");

            // Показываем прогресс если есть записи
            if (Form1.AllRecords.Count > 0)
            {
                var lastRecord = Form1.AllRecords[Form1.AllRecords.Count - 1];

                txtCurrentGoals.AppendText("\r\n===== ПОСЛЕДНИЕ ПОКАЗАТЕЛИ =====\r\n\r\n");
                txtCurrentGoals.AppendText($"Вода: {lastRecord.WaterGlasses}/{Form1.Goals.WaterGoal} стаканов ");
                txtCurrentGoals.AppendText($"({(lastRecord.WaterGlasses * 100.0 / Form1.Goals.WaterGoal):F0}%)\r\n");

                txtCurrentGoals.AppendText($"Сон: {lastRecord.SleepHours:F1}/{Form1.Goals.SleepGoal:F1} часов ");
                txtCurrentGoals.AppendText($"({(lastRecord.SleepHours * 100.0 / Form1.Goals.SleepGoal):F0}%)\r\n");

                txtCurrentGoals.AppendText($"Вес: {lastRecord.Weight:F1} кг ");
                double weightDiff = Form1.Goals.WeightGoal - lastRecord.Weight;
                if (Math.Abs(weightDiff) < 0.5)
                    txtCurrentGoals.AppendText("(Цель достигнута! ✓)\r\n");
                else if (weightDiff > 0)
                    txtCurrentGoals.AppendText($"(Нужно набрать: {weightDiff:F1} кг)\r\n");
                else
                    txtCurrentGoals.AppendText($"(Нужно сбросить: {Math.Abs(weightDiff):F1} кг)\r\n");
            }

            // Добавляем рекомендации
            txtCurrentGoals.AppendText("\r\n===== РЕКОМЕНДАЦИИ =====\r\n\r\n");

            if (Form1.Goals.WaterGoal < 8)
                txtCurrentGoals.AppendText("⚠️ Рекомендуется минимум 8 стаканов воды\r\n");

            if (Form1.Goals.SleepGoal < 7)
                txtCurrentGoals.AppendText("⚠️ Рекомендуется минимум 7-8 часов сна\r\n");
            else if (Form1.Goals.SleepGoal > 9)
                txtCurrentGoals.AppendText("⚠️ Слишком много сна может быть вредно\r\n");

            if (Form1.Goals.CaloriesGoal < 1200)
                txtCurrentGoals.AppendText("⚠️ Слишком низкое потребление калорий\r\n");
            else if (Form1.Goals.CaloriesGoal > 3500)
                txtCurrentGoals.AppendText("ℹ️ Высокое потребление - подходит для активных людей\r\n");

            if (Form1.Goals.StepsGoal < 7000)
                txtCurrentGoals.AppendText("ℹ️ Рекомендуется минимум 7000-10000 шагов\r\n");
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Валидация и сохранение
                int waterGoal = int.Parse(txtWaterGoal.Text);
                double sleepGoal = double.Parse(txtSleepGoal.Text);
                double weightGoal = double.Parse(txtWeightGoal.Text);
                int caloriesGoal = int.Parse(txtCaloriesGoal.Text);
                int stepsGoal = int.Parse(txtStepsGoal.Text);

                // Проверка разумных значений
                if (waterGoal < 1 || waterGoal > 20)
                {
                    MessageBox.Show("Цель по воде должна быть между 1 и 20 стаканами!",
                        "Неверное значение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (sleepGoal < 4 || sleepGoal > 12)
                {
                    MessageBox.Show("Цель по сну должна быть между 4 и 12 часами!",
                        "Неверное значение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (weightGoal < 30 || weightGoal > 200)
                {
                    MessageBox.Show("Целевой вес должен быть между 30 и 200 кг!",
                        "Неверное значение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (caloriesGoal < 800 || caloriesGoal > 5000)
                {
                    MessageBox.Show("Калории должны быть между 800 и 5000 ккал!",
                        "Неверное значение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (stepsGoal < 1000 || stepsGoal > 50000)
                {
                    MessageBox.Show("Цель по шагам должна быть между 1000 и 50000!",
                        "Неверное значение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Сохраняем цели
                Form1.Goals.WaterGoal = waterGoal;
                Form1.Goals.SleepGoal = sleepGoal;
                Form1.Goals.WeightGoal = weightGoal;
                Form1.Goals.CaloriesGoal = caloriesGoal;
                Form1.Goals.StepsGoal = stepsGoal;

                UpdateGoalsDisplay();

                MessageBox.Show("Цели успешно сохранены!", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
            }
            catch (FormatException)
            {
                MessageBox.Show("Введите корректные числовые значения!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Сбросить все цели к значениям по умолчанию?",
                "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Form1.Goals.WaterGoal = 8;
                Form1.Goals.SleepGoal = 8;
                Form1.Goals.WeightGoal = 70;
                Form1.Goals.CaloriesGoal = 2000;
                Form1.Goals.StepsGoal = 10000;

                LoadCurrentGoals();

                MessageBox.Show("Цели сброшены к значениям по умолчанию!", "Сброс",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCalculateIdealWeight_Click(object sender, EventArgs e)
        {
            // Показать калькулятор идеального веса
            string info = "===== ФОРМУЛЫ ИДЕАЛЬНОГО ВЕСА =====\r\n\r\n";
            info += "Введите ваш рост в поле ниже, и программа\r\n";
            info += "рассчитает идеальный вес по разным формулам:\r\n\r\n";
            info += "• Формула Devine\r\n";
            info += "• Формула Robinson\r\n";
            info += "• Формула Miller\r\n";
            info += "• Индекс BMI (18.5-24.9)\r\n";

            string input = Microsoft.VisualBasic.Interaction.InputBox(
                info + "\r\nВведите ваш рост в см:",
                "Калькулятор идеального веса",
                "170");

            if (!string.IsNullOrEmpty(input))
            {
                try
                {
                    double height = double.Parse(input);
                    if (height < 120 || height > 220)
                    {
                        MessageBox.Show("Рост должен быть между 120 и 220 см!",
                            "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string results = "===== РАСЧЕТ ИДЕАЛЬНОГО ВЕСА =====\r\n\r\n";
                    results += $"Рост: {height} см\r\n\r\n";

                    // Формула для мужчин и женщин
                    double heightInInches = height / 2.54;
                    double devineMale = 50 + 2.3 * (heightInInches - 60);
                    double devineFemale = 45.5 + 2.3 * (heightInInches - 60);

                    results += $"Формула Devine:\r\n";
                    results += $"  Мужчины: {devineMale:F1} кг\r\n";
                    results += $"  Женщины: {devineFemale:F1} кг\r\n\r\n";

                    // Диапазон по ИМТ
                    double heightInMeters = height / 100.0;
                    double minWeight = 18.5 * heightInMeters * heightInMeters;
                    double maxWeight = 24.9 * heightInMeters * heightInMeters;

                    results += $"Здоровый диапазон (ИМТ 18.5-24.9):\r\n";
                    results += $"  От {minWeight:F1} до {maxWeight:F1} кг\r\n\r\n";

                    results += $"Рекомендуемый целевой вес:\r\n";
                    double recommendedWeight = (minWeight + maxWeight) / 2;
                    results += $"  {recommendedWeight:F1} кг (середина диапазона)\r\n";

                    if (MessageBox.Show(results + "\r\n\r\nУстановить рекомендуемый вес как цель?",
                        "Результаты", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        txtWeightGoal.Text = recommendedWeight.ToString("F1");
                    }
                }
                catch
                {
                    MessageBox.Show("Введите корректное числовое значение!",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void txtWaterGoal_TextChanged(object sender, EventArgs e)
        {
            // Динамическое обновление рекомендаций при изменении
            if (!string.IsNullOrWhiteSpace(txtWaterGoal.Text))
            {
                try
                {
                    int value = int.Parse(txtWaterGoal.Text);
                    if (value < 6)
                        lblWaterHint.Text = "⚠️ Маловато";
                    else if (value >= 8 && value <= 12)
                        lblWaterHint.Text = "✓ Отлично";
                    else if (value > 12)
                        lblWaterHint.Text = "ℹ️ Много";
                    else
                        lblWaterHint.Text = "ℹ️ Норма";
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
                        lblSleepHint.Text = "⚠️ Мало";
                    else if (value >= 7 && value <= 9)
                        lblSleepHint.Text = "✓ Идеально";
                    else if (value > 9)
                        lblSleepHint.Text = "⚠️ Много";
                    else
                        lblSleepHint.Text = "ℹ️ Норма";
                }
                catch { lblSleepHint.Text = ""; }
            }
        }
    }
}