using System.Security;

namespace calculator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            double n1 = Convert.ToDouble(num1.Text);
            double n2 = Convert.ToDouble(num2.Text);
            String k = comboop.Text;
            double result = 0;
            if (k == "+")
            {
                result = n1 + n2;
            }
            else if (k == "-")
            {
                result = n1 - n2;
            }
            else if (k == "*")
            {
                result = n1 * n2;
            }
            else if (k == "/")
            {
                if (n2 == 0)
                {
                    MessageBox.Show("0-a bolmek olmaz");
                }
            }
            else
            {
                result = n1 / n2;
            }

            history.Items.Add($"{n1}{k}{n2}={result}");

        }

        private void comboop_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
