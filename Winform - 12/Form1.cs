using System.Media;

namespace Bomb_Rain
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Random r = new Random();
        int s1 = 0, s2 = 0, score = 0;

        private void Form1_Load(object sender, EventArgs e)
        {
            bomb1.Location = new Point(r.Next(0, 575), r.Next(0, 100));
            bomb2.Location = new Point(r.Next(0, 575), r.Next(0, 100));
            if (bomb1.Bounds.IntersectsWith(bomb2.Bounds))
            {
                Form1_Load(sender, e);
            }
            else
            {
                MessageBox.Show("Game Started use <- and -> , every avoidness is 10 point", "Information", MessageBoxButtons.OK);
                man.Location = new Point(250, 290);
                left.Location = new Point(man.Location.X, man.Location.Y + 15);
                right.Location = new Point(man.Location.X, man.Location.Y + 15);
                man.Visible = true;
                left.Visible = false;
                right.Visible = false;
                s1 = 0;
                s2 = 0;
                score = 0;
                timer1.Start();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (left.Bounds.IntersectsWith(bomb1.Bounds) || left.Bounds.IntersectsWith(bomb2.Bounds) ||
               right.Bounds.IntersectsWith(bomb1.Bounds) || right.Bounds.IntersectsWith(bomb2.Bounds) ||
               man.Bounds.IntersectsWith(bomb1.Bounds) || man.Bounds.IntersectsWith(bomb2.Bounds))
            {
                timer1.Stop();
                SoundPlayer sound = new SoundPlayer("C:\\Users\\user\\Downloads\\explosion.wav");
                sound.Play();
                DialogResult d = MessageBox.Show($"Your Score : {score}\n" + $"You Lose , Want to restart?", "Information", MessageBoxButtons.YesNo);
                if (d == DialogResult.Yes)
                {
                    Form1_Load(sender, e);
                }
                else
                {
                    this.Close();
                }
            }
            if (bomb1.Location.Y >= 560)
            {
                bomb1.Location = new Point(r.Next(0, 575), r.Next(0, 100));
                if (bomb1.Bounds.IntersectsWith(bomb2.Bounds))
                {
                    while (bomb1.Bounds.IntersectsWith(bomb2.Bounds))
                        bomb1.Location = new Point(r.Next(0, 575), r.Next(0, 100));
                }
                s1 = 0;
                score += 10;
            }
            if (bomb2.Location.Y >= 560)
            {
                bomb2.Location = new Point(r.Next(0, 575), r.Next(0, 100));
                if (bomb1.Bounds.IntersectsWith(bomb2.Bounds))
                {
                    while (bomb1.Bounds.IntersectsWith(bomb2.Bounds))
                        bomb2.Location = new Point(r.Next(0, 575), r.Next(0, 100));
                }
                s2 = 0;
                score += 10;
            }
            s1++;
            s2++;
            bomb1.Top += s1;
            bomb2.Top += s2;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                man.Visible = false;
                right.Visible = false;
                left.Visible = true;
                if (left.Left - 10 > 0)
                {
                    left.Left -= 10;
                }
                else
                {
                    left.Left = 0;
                }
                right.Location = left.Location;
            }
            if (e.KeyCode == Keys.Right)
            {
                man.Visible = false;
                left.Visible = false;
                right.Visible = true;
                if (right.Left + 10 < 550)
                {
                    right.Left += 10;
                }
                else
                {
                    right.Left = 550;
                }
                left.Location = right.Location;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            man.Location = new Point(left.Location.X, left.Location.Y - 15);
            if (right.Left == 550)
            {
                man.Left = 596;
            }
            man.Visible = true;
            if (left.Visible)
            {
                left.Visible = false;
            }
            if (right.Visible)
            {
                right.Visible = false;
            }
        }
    }
}
