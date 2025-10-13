namespace calculator4
{
    public partial class Form1 : Form
    {

        double a = 0;
        double b = 0;
        string op = "";
        public Form1()
        {
            InitializeComponent();
        }

        private void Number_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            if (textBox1.Text == "0" || textBox1.Text == "Error")
                textBox1.Text = b.Text;
            else
                textBox1.Text += b.Text;

        }

        private void Op_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            a = double.Parse(textBox1.Text);
            op = b.Text;
            textBox1.Text = "0";
        }

        private void C_Click(object sender, EventArgs e)
        {
            textBox1.Text = "0";
            a = 0;
            b = 0;
            op = "";
        }

        private void CE_Click(object sender, EventArgs e)
        {
            textBox1.Text = "0";
        }

        private void Equals_Click(object sender, EventArgs e)
        {
            b = double.Parse(textBox1.Text);
            double result = 0;

            if (op == "+")
                result = a + b;
            else if (op == "-")
                result = a - b;
            else if (op == "x" || op == "*")
                result = a * b;
            else if (op == "/")
            {
                if (b != 0)
                    result = a / b;
                else
                {
                    MessageBox.Show("Cannot divide by zero!", "Error");
                    return;
                }
            }
            

            textBox1.Text = result.ToString();


        }

        private void Percent_Click(object sender, EventArgs e)
        {
            double value = double.Parse(textBox1.Text);
            textBox1.Text = (value / 100).ToString();
        }

        private void Sqrt_Click(object sender, EventArgs e)
        {
            double value = double.Parse(textBox1.Text);
            if (value >= 0)
                textBox1.Text = Math.Sqrt(value).ToString();
            else
                textBox1.Text = "Error";
        }

        
    }
}
