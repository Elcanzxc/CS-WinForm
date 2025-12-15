using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dailyhealth
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            LoadHistory();
            CalculateStatistics();
        }

        private void LoadHistory()
        {
            lvHistory.Items.Clear();

            if (Form1.AllRecords.Count == 0)
            {
                MessageBox.Show("Нет сохраненных записей!", "Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            foreach (var record in Form1.AllRecords.OrderByDescending(r => r.Date))
            {
                ListViewItem item = new ListViewItem(record.Date.ToString("dd.MM.yyyy"));
                item.SubItems.Add(record.Date.ToString("HH:mm"));
                item.SubItems.Add(record.Name);
                item.SubItems.Add(record.Weight.ToString("F1"));
                item.SubItems.Add(record.BMI.ToString("F2"));
                item.SubItems.Add(record.BMICategory);
                item.SubItems.Add(record.WaterGlasses.ToString());
                item.SubItems.Add(record.SleepHours.ToString("F1"));
                item.SubItems.Add(record.ActivityLevel);
                item.SubItems.Add(record.HealthScore.ToString());

                // Цветовое кодирование по оценке здоровья
                if (record.HealthScore >= 80)
                    item.BackColor = Color.LightGreen;
                else if (record.HealthScore >= 50)
                    item.BackColor = Color.LightYellow;
                else
                    item.BackColor = Color.LightCoral;

                lvHistory.Items.Add(item);
            }
        }

        private void CalculateStatistics()
        {
            if (Form1.AllRecords.Count == 0)
                return;

            var records = Form1.AllRecords;

            // Средние значения
            double avgBMI = records.Average(r => r.BMI);
            double avgWeight = records.Average(r => r.Weight);
            double avgWater = records.Average(r => r.WaterGlasses);
            double avgSleep = records.Average(r => r.SleepHours);
            double avgScore = records.Average(r => r.HealthScore);

            // Последние значения
            var latest = records.Last();
            var oldest = records.First();
            double weightChange = latest.Weight - oldest.Weight;

            // Отображение статистики
            txtStats.Clear();
            txtStats.AppendText("========== СТАТИСТИКА ==========\r\n\r\n");
            txtStats.AppendText($"Всего записей: {records.Count}\r\n");
            txtStats.AppendText($"Период: {oldest.Date:dd.MM.yyyy} - {latest.Date:dd.MM.yyyy}\r\n\r\n");

            txtStats.AppendText("--- Средние показатели ---\r\n");
            txtStats.AppendText($"ИМТ: {avgBMI:F2}\r\n");
            txtStats.AppendText($"Вес: {avgWeight:F1} кг\r\n");
            txtStats.AppendText($"Вода: {avgWater:F1} стаканов/день\r\n");
            txtStats.AppendText($"Сон: {avgSleep:F1} часов/день\r\n");
            txtStats.AppendText($"Оценка здоровья: {avgScore:F0}%\r\n\r\n");

            txtStats.AppendText("--- Изменения ---\r\n");
            txtStats.AppendText($"Изменение веса: {(weightChange >= 0 ? "+" : "")}{weightChange:F1} кг\r\n");
            txtStats.AppendText($"Текущий вес: {latest.Weight:F1} кг\r\n");
            txtStats.AppendText($"Начальный вес: {oldest.Weight:F1} кг\r\n\r\n");

            // Категории ИМТ
            var bmiGroups = records.GroupBy(r => r.BMICategory);
            txtStats.AppendText("--- Распределение по ИМТ ---\r\n");
            foreach (var group in bmiGroups)
            {
                double percentage = (group.Count() * 100.0) / records.Count;
                txtStats.AppendText($"{group.Key}: {group.Count()} раз ({percentage:F0}%)\r\n");
            }

            txtStats.AppendText("\r\n--- Уровни активности ---\r\n");
            var activityGroups = records.GroupBy(r => r.ActivityLevel);
            foreach (var group in activityGroups)
            {
                double percentage = (group.Count() * 100.0) / records.Count;
                txtStats.AppendText($"{group.Key}: {group.Count()} раз ({percentage:F0}%)\r\n");
            }

            // Лучший день
            var bestDay = records.OrderByDescending(r => r.HealthScore).First();
            txtStats.AppendText($"\r\n--- Лучший день ---\r\n");
            txtStats.AppendText($"Дата: {bestDay.Date:dd.MM.yyyy}\r\n");
            txtStats.AppendText($"Оценка: {bestDay.HealthScore}%\r\n");
            txtStats.AppendText($"ИМТ: {bestDay.BMI:F2}\r\n");
            txtStats.AppendText($"Вода: {bestDay.WaterGlasses} стаканов\r\n");
            txtStats.AppendText($"Сон: {bestDay.SleepHours:F1} часов\r\n");
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadHistory();
            CalculateStatistics();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lvHistory.SelectedItems.Count == 0)
            {
                MessageBox.Show("Выберите запись для удаления!", "Предупреждение",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Удалить выбранную запись?", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                int index = lvHistory.SelectedIndices[0];
                int recordIndex = Form1.AllRecords.Count - 1 - index;

                Form1.AllRecords.RemoveAt(recordIndex);
                LoadHistory();
                CalculateStatistics();

                MessageBox.Show("Запись удалена!", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (Form1.AllRecords.Count == 0)
            {
                MessageBox.Show("Нет данных для экспорта!", "Предупреждение",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "CSV файлы (*.csv)|*.csv|Текстовые файлы (*.txt)|*.txt";
            saveDialog.FileName = $"health_data_{DateTime.Now:yyyyMMdd}.csv";

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(saveDialog.FileName))
                    {
                        // Заголовок
                        file.WriteLine("Дата,Время,Имя,Вес,ИМТ,Категория,Вода,Сон,Активность,Оценка");

                        // Данные
                        foreach (var record in Form1.AllRecords)
                        {
                            file.WriteLine($"{record.Date:dd.MM.yyyy},{record.Date:HH:mm}," +
                                         $"{record.Name},{record.Weight:F1},{record.BMI:F2}," +
                                         $"{record.BMICategory},{record.WaterGlasses}," +
                                         $"{record.SleepHours:F1},{record.ActivityLevel},{record.HealthScore}");
                        }
                    }

                    MessageBox.Show("Данные успешно экспортированы!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при экспорте: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lvHistory_DoubleClick(object sender, EventArgs e)
        {
            if (lvHistory.SelectedItems.Count > 0)
            {
                int index = lvHistory.SelectedIndices[0];
                int recordIndex = Form1.AllRecords.Count - 1 - index;
                var record = Form1.AllRecords[recordIndex];

                string details = $"=== ДЕТАЛИ ЗАПИСИ ===\r\n\r\n" +
                               $"Дата и время: {record.Date:dd.MM.yyyy HH:mm}\r\n\r\n" +
                               $"Личные данные:\r\n" +
                               $"  Имя: {record.Name}\r\n" +
                               $"  Возраст: {record.Age}\r\n" +
                               $"  Пол: {record.Gender}\r\n" +
                               $"  Рост: {record.Height} см\r\n" +
                               $"  Вес: {record.Weight} кг\r\n\r\n" +
                               $"Показатели здоровья:\r\n" +
                               $"  ИМТ: {record.BMI:F2} ({record.BMICategory})\r\n" +
                               $"  Вода: {record.WaterGlasses} стаканов\r\n" +
                               $"  Сон: {record.SleepHours} часов\r\n" +
                               $"  Активность: {record.ActivityLevel}\r\n\r\n" +
                               $"Питание:\r\n" +
                               $"  Завтрак: {(record.HadBreakfast ? "Да" : "Нет")}\r\n" +
                               $"  Обед: {(record.HadLunch ? "Да" : "Нет")}\r\n" +
                               $"  Ужин: {(record.HadDinner ? "Да" : "Нет")}\r\n\r\n" +
                               $"Общая оценка: {record.HealthScore}%";

                MessageBox.Show(details, "Детали записи",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}