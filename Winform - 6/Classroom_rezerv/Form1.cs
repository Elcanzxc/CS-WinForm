namespace Classroom_rezerv
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        int counter1 = 0;
        int counter2 = 0;
        int counter3 = 0;
        int counter4 = 0;
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (maskedTextBox1.Text.Equals("") || textBox1.Text.Equals("")) MessageBox.Show("Zəhmət olmasa müddət və ya qrup adını daxil edin");
            else
            {
                timer1.Start();
                int.TryParse(maskedTextBox1.Text, out counter1);
                textBox2.Text = counter1.ToString();
                groupBox1.BackColor = Color.Red;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (counter1 > 0)
            {
                counter1--;
                textBox2.Text = counter1.ToString();
            }

            if (counter1 == 0)
            {
                timer1.Stop();
                groupBox1.BackColor = Color.Green;
                textBox1.Clear();
                maskedTextBox1.Clear();
                textBox2.Clear();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (maskedTextBox2.Text.Equals("") || textBox4.Text.Equals("")) MessageBox.Show("Zəhmət olmasa müddət və ya qrup adını daxil edin");
            else
            {
                timer2.Start();
                int.TryParse(maskedTextBox2.Text, out counter2);
                textBox3.Text = counter2.ToString();
                groupBox2.BackColor = Color.Red;
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (counter2 > 0)
            {
                counter2--;
                textBox3.Text = counter2.ToString();
            }

            if (counter2 == 0)
            {
                timer2.Stop();
                groupBox2.BackColor = Color.Green;
                textBox4.Clear();
                maskedTextBox2.Clear();
                textBox3.Clear();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (maskedTextBox3.Text.Equals("") || textBox6.Text.Equals("")) MessageBox.Show("Zəhmət olmasa müddət və ya qrup adını daxil edin");
            else
            {
                timer3.Start();
                int.TryParse(maskedTextBox3.Text, out counter3);
                textBox5.Text = counter3.ToString();
                groupBox3.BackColor = Color.Red;
            }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            if (counter3 > 0)
            {
                counter3--;
                textBox5.Text = counter3.ToString();
            }

            if (counter3 == 0)
            {
                timer3.Stop();
                groupBox3.BackColor = Color.Green;
                textBox6.Clear();
                maskedTextBox3.Clear();
                textBox5.Clear();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (maskedTextBox4.Text.Equals("") || textBox8.Text.Equals("")) MessageBox.Show("Zəhmət olmasa müddət və ya qrup adını daxil edin");
            else
            {
                timer4.Start();
                int.TryParse(maskedTextBox4.Text, out counter4);
                textBox7.Text = counter4.ToString();
                groupBox4.BackColor = Color.Red;
            }
        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            if (counter4 > 0)
            {
                counter4--;
                textBox7.Text = counter4.ToString();
            }

            if (counter4 == 0)
            {
                timer4.Stop();
                groupBox4.BackColor = Color.Green;
                textBox8.Clear();
                maskedTextBox4.Clear();
                textBox7.Clear();
            }
        }
    }
}
