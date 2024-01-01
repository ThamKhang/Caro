using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Caro
{
    public partial class CaroAI : Form
    {
        private Button[,] buttons;
        private int columnCount, rowCount;
        private CurrentTurn currentTurn = CurrentTurn.X;
        public CaroAI()
        {
            InitializeComponent();
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            tableLayoutPanel1.Controls.Clear();
            columnCount = tableLayoutPanel1.ColumnCount;
            rowCount = tableLayoutPanel1.RowCount;
            buttons = new Button[rowCount, columnCount];

            for (int row = 0; row < rowCount; row++)
            {
                for (int col = 0; col < columnCount; col++)
                {
                    Button button = new Button
                    {
                        Dock = DockStyle.Fill,
                        Margin = Padding.Empty,
                        Tag = new Tuple<int, int>(row, col),
                    };
                    button.Click += Button_Click;
                    tableLayoutPanel1.Controls.Add(button, col, row);
                    buttons[row, col] = button;
                }
            }
        }

        // Xử lý sự kiện nhấn nút
        private void Button_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            if (button.Text != "") return;

            // Người chơi đánh
            MakeMove(button, currentTurn);

            // Kiểm tra kết thúc trò chơi sau nước đi của người chơi
            if (CheckEndGame()) return;

            // Máy đánh
            MakeAIMove();
            // Kiểm tra kết thúc trò chơi sau nước đi của máy
            CheckEndGame();
        }

        private void MakeMove(Button button, CurrentTurn turn)
        {
            button.Text = turn == CurrentTurn.X ? "X" : "O";
            button.ForeColor = turn == CurrentTurn.X ? Color.Red : Color.Blue;
            button.Font = new Font(button.Font, FontStyle.Bold);
            currentTurn = currentTurn == CurrentTurn.X ? CurrentTurn.O : CurrentTurn.X;
        }

        private void MakeAIMove()
        {
            // AI đơn giản: chọn một ô trống ngẫu nhiên
            var emptyButtons = buttons.Cast<Button>().Where(b => b.Text == "").ToList();
            if (emptyButtons.Count > 0)
            {
                var random = new Random();
                var button = emptyButtons[random.Next(emptyButtons.Count)];
                MakeMove(button, currentTurn);
            }
        }

        private bool CheckEndGame()
        {
            string winner = CheckWinner();
            if (!string.IsNullOrEmpty(winner))
            {
                MessageBox.Show($"{winner} chiến thắng!");
                InitializeBoard();
                currentTurn = CurrentTurn.X;
                return true;
            }
            if (IsBoardFull())
            {
                MessageBox.Show("Hòa!");
                InitializeBoard();
                currentTurn = CurrentTurn.X;
                return true;
            }
            return false;
        }

        // Cập nhật phương thức CheckWinner để không cần thông tin về hàng và cột cụ thể
        private string CheckWinner()
        {
            // Kiểm tra từng ô trên bảng
            for (int row = 0; row < rowCount; row++)
            {
                for (int col = 0; col < columnCount; col++)
                {
                    string currentPlayer = buttons[row, col].Text;
                    if (!string.IsNullOrEmpty(currentPlayer))
                    {
                        if (CheckLine(row, col, 0, 1, currentPlayer) || CheckLine(row, col, 1, 0, currentPlayer) ||
                            CheckLine(row, col, 1, 1, currentPlayer) || CheckLine(row, col, 1, -1, currentPlayer))
                            return currentPlayer;
                    }
                }
            }
            return null;
        }


        // Kiểm tra 5 ô liên tiếp
        private bool CheckLine(int row, int col, int deltaRow, int deltaCol, string player)
        {
            int count = 0;
            for (int i = -4; i <= 4; i++)
            {
                int r = row + i * deltaRow;
                int c = col + i * deltaCol;
                if (r < 0 || r >= rowCount || c < 0 || c >= columnCount) continue;
                if (buttons[r, c].Text == player)
                {
                    count++;
                    if (count == 5) return true;
                }
                else count = 0;
            }
            return false;
        }

        // Kiểm tra xem bàn cờ đã đầy chưa
        private bool IsBoardFull()
        {
            for (int i = 0; i < rowCount; i++)
                for (int j = 0; j < columnCount; j++)
                    if (string.IsNullOrEmpty(buttons[i, j].Text))
                        return false;
            return true;
        }

        // Enum định nghĩa lượt đi
        private enum CurrentTurn { X, O }
    }
}
