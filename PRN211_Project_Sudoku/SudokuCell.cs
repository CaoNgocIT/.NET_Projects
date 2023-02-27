using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN211_Project_Sudoku
{
    internal class SudokuCell : Button
    {
        public int Value { get; set; }
        public bool IsFixed { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public void Clear()
        {
            this.Text = string.Empty;
            this.IsFixed = false;
        }
    }
}
