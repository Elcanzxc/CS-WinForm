using System.Text.RegularExpressions;

namespace Card_Information
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        bool isInformationValid = false;

        private void check_Click(object sender, EventArgs e)
        {
            int count = card_number.Text.Count(c => char.IsDigit(c));
            if (count == 16 && (full_name.Text.Length >= 7 && full_name.Text.Count(c => char.IsDigit(c)) == 0 && full_name.Text.Trim().Count(c => c == ' ') == 1) &&
                Regex.IsMatch(until_date.Text, @"^(0[1-9]|1[0-2])/([2-9][6-9]|[3-9][0-9])$") && cvc.Text.Count(c => char.IsDigit(c)) == 3)
            {
                isInformationValid = true;
                card_number_label.Text = "";
                full_name_label.Text = "";
                until_date_label.Text = "";
                cvc_label.Text = "";
                MessageBox.Show("Valid information!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cvc_label.Text = cvc.Text;
                until_date_label.Text = until_date.Text;
                foreach (char c in full_name.Text)
                {
                    if (c == ' ')
                    {
                        full_name_label.Text += "  ";
                    }
                    else
                    {
                        full_name_label.Text += char.ToUpper(c);
                    }
                }
                count = 0;
                for (int i = 0; i < card_number.Text.Length; i++)
                {
                    if (count == 4 || count == 8 || count == 12)
                    {
                        card_number_label.Text += "      ";
                    }
                    if (char.IsDigit(card_number.Text[i]))
                    {
                        count++;
                        card_number_label.Text += card_number.Text[i];
                    }
                }
            }
            else
            {
                isInformationValid = false;
                MessageBox.Show("Invalid information!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        bool isEyeOpen = false;

        private void eye_Click(object sender, EventArgs e)
        {
            if (isEyeOpen && isInformationValid && cvc.Text.Length == 3)
            {
                eye.Image = Properties.Resources.close_eye;
                cvc_label.Text = "***";
                isEyeOpen = false;
            }
            else
            {
                if (isInformationValid && cvc.Text.Length == 3)
                {
                    eye.Image = Properties.Resources.open_eye;
                    cvc_label.Text = cvc.Text;
                    isEyeOpen = true;
                }
            }
        }
    }
}
