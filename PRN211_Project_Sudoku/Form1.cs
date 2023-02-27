namespace PRN211_Project_Sudoku
{
    public partial class Form1 : Form
    {
        SudokuCell[,] cells = new SudokuCell[9, 9];
        Random random = new Random();
        public Form1()
        {
            InitializeComponent();
            ShowLevel();
            CreateCells();
            StartGame();
        }
        private void ShowLevel()
        {
            cbLevel.Items.Add("Easy");
            cbLevel.Items.Add("Normal");
            cbLevel.Items.Add("Hard");
            cbLevel.Text = "Choose Level";
            
        }
        public void CreateCells()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    cells[i, j] = new SudokuCell();
                    cells[i, j].Font = new Font(SystemFonts.DefaultFont.FontFamily, 20);
                    cells[i, j].Size = new Size(60, 60);
                    cells[i, j].ForeColor = SystemColors.ControlDarkDark;
                    cells[i, j].Location = new Point(i * 60, j * 60);
                    cells[i, j].BackColor = ((i / 3) + (j / 3)) % 2 == 0 ? SystemColors.Control : Color.LightBlue;
                    cells[i, j].FlatStyle = FlatStyle.Flat;
                    cells[i, j].FlatAppearance.BorderColor = Color.Black;
                    cells[i, j].X = i;
                    cells[i, j].Y = j;

                    // Assign key press event for each cells
                    cells[i, j].KeyPress += CellKeyPressed;

                    panel1.Controls.Add(cells[i, j]);
                }
            }
        }
        public void CellKeyPressed(object sender, KeyPressEventArgs e)
        {
            var cell = sender as SudokuCell;

            // Do nothing if the cell is fixed
            if (cell.IsFixed)
                return;

            int value;

            // Add the pressed key value in the cell only if it is a number
            if (int.TryParse(e.KeyChar.ToString(), out value))
            {
                // Clear the cell value if pressed key is zero
                if (value == 0)
                    cell.Clear();
                else
                    cell.Text = value.ToString();

                cell.ForeColor = SystemColors.ControlDarkDark;
            }
        }
        private void StartGame()
        {
            LoadValues();
            int hints = GetHintByLevel();
            GenerateRandomPuzzle(hints);
        }
        private int GetHintByLevel()
        {
            int hints = 0;
            if (cbLevel.Text.Equals("Easy"))
            {
                hints = 45;
            }
            else if (cbLevel.Text.Equals("Normal"))
            {
                hints = 40;
            }
            else if (cbLevel.Text.Equals("Hard"))
            {
                hints = 30;
            }
            return hints;
        }
        private void GenerateRandomPuzzle(int hints)
        {
            for(int i = 0; i < hints; i++)
            {
                var randomX = random.Next(9);
                var randomY = random.Next(9);

                cells[randomX, randomY].Text = cells[randomX, randomY].Value.ToString();
                cells[randomX, randomY].ForeColor = Color.Black;
                cells[randomX, randomY].IsFixed = true;
            }
        }
        private void LoadValues()
        {
            foreach (var cell in cells)
            {
                cell.Value = 0;
                cell.Clear();
            }

            FindValueForNextCell(0, -1);
        }
        private bool FindValueForNextCell(int i, int j)
        {
            // Increase  i and j  to move to the next cell
            // and if the columns ends, move to the next row
            if (++j > 8)
            {
                j = 0;

                // Exit if the line ends
                if (++i > 8)
                    return true;
            }

            var value = 0;
            var numberLeft = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            do
            {
                if (numberLeft.Count < 1)
                {
                    cells[i, j].Value = 0;
                    return false;
                }
                value = numberLeft[random.Next(0, numberLeft.Count)];
                cells[i, j].Value = value;

                numberLeft.Remove(value);
            }
            while (!IsValidNumber(value, i, j) || !FindValueForNextCell(i, j));
            return true;
        }
        private bool IsValidNumber(int value, int x, int y)
        {
            for (int i = 0; i < 9; i++)
            {
                // Check all the cells in columns
                if (i != y && cells[x, i].Value == value)
                    return false;

                // Check all the cells in rows
                if (i != x && cells[i, y].Value == value)
                    return false;
            }

            // Check all the cells in a 3x3 block
            for (int i = x - (x % 3); i < x - (x % 3) + 3; i++)
            {
                for (int j = y - (y % 3); j < y - (y % 3) + 3; j++)
                {
                    if (i != x && j != y && cells[i, j].Value == value)
                        return false;
                }
            }
            return true;
        }
        private void btnNewGame_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Do you want to start a new game?", "New Game", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            StartGame();
        }
        private void btnRestart_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to restart this game?", "Restart", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                foreach (var cell in cells)
                {
                    if (cell.IsFixed == false)
                        cell.Clear();
                }
            }
        }
        private void btnCheck_Click(object sender, EventArgs e)
        {
            var wrongCell = new List<SudokuCell>();
            foreach (var cell in cells)
            {
                if (!string.Equals(cell.Value.ToString(), cell.Text))
                {
                    wrongCell.Add(cell);
                }
            }

            if (wrongCell.Any())
            {
                wrongCell.ForEach(cell => cell.ForeColor = Color.Red);
                MessageBox.Show("You still got mistakes!", "Check Answer");
            }
            else
            {
                MessageBox.Show("Level Completed!");
            }
        }
        private void btnQuit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you really want to quit?", "Quit", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close();
            }
        }
    }
}