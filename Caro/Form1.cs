namespace Caro
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            BanCo banCo = new BanCo();
            banCo.ShowDialog();
        }
    }
}
