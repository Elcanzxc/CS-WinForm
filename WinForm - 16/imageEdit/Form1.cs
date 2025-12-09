using System.Drawing.Drawing2D;

namespace imageEdit
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }
        Bitmap bmp;
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string path = openFileDialog1.FileName;
                bmp = new Bitmap(path);
                pictureBox2.Image = bmp;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog sv = new SaveFileDialog();
            if (DialogResult.OK == sv.ShowDialog())
            {
                string path = sv.FileName;
                bmp.Save(path);
                MessageBox.Show("Image saved successfully!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Graphics gr = Graphics.FromImage(bmp);
            LinearGradientBrush liner = new LinearGradientBrush(
                new Point(0, 0), new Point(bmp.Width, 0), Color.FromArgb(50, 225, 128, 172), Color.FromArgb(50, 52, 58, 178));
            gr.FillRectangle(liner, 0, 0, bmp.Width, bmp.Height);
            pictureBox2.Image = bmp;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            Graphics gr = Graphics.FromImage(bmp);
            LinearGradientBrush liner = new LinearGradientBrush(
                new Point(0, 0), new Point(bmp.Width, 0),
                Color.FromArgb(60, 30, 30, 30),
                Color.FromArgb(60, 80, 0, 0));
            gr.FillRectangle(liner, 0, 0, bmp.Width, bmp.Height);
            pictureBox2.Image = bmp;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Graphics gr = Graphics.FromImage(bmp);
            LinearGradientBrush liner = new LinearGradientBrush(
                new Point(0, 0), new Point(bmp.Width, 0),
                Color.FromArgb(60, 180, 220, 255),
                Color.FromArgb(60, 100, 140, 200));
            gr.FillRectangle(liner, 0, 0, bmp.Width, bmp.Height);
            pictureBox2.Image = bmp;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Graphics gr = Graphics.FromImage(bmp);
            LinearGradientBrush liner = new LinearGradientBrush(
                new Point(0, 0), new Point(bmp.Width, 0),
                Color.FromArgb(60, 255, 255, 200),
                Color.FromArgb(60, 255, 220, 120));
            gr.FillRectangle(liner, 0, 0, bmp.Width, bmp.Height);
            pictureBox2.Image = bmp;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Graphics gr = Graphics.FromImage(bmp);
            LinearGradientBrush liner = new LinearGradientBrush(
                new Point(0, 0), new Point(bmp.Width, 0),
                Color.FromArgb(60, 180, 255, 180),
                Color.FromArgb(60, 120, 200, 140));
            gr.FillRectangle(liner, 0, 0, bmp.Width, bmp.Height);
            pictureBox2.Image = bmp;
        }
        
        private void button9_Click(object sender, EventArgs e)
        {
            Graphics gr = Graphics.FromImage(bmp);
            LinearGradientBrush liner = new LinearGradientBrush(
                new Point(0, 0), new Point(bmp.Width, 0),
                Color.FromArgb(60, 255, 240, 150),  
                Color.FromArgb(60, 255, 200, 80));   
            gr.FillRectangle(liner, 0, 0, bmp.Width, bmp.Height);
            pictureBox2.Image = bmp;
        }
    }
}
