using System.Media;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        bool isMoving = false;
        bool lightsOn = false;
        int speed = 5;
        private void button6_Click(object sender, EventArgs e)
        {
            isMoving = true;
            timer1.Start();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            isMoving = false;
            timer1.Stop();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (isMoving)
            {
                if (pictureBox1.Top >= 0)
                {
                    pictureBox1.Top -= speed;
                    pictureBox2.Top -= speed;
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            lightsOn = !lightsOn;

            if (lightsOn)
                pictureBox2.Visible = true;
            else
                pictureBox2.Visible = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
       
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SoundPlayer s1 = new SoundPlayer("C:\\Users\\student\\Desktop\\Task14\\inishazirliklari.wav");
            s1.Play();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SoundPlayer s5 = new SoundPlayer("C:\\Users\\student\\Desktop\\Task14\\turbulans.wav");
            s5.Play();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SoundPlayer s3 = new SoundPlayer("C:\\Users\\student\\Desktop\\Task14\\kemerler.wav");
            s3.Play();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SoundPlayer s4 = new SoundPlayer("C:\\Users\\student\\Desktop\\Task14\\tesekkurler.wav");
            s4.Play();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SoundPlayer s2 = new SoundPlayer("C:\\Users\\student\\Desktop\\Task14\\isikacmakapamasesi.wav");
            s2.Play();
        }
    }
}