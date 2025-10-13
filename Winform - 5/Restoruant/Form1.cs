using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Restoruant
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            string name = "Plovustu";
            int price = 7;
            listBox1.Items.Add($"{name} - {price} AZN");
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            string name = "Karto Fri";
            int price = 3;
            listBox1.Items.Add($"{name} - {price} AZN");
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            string name = "Lays";
            int price = 2;
            listBox1.Items.Add($"{name} - {price} AZN");
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            string name = "Xengel";
            int price = 1;
            listBox1.Items.Add($"{name} - {price} AZN");
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            string name = "Burger";
            int price = 3;
            listBox1.Items.Add($"{name} - {price} AZN");
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            string name = "Makaron";
            int price = 2;
            listBox1.Items.Add($"{name} - {price} AZN");
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            string name = "Taco";
            int price = 4;
            listBox1.Items.Add($"{name} - {price} AZN");
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            string name = "Corek";
            int price = 1;
            listBox1.Items.Add($"{name} - {price} AZN");
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            string name = "Et ve salat";
            int price = 9;
            listBox1.Items.Add($"{name} - {price} AZN");
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            string name = "Hot-dog";
            int price = 2;
            listBox1.Items.Add($"{name} - {price} AZN");
        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {
            string name = "Krousan";
            int price = 2;
            listBox1.Items.Add($"{name} - {price} AZN");
        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {
            string name = "Limonad";
            int price = 2;
            listBox1.Items.Add($"{name} - {price} AZN");
        }

        private void pictureBox13_Click(object sender, EventArgs e)
        {
      
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count > 0)
            {
                listBox1.Items.RemoveAt(listBox1.Items.Count - 1);
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            textBox1.Clear();
            textBox2.Clear();
        }


        private void button3_Click(object sender, EventArgs e)
        {
            int total = 0;
            foreach (var item in listBox1.Items)
            {
                string str = item.ToString();
                string[] parts = str.Split('-');
                if (parts.Length == 2)
                {
                    string pricePart = parts[1].Replace("AZN", "").Trim();
                    if (int.TryParse(pricePart, out int price))
                    {
                        total += price;
                    }
                }
            }
            textBox1.Text = total.ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBox1.Text, out int total) &&
                int.TryParse(textBox2.Text, out int money))
            {
                if (money >= total)
                {
                    MessageBox.Show("Pulunuz kifayətdir.");
                }
                else
                {
                    MessageBox.Show("Pulunuz çatmır.");
                }
            }
            else
            {
                MessageBox.Show("Zəhmət olmasa düzgün dəyərlər daxil edin.");
            }
        }


        private void button5_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBox1.Text, out int total) &&
                int.TryParse(textBox2.Text, out int money))
            {
                int change = money - total;
                if (change >= 0)
                {
                    MessageBox.Show($"Qalıq: {change} AZN");
                }
                else
                {
                    MessageBox.Show("Qalıq yoxdur, pul çatmır.");
                }
            }

            textBox2.Clear();
        }


        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void textBox2_TextChanged(object sender, EventArgs e) { }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
        private void panel1_Paint(object sender, PaintEventArgs e) { }
        private void panel2_Paint(object sender, PaintEventArgs e) { }
        private void Form1_Load(object sender, EventArgs e) { }
    }
}
