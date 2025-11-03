using System.Threading;

namespace WinFormsApp5
{
    public partial class Form1 : Form


    {

        int speed = 0; 
        int x = 0;  
        int y = -5;

        public Form1()
        {
            InitializeComponent();
            timer1.Interval = 50;  
            timer1.Tick += Timer1_Tick;
        }
        private void Timer1_Tick(object sender, EventArgs e)
        {
            car.Top += y * (speed / 5);
            car.Left += x;
        }

        private void btnBreak_Click(object sender, EventArgs e)
        {
          
            speed = 0; 
            timer1.Stop();

        }

        private void btnGas_Click(object sender, EventArgs e)
        {
            
            speed = 5;   
            timer1.Start();
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            x = -3;
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            x = 3;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            y = 3;
        }
    }
}
