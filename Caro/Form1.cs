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
            ChessBoard local = new ChessBoard();
            local.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CaroAI local = new CaroAI();
            local.ShowDialog();
        }
    }
}
