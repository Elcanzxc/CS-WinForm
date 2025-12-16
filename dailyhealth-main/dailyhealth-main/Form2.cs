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
                MessageBox.Show("No saved records found!", "Information",
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

           
            double avgBMI = records.Average(r => r.BMI);
            double avgWeight = records.Average(r => r.Weight);
            double avgWater = records.Average(r => r.WaterGlasses);
            double avgSleep = records.Average(r => r.SleepHours);
            double avgScore = records.Average(r => r.HealthScore);

            
            var latest = records.Last();
            var oldest = records.First();
            double weightChange = latest.Weight - oldest.Weight;

            
            txtStats.Clear();
            txtStats.AppendText("========== STATISTICS ==========\r\n\r\n");
            txtStats.AppendText($"Total records: {records.Count}\r\n");
            txtStats.AppendText($"Period: {oldest.Date:dd.MM.yyyy} - {latest.Date:dd.MM.yyyy}\r\n\r\n");

            txtStats.AppendText("--- Average Indicators ---\r\n");
            txtStats.AppendText($"BMI: {avgBMI:F2}\r\n");
            txtStats.AppendText($"Weight: {avgWeight:F1} kg\r\n");
            txtStats.AppendText($"Water: {avgWater:F1} glasses/day\r\n");
            txtStats.AppendText($"Sleep: {avgSleep:F1} hours/day\r\n");
            txtStats.AppendText($"Health Score: {avgScore:F0}%\r\n\r\n");

            txtStats.AppendText("--- Changes ---\r\n");
            txtStats.AppendText($"Weight Change: {(weightChange >= 0 ? "+" : "")}{weightChange:F1} kg\r\n");
            txtStats.AppendText($"Current Weight: {latest.Weight:F1} kg\r\n");
            txtStats.AppendText($"Starting Weight: {oldest.Weight:F1} kg\r\n\r\n");

          
            var bmiGroups = records.GroupBy(r => r.BMICategory);
            txtStats.AppendText("--- BMI Distribution ---\r\n");
            foreach (var group in bmiGroups)
            {
                double percentage = (group.Count() * 100.0) / records.Count;
                txtStats.AppendText($"{group.Key}: {group.Count()} times ({percentage:F0}%)\r\n");
            }

            
            txtStats.AppendText("\r\n--- Activity Levels ---\r\n");
            var activityGroups = records.GroupBy(r => r.ActivityLevel);
            foreach (var group in activityGroups)
            {
                double percentage = (group.Count() * 100.0) / records.Count;
                txtStats.AppendText($"{group.Key}: {group.Count()} times ({percentage:F0}%)\r\n");
            }

          
            var bestDay = records.OrderByDescending(r => r.HealthScore).First();
            txtStats.AppendText($"\r\n--- Best Day ---\r\n");
            txtStats.AppendText($"Date: {bestDay.Date:dd.MM.yyyy}\r\n");
            txtStats.AppendText($"Score: {bestDay.HealthScore}%\r\n");
            txtStats.AppendText($"BMI: {bestDay.BMI:F2}\r\n");
            txtStats.AppendText($"Water: {bestDay.WaterGlasses} glasses\r\n");
            txtStats.AppendText($"Sleep: {bestDay.SleepHours:F1} hours\r\n");
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
                MessageBox.Show("Please select a record to delete!", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Delete the selected record?", "Confirmation",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                int index = lvHistory.SelectedIndices[0];
                int recordIndex = Form1.AllRecords.Count - 1 - index;

                Form1.AllRecords.RemoveAt(recordIndex);
                LoadHistory();
                CalculateStatistics();

                MessageBox.Show("Record deleted!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (Form1.AllRecords.Count == 0)
            {
                MessageBox.Show("No data available for export!", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "CSV files (*.csv)|*.csv|Text files (*.txt)|*.txt";
            saveDialog.FileName = $"health_data_{DateTime.Now:yyyyMMdd}.csv";

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(saveDialog.FileName))
                    {
                     
                        file.WriteLine("Date,Time,Name,Weight,BMI,Category,Water,Sleep,Activity,Score");

                        foreach (var record in Form1.AllRecords)
                        {
                            file.WriteLine($"{record.Date:dd.MM.yyyy},{record.Date:HH:mm}," +
                                         $"{record.Name},{record.Weight:F1},{record.BMI:F2}," +
                                         $"{record.BMICategory},{record.WaterGlasses}," +
                                         $"{record.SleepHours:F1},{record.ActivityLevel},{record.HealthScore}");
                        }
                    }

                    MessageBox.Show("Data exported successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Export error: {ex.Message}", "Error",
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

                string details = $"=== RECORD DETAILS ===\r\n\r\n" +
                               $"Date and Time: {record.Date:dd.MM.yyyy HH:mm}\r\n\r\n" +
                               $"Personal Data:\r\n" +
                               $"  Name: {record.Name}\r\n" +
                               $"  Age: {record.Age}\r\n" +
                               $"  Gender: {record.Gender}\r\n" +
                               $"  Height: {record.Height} cm\r\n" +
                               $"  Weight: {record.Weight} kg\r\n\r\n" +
                               $"Health Indicators:\r\n" +
                               $"  BMI: {record.BMI:F2} ({record.BMICategory})\r\n" +
                               $"  Water: {record.WaterGlasses} glasses\r\n" +
                               $"  Sleep: {record.SleepHours} hours\r\n" +
                               $"  Activity: {record.ActivityLevel}\r\n\r\n" +
                               $"Nutrition:\r\n" +
                               $"  Breakfast: {(record.HadBreakfast ? "Yes" : "No")}\r\n" +
                               $"  Lunch: {(record.HadLunch ? "Yes" : "No")}\r\n" +
                               $"  Dinner: {(record.HadDinner ? "Yes" : "No")}\r\n\r\n" +
                               $"Overall Score: {record.HealthScore}%";

                MessageBox.Show(details, "Record Details",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}