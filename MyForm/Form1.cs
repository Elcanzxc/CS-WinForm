namespace MyForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void CheckPassword_Click(object sender, EventArgs e)
        {
            // mene bir hisse maraqlidi, indi c# kecmeyenler bu misali nece yazsin, men indi Akademiyada oxuyuram diye DotNet kecmisem
            // kodun her bir hissesini izah ede bilerem eger lazim olarsa

            string password = InputPassword.Text;
            string errorMessage = "";

            if (string.IsNullOrEmpty(password))
            {
                errorMessage = "Şifrə boş ola bilməz.";
            }
            else if (password.Length < 8)
            {
                errorMessage = "Şifrədə ən azı 8 simvol olmalıdır.";
            }
            else if (!password.Any(char.IsUpper))
            {
                errorMessage = "Şifrədə ən azı bir böyük hərf (A-Z) olmalıdır.";
            }
            else if (!password.Any(char.IsLower))
            {
                errorMessage = "Şifrədə ən azı bir kiçik hərf (a-z) olmalıdır.";
            }
            else if (!password.Any(char.IsDigit))
            {
                errorMessage = "Şifrədə ən azı bir rəqəm (0-9) olmalıdır.";
            }
            else if (!password.Any(c => "@#$%!*".Contains(c)))
            {
                errorMessage = "Şifrədə ən azı bir xüsusi işarə (@, #, $, %, !, *) olmalıdır.";
            }
            else
            {

                bool isPasswordHasSequence = false;
                for (int i = 0; i < password.Length - 2; i++)
                {

                    if (char.IsDigit(password[i]) && char.IsDigit(password[i + 1]) && char.IsDigit(password[i + 2]))
                    {
                        int first = password[i] - '0';
                        int second = password[i + 1] - '0';
                        int third = password[i + 2] - '0';

                        if (second == first + 1 && third == second + 1)
                        {
                            isPasswordHasSequence = true;
                            break;
                        }
                    }

                    if (char.IsLetter(password[i]) && char.IsLetter(password[i + 1]) && char.IsLetter(password[i + 2]))
                    {
                        int first = char.ToLower(password[i]);
                        int second = char.ToLower(password[i + 1]);
                        int third = char.ToLower(password[i + 2]);

                        if (second == first + 1 && third == second + 1)
                        {
                            isPasswordHasSequence = true;
                            break;
                        }
                    }
                }

                if (isPasswordHasSequence)
                {
                    errorMessage = "Şifrədə '123' və ya 'abc' kimi ardıcıl simvollar olmamalıdır.";
                }
            }

            if (!string.IsNullOrEmpty(errorMessage))
            {
                MessageBox.Show(errorMessage, "Xəta");
            }
            else
            {
                MessageBox.Show("Şifrə qəbul edildi!", "Uğurlu");
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }
    }
}
