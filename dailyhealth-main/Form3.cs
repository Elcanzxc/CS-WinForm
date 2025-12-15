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
                // Проверка ввода
                if (string.IsNullOrWhiteSpace(txtAge.Text) ||
                    string.IsNullOrWhiteSpace(txtWeight.Text) ||
                    string.IsNullOrWhiteSpace(txtHeight.Text))
                {
                    MessageBox.Show("Заполните все поля!", "Требуется ввод",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int age = int.Parse(txtAge.Text);
                double weight = double.Parse(txtWeight.Text);
                double height = double.Parse(txtHeight.Text);
                string gender = cmbGender.SelectedItem.ToString();
                string activityLevel = cmbActivityLevel.SelectedItem.ToString();
                string goal = cmbGoal.SelectedItem.ToString();

                // Формула Миффлина-Сан Жеора для базального метаболизма (BMR)
                double bmr;
                if (gender == "Мужской")
                {
                    bmr = (10 * weight) + (6.25 * height) - (5 * age) + 5;
                }
                else
                {
                    bmr = (10 * weight) + (6.25 * height) - (5 * age) - 161;
                }

                // Коэффициент активности
                double activityFactor = 1.2; // Малоподвижный по умолчанию
                switch (cmbActivityLevel.SelectedIndex)
                {
                    case 0: activityFactor = 1.2; break;  // Малоподвижный
                    case 1: activityFactor = 1.375; break; // Легкая активность
                    case 2: activityFactor = 1.55; break;  // Умеренная активность
                    case 3: activityFactor = 1.725; break; // Высокая активность
                    case 4: activityFactor = 1.9; break;   // Очень высокая
                }

                // Общий расход калорий (TDEE)
                double tdee = bmr * activityFactor;

                // Корректировка по цели
                calculatedCalories = tdee;
                switch (cmbGoal.SelectedIndex)
                {
                    case 0: // Похудение
                        calculatedCalories = tdee - 500; // Дефицит 500 ккал
                        break;
                    case 1: // Поддержание веса
                        calculatedCalories = tdee;
                        break;
                    case 2: // Набор массы
                        calculatedCalories = tdee + 500; // Профицит 500 ккал
                        break;
                }

                // Отображение результатов
                lblBMR.Text = $"Базальный метаболизм (BMR): {bmr:F0} ккал/день";
                lblTDEE.Text = $"Общий расход (TDEE): {tdee:F0} ккал/день";
                lblRecommended.Text = $"Рекомендуемое потребление: {calculatedCalories:F0} ккал/день";
                lblRecommended.ForeColor = Color.DarkGreen;

                // Распределение макронутриентов
                CalculateMacros();

                // Сохранение цели
                Form1.Goals.CaloriesGoal = (int)calculatedCalories;
            }
            catch (FormatException)
            {
                MessageBox.Show("Введите корректные числовые значения!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CalculateMacros()
        {
            if (calculatedCalories == 0) return;

            // Стандартное распределение макронутриентов
            double proteinPercent = 0.30; // 30% белков
            double carbsPercent = 0.40;   // 40% углеводов
            double fatPercent = 0.30;     // 30% жиров

            // Корректировка по цели
            switch (cmbGoal.SelectedIndex)
            {
                case 0: // Похудение
                    proteinPercent = 0.35;
                    carbsPercent = 0.35;
                    fatPercent = 0.30;
                    break;
                case 2: // Набор массы
                    proteinPercent = 0.30;
                    carbsPercent = 0.45;
                    fatPercent = 0.25;
                    break;
            }

            double proteinCalories = calculatedCalories * proteinPercent;
            double carbsCalories = calculatedCalories * carbsPercent;
            double fatCalories = calculatedCalories * fatPercent;

            // 1г белка = 4 ккал, 1г углеводов = 4 ккал, 1г жиров = 9 ккал
            double proteinGrams = proteinCalories / 4;
            double carbsGrams = carbsCalories / 4;
            double fatGrams = fatCalories / 9;

            txtMacros.Clear();
            txtMacros.AppendText("===== МАКРОНУТРИЕНТЫ =====\r\n\r\n");
            txtMacros.AppendText($"Белки: {proteinGrams:F0}г ({proteinPercent * 100:F0}%)\r\n");
            txtMacros.AppendText($"  = {proteinCalories:F0} ккал\r\n\r\n");
            txtMacros.AppendText($"Углеводы: {carbsGrams:F0}г ({carbsPercent * 100:F0}%)\r\n");
            txtMacros.AppendText($"  = {carbsCalories:F0} ккал\r\n\r\n");
            txtMacros.AppendText($"Жиры: {fatGrams:F0}г ({fatPercent * 100:F0}%)\r\n");
            txtMacros.AppendText($"  = {fatCalories:F0} ккал\r\n\r\n");

            // Рекомендации по питанию
            txtRecommendations.Clear();
            txtRecommendations.AppendText("===== РЕКОМЕНДАЦИИ =====\r\n\r\n");

            switch (cmbGoal.SelectedIndex)
            {
                case 0: // Похудение
                    txtRecommendations.AppendText("Для ПОХУДЕНИЯ:\r\n\r\n");
                    txtRecommendations.AppendText("• Создан дефицит калорий (-500 ккал)\r\n");
                    txtRecommendations.AppendText("• Увеличено потребление белка для сохранения мышц\r\n");
                    txtRecommendations.AppendText("• Пейте 2-3л воды в день\r\n");
                    txtRecommendations.AppendText("• Ешьте 4-5 раз в день небольшими порциями\r\n");
                    txtRecommendations.AppendText("• Избегайте быстрых углеводов и жареного\r\n");
                    txtRecommendations.AppendText("• Рекомендуемая потеря: 0.5-1 кг/неделю\r\n");
                    break;

                case 1: // Поддержание
                    txtRecommendations.AppendText("Для ПОДДЕРЖАНИЯ ВЕСА:\r\n\r\n");
                    txtRecommendations.AppendText("• Сбалансированное питание\r\n");
                    txtRecommendations.AppendText("• Поддержание текущего уровня активности\r\n");
                    txtRecommendations.AppendText("• Регулярные приемы пищи\r\n");
                    txtRecommendations.AppendText("• Разнообразный рацион\r\n");
                    txtRecommendations.AppendText("• Контроль порций\r\n");
                    break;

                case 2: // Набор массы
                    txtRecommendations.AppendText("Для НАБОРА МАССЫ:\r\n\r\n");
                    txtRecommendations.AppendText("• Создан профицит калорий (+500 ккал)\r\n");
                    txtRecommendations.AppendText("• Увеличено потребление углеводов для энергии\r\n");
                    txtRecommendations.AppendText("• Достаточно белка для роста мышц\r\n");
                    txtRecommendations.AppendText("• Ешьте 5-6 раз в день\r\n");
                    txtRecommendations.AppendText("• Сочетайте с силовыми тренировками\r\n");
                    txtRecommendations.AppendText("• Рекомендуемый набор: 0.5-1 кг/неделю\r\n");
                    break;
            }

            txtRecommendations.AppendText("\r\n===== ПРИМЕРЫ ПРОДУКТОВ =====\r\n\r\n");
            txtRecommendations.AppendText("Источники белка:\r\n");
            txtRecommendations.AppendText("• Курица, индейка, рыба\r\n");
            txtRecommendations.AppendText("• Яйца, творог\r\n");
            txtRecommendations.AppendText("• Бобовые, тофу\r\n\r\n");

            txtRecommendations.AppendText("Источники углеводов:\r\n");
            txtRecommendations.AppendText("• Крупы (гречка, овсянка, рис)\r\n");
            txtRecommendations.AppendText("• Цельнозерновой хлеб\r\n");
            txtRecommendations.AppendText("• Овощи и фрукты\r\n\r\n");

            txtRecommendations.AppendText("Источники жиров:\r\n");
            txtRecommendations.AppendText("• Орехи и семена\r\n");
            txtRecommendations.AppendText("• Авокадо, оливковое масло\r\n");
            txtRecommendations.AppendText("• Жирная рыба\r\n");
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