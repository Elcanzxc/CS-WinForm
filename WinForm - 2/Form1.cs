namespace WinForm
{
    public partial class Form : System.Windows.Forms.Form
    {
        public Form()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void groupBox_Enter(object sender, EventArgs e)
        {

        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            string cinsiyet = radioMale.Checked ? "Kişi" :
                              radioFemale.Checked ? "Qadın" : "Göstərilməyib";



            string dil = InputLanguage.SelectedItem != null ? InputLanguage.SelectedItem.ToString() : "Seçilməyib";



            string proqramBiliyi = "";
            foreach (var item in checkedListBox.CheckedItems)
            {
                proqramBiliyi += item.ToString() + " ";
            }


            if (proqramBiliyi == "") proqramBiliyi = "Göstərilməyib";



            string seher = InputLiveCity.Text != "" ? InputLiveCity.Text : "Göstərilməyib";
            string universitet = InputUniversity.Text != "" ? InputUniversity.Text : "Göstərilməyib";



            string mesaj = $"Cinsiyyət: {cinsiyet}\n" +
                           $"Dil: {dil}\n" +
                           $"Proqram biliyi: {proqramBiliyi}\n" +
                           $"Yaşadığı yer: {seher}\n" +
                           $"Universitet: {universitet}";


            MessageBox.Show(mesaj, "Məlumat");

        }

        private void radioMale_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
