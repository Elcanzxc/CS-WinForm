using static System.Formats.Asn1.AsnWriter;

namespace ArkanoidGame
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        bool isRight = false;
        bool isLeft = false;
        int panelSpeed = 12;
        int topYSpeed = 6;
        int topXSpeed = 1;
        int score = 0;
        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (isRight)
            {
                if (panel1.Right <= this.ClientSize.Width) panel1.Left += panelSpeed;
            }
            if (isLeft)
            {
                if (panel1.Left >= 0) panel1.Left -= panelSpeed;
            }

            top.Left += topXSpeed;
            top.Top += topYSpeed;

            if (top.Left <= 0 || top.Right >= this.ClientSize.Width) topXSpeed = -topXSpeed;

            if (top.Top <= 0) topYSpeed = -topYSpeed;

            if (top.Bounds.IntersectsWith(panel1.Bounds) && topYSpeed > 0)
            {
                topYSpeed = -Math.Abs(topYSpeed);
                score++;
            }

            if (top.Top > this.ClientSize.Height)
            {
                timer1.Stop();
                DialogResult d = MessageBox.Show("Top aşağı düşdü! Topladığınız xal: " + score + "\nYeni oyun oynamaq istəyirsiniz?", "Oyun Bitdi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (d == DialogResult.Yes)
                {
                    Application.Restart();
                }
                else
                {
                    Close();
                }
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                isRight = false;
                isLeft = true;
            }
            if (e.KeyCode == Keys.Right)
            {
                isLeft = false;
                isRight = true;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left) isLeft = false;
            if (e.KeyCode == Keys.Right) isRight = false;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}