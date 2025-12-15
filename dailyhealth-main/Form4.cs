using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace dailyhealth
{
    public partial class Form4 : Form
    {
        private Dictionary<string, List<string>> workoutPlans = new Dictionary<string, List<string>>();

        public Form4()
        {
            InitializeComponent();
            InitializeWorkoutPlans();
            cmbLevel.SelectedIndex = 1; // Средний по умолчанию
            cmbGoal.SelectedIndex = 0;
        }

        private void InitializeWorkoutPlans()
        {
            // Планы для похудения
            workoutPlans["beginner_weight_loss"] = new List<string>
            {
                "ПОНЕДЕЛЬНИК - Кардио",
                "• Ходьба быстрым шагом - 30 минут",
                "• Легкие прыжки на месте - 2x15",
                "• Растяжка - 10 минут",
                "",
                "СРЕДА - Силовая тренировка",
                "• Приседания - 3x10",
                "• Отжимания от колен - 3x8",
                "• Планка - 3x20 секунд",
                "• Выпады - 3x10 на каждую ногу",
                "",
                "ПЯТНИЦА - Комбинированная",
                "• Бег трусцой - 20 минут",
                "• Приседания с прыжком - 3x10",
                "• Скручивания на пресс - 3x15",
                "• Растяжка - 10 минут"
            };

            workoutPlans["intermediate_weight_loss"] = new List<string>
            {
                "ПОНЕДЕЛЬНИК - Интервальное кардио",
                "• Бег 2 мин / Ходьба 1 мин - 30 минут",
                "• Прыжки со скакалкой - 3x1 минута",
                "• Берпи - 3x10",
                "",
                "ВТОРНИК - Силовая (верх)",
                "• Отжимания - 4x12",
                "• Подтягивания - 4x8",
                "• Жим гантелей - 4x12",
                "• Планка - 3x45 секунд",
                "",
                "ЧЕТВЕРГ - Силовая (низ)",
                "• Приседания - 4x15",
                "• Выпады с гантелями - 4x12",
                "• Мертвая тяга - 4x12",
                "• Подъемы на носки - 3x20",
                "",
                "СУББОТА - HIIT тренировка",
                "• Берпи - 4x15",
                "• Прыжки с выпадами - 4x12",
                "• Скалолаз - 4x20",
                "• Отдых между упражнениями 30 сек"
            };

            // Планы для набора массы
            workoutPlans["beginner_muscle"] = new List<string>
            {
                "ПОНЕДЕЛЬНИК - Грудь и трицепс",
                "• Жим лежа - 4x10",
                "• Отжимания - 3x12",
                "• Разводка гантелей - 3x12",
                "• Французский жим - 3x12",
                "",
                "СРЕДА - Спина и бицепс",
                "• Подтягивания - 4x8",
                "• Тяга штанги в наклоне - 4x10",
                "• Тяга верхнего блока - 3x12",
                "• Сгибания рук с гантелями - 3x12",
                "",
                "ПЯТНИЦА - Ноги и плечи",
                "• Приседания со штангой - 4x10",
                "• Жим ногами - 3x12",
                "• Жим гантелей стоя - 4x10",
                "• Махи гантелями в стороны - 3x12"
            };

            workoutPlans["intermediate_muscle"] = new List<string>
            {
                "ПОНЕДЕЛЬНИК - Грудь",
                "• Жим лежа - 4x8-10",
                "• Жим гантелей на наклонной - 4x10",
                "• Отжимания на брусьях - 3x12",
                "• Разводка гантелей - 3x12",
                "• Кроссовер - 3x15",
                "",
                "ВТОРНИК - Спина",
                "• Становая тяга - 4x6",
                "• Подтягивания широким хватом - 4x10",
                "• Тяга штанги в наклоне - 4x10",
                "• Тяга Т-грифа - 3x12",
                "• Шраги - 3x15",
                "",
                "ЧЕТВЕРГ - Ноги",
                "• Приседания - 5x8",
                "• Жим ногами - 4x12",
                "• Румынская тяга - 4x10",
                "• Сгибания ног - 3x15",
                "• Разгибания ног - 3x15",
                "",
                "ПЯТНИЦА - Плечи и руки",
                "• Жим штанги стоя - 4x10",
                "• Махи гантелями - 4x12",
                "• Подъем штанги на бицепс - 4x10",
                "• Французский жим лежа - 4x10",
                "• Молотки - 3x12"
            };

            // Планы для поддержания формы
            workoutPlans["beginner_maintenance"] = new List<string>
            {
                "ПОНЕДЕЛЬНИК - Полное тело",
                "• Приседания - 3x12",
                "• Отжимания - 3x10",
                "• Тяга гантелей - 3x12",
                "• Планка - 3x30 секунд",
                "",
                "ЧЕТВЕРГ - Кардио и тонус",
                "• Бег/Велосипед - 30 минут",
                "• Выпады - 3x10",
                "• Скручивания - 3x15",
                "• Растяжка - 10 минут"
            };

            workoutPlans["intermediate_maintenance"] = new List<string>
            {
                "ПОНЕДЕЛЬНИК/ЧЕТВЕРГ - Силовая",
                "• Приседания - 4x12",
                "• Жим лежа/Отжимания - 4x12",
                "• Подтягивания/Тяга - 4x10",
                "• Жим гантелей сидя - 3x12",
                "• Планка - 3x1 минута",
                "",
                "ВТОРНИК/СУББОТА - Кардио",
                "• Бег/Плавание - 40 минут",
                "• Интервалы высокой интенсивности - 10 минут",
                "• Растяжка - 15 минут"
            };
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            string level = cmbLevel.SelectedItem.ToString();
            string goal = cmbGoal.SelectedItem.ToString();

            string levelKey = level == "Новичок" ? "beginner" : "intermediate";
            string goalKey = "";

            if (goal == "Похудение")
                goalKey = "weight_loss";
            else if (goal == "Набор массы")
                goalKey = "muscle";
            else
                goalKey = "maintenance";

            string planKey = $"{levelKey}_{goalKey}";

            txtWorkoutPlan.Clear();

            if (workoutPlans.ContainsKey(planKey))
            {
                txtWorkoutPlan.AppendText($"===== ПЛАН ТРЕНИРОВОК =====\r\n");
                txtWorkoutPlan.AppendText($"Уровень: {level}\r\n");
                txtWorkoutPlan.AppendText($"Цель: {goal}\r\n");
                txtWorkoutPlan.AppendText($"==============================\r\n\r\n");

                foreach (string line in workoutPlans[planKey])
                {
                    txtWorkoutPlan.AppendText(line + "\r\n");
                }

                // Добавляем общие рекомендации
                txtWorkoutPlan.AppendText("\r\n===== ВАЖНЫЕ РЕКОМЕНДАЦИИ =====\r\n\r\n");
                txtWorkoutPlan.AppendText("• Разминка 5-10 минут перед каждой тренировкой\r\n");
                txtWorkoutPlan.AppendText("• Отдых между подходами: 60-90 секунд\r\n");
                txtWorkoutPlan.AppendText("• Отдых между тренировками: минимум 1 день\r\n");
                txtWorkoutPlan.AppendText("• Пейте воду во время тренировки\r\n");
                txtWorkoutPlan.AppendText("• Соблюдайте правильную технику выполнения\r\n");
                txtWorkoutPlan.AppendText("• Постепенно увеличивайте нагрузку\r\n");

                if (goal == "Набор массы")
                {
                    txtWorkoutPlan.AppendText("\r\nДЛЯ РОСТА МЫШЦ:\r\n");
                    txtWorkoutPlan.AppendText("• Питание с профицитом калорий\r\n");
                    txtWorkoutPlan.AppendText("• 1.6-2.2г белка на кг веса\r\n");
                    txtWorkoutPlan.AppendText("• Прогрессивная перегрузка\r\n");
                    txtWorkoutPlan.AppendText("• Сон 8-9 часов\r\n");
                }
                else if (goal == "Похудение")
                {
                    txtWorkoutPlan.AppendText("\r\nДЛЯ ЖИРОСЖИГАНИЯ:\r\n");
                    txtWorkoutPlan.AppendText("• Дефицит калорий 300-500 ккал\r\n");
                    txtWorkoutPlan.AppendText("• Высокоинтенсивное кардио 3-4 раза/неделю\r\n");
                    txtWorkoutPlan.AppendText("• Сохранение мышечной массы силовыми\r\n");
                    txtWorkoutPlan.AppendText("• Контроль питания важнее тренировок\r\n");
                }
            }

            // Генерация дополнительных советов
            GenerateExerciseTips();
        }

        private void GenerateExerciseTips()
        {
            txtTips.Clear();
            txtTips.AppendText("===== СОВЕТЫ ПО УПРАЖНЕНИЯМ =====\r\n\r\n");

            txtTips.AppendText("ПРИСЕДАНИЯ:\r\n");
            txtTips.AppendText("• Спина прямая, взгляд вперед\r\n");
            txtTips.AppendText("• Колени не выходят за носки\r\n");
            txtTips.AppendText("• Опускаться до параллели с полом\r\n\r\n");

            txtTips.AppendText("ОТЖИМАНИЯ:\r\n");
            txtTips.AppendText("• Тело - прямая линия\r\n");
            txtTips.AppendText("• Локти под углом 45°\r\n");
            txtTips.AppendText("• Полная амплитуда движения\r\n\r\n");

            txtTips.AppendText("ПЛАНКА:\r\n");
            txtTips.AppendText("• Не прогибаться в пояснице\r\n");
            txtTips.AppendText("• Пресс напряжен\r\n");
            txtTips.AppendText("• Дышать равномерно\r\n\r\n");

            txtTips.AppendText("ВЫПАДЫ:\r\n");
            txtTips.AppendText("• Колено передней ноги над стопой\r\n");
            txtTips.AppendText("• Заднее колено почти касается пола\r\n");
            txtTips.AppendText("• Корпус вертикально\r\n\r\n");

            txtTips.AppendText("===== ПРЕДОТВРАЩЕНИЕ ТРАВМ =====\r\n\r\n");
            txtTips.AppendText("✓ Всегда разминайтесь\r\n");
            txtTips.AppendText("✓ Увеличивайте нагрузку постепенно\r\n");
            txtTips.AppendText("✓ Слушайте свое тело\r\n");
            txtTips.AppendText("✓ Восстанавливайтесь должным образом\r\n");
            txtTips.AppendText("✓ При боли - прекратите упражнение\r\n");
        }

        private void btnSavePlan_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtWorkoutPlan.Text))
            {
                MessageBox.Show("Сначала создайте план тренировок!", "Предупреждение",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Текстовые файлы (*.txt)|*.txt";
            saveDialog.FileName = $"workout_plan_{DateTime.Now:yyyyMMdd}.txt";

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    System.IO.File.WriteAllText(saveDialog.FileName, txtWorkoutPlan.Text);
                    MessageBox.Show("План успешно сохранен!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка",
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