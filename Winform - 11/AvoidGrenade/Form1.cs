namespace AvoidGrenade
{
    public partial class Form1 : Form
    {


        Random random = new Random();

        int count = 0;
        int grenadeX;
        int grenadeY;
        public Form1()
        {
            InitializeComponent();
            grenadeX = random.Next(0, 546);
            grenadeY = random.Next(0, 345);
            grenade.Location = new Point(grenadeX, grenadeY);
        }

        private void btnCreatePanel_Click(object sender, EventArgs e)
        {
            int panelX = random.Next(0, 546);
            int panelY = random.Next(0, 345);

            count++;
            textBox1.Text = count.ToString();

            Panel panel = new Panel();
            panel.Size = new Size(60, 60);
            panel.BackColor = Color.Red;
            panel.Location = new Point(panelX, panelY);
            panel1.Controls.Add(panel);
            panel.BringToFront();

            if (panelX + 60 >= grenadeX &&
                panelX <= grenadeX + 60 &&
                panelY + 60 >= grenadeY &&
                panelY <= grenadeY + 60)
            {
                MessageBox.Show("Bomba partladý! \nYaradýlan panel sayisi: " + count, "Uduzdunuz", MessageBoxButtons.OK);
                panel.BackColor = Color.DarkRed;
                btnCreatePanel.Enabled = false;
                return;
            }



        }

        private void btnNewGame_Click(object sender, EventArgs e)
        {
            for (int i = panel1.Controls.Count - 1; i >= 0; i--)
            {
                if (panel1.Controls[i].Name != "grenade")
                {
                    panel1.Controls.RemoveAt(i);
                }
            }

            count = 0;
            textBox1.Text = "0";
            btnCreatePanel.Enabled = true;

            grenadeX = random.Next(0, 546);
            grenadeY = random.Next(0, 345);
            grenade.Location = new Point(grenadeX, grenadeY);

        }

        private void grenade_Click(object sender, EventArgs e)
        {

        }
    }
}
