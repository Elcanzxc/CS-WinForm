using System;
using System.Text;
using System.Windows.Forms;

namespace Exercise___9
{
    public partial class Form1 : Form
    {

        private Label[] noteLabels;

        public Form1()
        {
            InitializeComponent();

            noteLabels = new Label[] { label9, label8, label7, label6, label5, label4, label3, label2 };
            
            ClearCounts();
        }

     
        private void button2_Click(object sender, EventArgs e)
        {
            ClearCounts();
        }

    
        private void button1_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(textBox1.Text, out int amount) || amount < 0)
            {
                MessageBox.Show("Xahiş edirik, düzgün müsbət məbləği daxil edin.", "Daxil etmə səhvi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

          
            if (amount == 0)
            {
                ClearCounts();
                return;
            }

            int[] denominations = { 500, 200, 100, 50, 20, 10, 5, 1 };
            int[] noteCounts = new int[denominations.Length]; 

            var resultBuilder = new StringBuilder();
            resultBuilder.AppendLine($"{amount} AZN məbləğini vermək üçün istifadə olunan əskinazlar:");
            resultBuilder.AppendLine("----------------------------------");

            int remainingAmount = amount;


            for (int i = 0; i < denominations.Length; i++)
            {
                int note = denominations[i];
                int count = remainingAmount / note;
                noteCounts[i] = count; 

                if (count > 0)
                {
                    resultBuilder.AppendLine($"{count} x {note} AZN");
                    remainingAmount %= note;
                }
            }

            UpdateLabels(noteCounts);

            MessageBox.Show(resultBuilder.ToString(), "Vermə nəticəsi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void UpdateLabels(int[] counts)
        {
            for (int i = 0; i < noteLabels.Length; i++)
            {
                noteLabels[i].Text = counts[i].ToString();
            }
        }

    
        private void ClearCounts()
        {
            textBox1.Text = "0";
            foreach (Label lbl in noteLabels)
            {
                lbl.Text = "0";
            }
        }

  
        private void AddMoney(int amountToAdd, Label labelToUpdate)
        {
     
            int currentAmount = 0;
            int.TryParse(textBox1.Text, out currentAmount);
            textBox1.Text = (currentAmount + amountToAdd).ToString();


            int currentLabelCount = 0;
            int.TryParse(labelToUpdate.Text, out currentLabelCount);
            labelToUpdate.Text = (currentLabelCount + 1).ToString();
        }



        private void pictureBox8_Click(object sender, EventArgs e) 
        {
            AddMoney(500, label9);
        }

        private void pictureBox7_Click(object sender, EventArgs e) 
        {
            AddMoney(200, label8);
        }

        private void pictureBox6_Click(object sender, EventArgs e) 
        {
            AddMoney(100, label7);
        }

        private void pictureBox5_Click(object sender, EventArgs e) 
        {
            AddMoney(50, label6);
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            AddMoney(20, label5);
        }

        private void pictureBox3_Click(object sender, EventArgs e) 
        {
            AddMoney(10, label4);
        }

        private void pictureBox2_Click(object sender, EventArgs e) 
        {
            AddMoney(5, label3);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            AddMoney(1, label2);
        }

        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
        private void label4_Click(object sender, EventArgs e) { }
        private void label5_Click(object sender, EventArgs e) { }
        private void label6_Click(object sender, EventArgs e) { }
        private void label7_Click(object sender, EventArgs e) { }
        private void label8_Click(object sender, EventArgs e) { }
        private void label9_Click(object sender, EventArgs e) { }
    }
}