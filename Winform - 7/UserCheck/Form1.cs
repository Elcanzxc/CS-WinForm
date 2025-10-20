using Dapper;
using Microsoft.Data.SqlClient;


namespace UserCheck
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string connectionString = "Server=localhost;Database=UserDb;User Id=admin;Password=admin;TrustServerCertificate=True;";




        private void button1_Click(object sender, EventArgs e)
        {

            if (textBox1.Text == "" && textBox2.Text == "")
            {
                MessageBox.Show("Please enter username and password", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                String user_name = textBox1.Text;
                String pass_word = textBox2.Text;
                String date_now = DateTime.Now.ToString();
                dataGridView1.Rows.Add(user_name, pass_word, date_now);

                using (var connection = new SqlConnection(connectionString))
                {
                    string sql = "INSERT INTO Users (Name, Password) VALUES (@Name, @Password)";
                    connection.Execute(sql, new { Name = user_name, Password = pass_word });
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.Show();
            this.Hide();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
