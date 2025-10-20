using Dapper;
using Microsoft.Data.SqlClient;

namespace UserCheck
{
    public partial class Form2 : Form
    {
        string connectionString = "Server=localhost;Database=UserDb;User Id=admin;Password=admin;TrustServerCertificate=True;";
        public Form2()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string inputName = textBox1.Text.Trim();

            using (var connection = new SqlConnection(connectionString))
            {
                string sql = "SELECT Name, Password FROM Users WHERE LOWER(Name) = LOWER(@Name)";
                var user = connection.QueryFirstOrDefault<(string Name, string Password)>(sql, new { Name = inputName });

                if (user.Name != null)
                {
                    richTextBox1.Text = $"İstifadəçi tapıldı:\nUserName: {user.Name}\nPassword: {user.Password}";
                }
                else
                {
                    richTextBox1.Text = "İstifadəçi tapılmadı.";
                }
            }

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
