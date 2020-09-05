using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// Author: Sina Lyon

namespace KnightsTour
{
    /**
     * @interface Chessboard contains methods to be implemented by it's Knight piece.
     */ 
    public interface Chessboard
    {
        void reset(ref Label[,] cells); // clears the chessboard to all 0s
        void update(ref Label[,] cells); // updates the chessboard visually
        void access(ref Label[,] cells); // for accessablity heuristic method
    } // end of Chessboard interface

    /**
     * @class Knight contains the main logic of this program. It has methods for both the non-intelligent 
     *          and intelligent versions of traversing through the chessboard using Knight piece movements.
     *  
     *  Note for partial class: This section focuses on the Chessboard methods of the Knight class.
     */
    public partial class Knight : ChessPiece, Chessboard
    {
        /**
         * @method update simply updates the current cell of the Knight with it's
         *              next turn number. It is called only after position is checked.
         */
        public void update(ref Label[,] cells)
        {
            Occupied++;
            cells[Row, Column].Text = "" + Occupied;
            cells[Row, Column].BackColor = System.Drawing.Color.Azure;
            cells[Row, Column].Font = new Font(cells[Row, Column].Font, FontStyle.Bold);
        } // end of update()

        /**
         * @method reset simply sets all cell labels text value to 0.
         */
        public void reset(ref Label[,] cells)
        {
            int row = 8;
            do
            {
                foreach (var column in cells)
                {
                    column.Text = "0";
                    column.BackColor = Color.Empty;
                    column.Font = new Font(column.Font, FontStyle.Regular); // remove bold font
                }
                --row;
            } while (row > 0);
        } // end of reset() method

        /**
         * @author Sina Lyon
         * @method access() sets up the accessability heuristic within the chessboard cells. This means
         *              that the Knight is more likely to go for a higher access number like 8.
         */
        public void access(ref Label[,] cells)
        {
            byte row = 0, col; // starter row & column variables to traverse through cells[,]
            do // traversing through row
            {
                // Going through the 2D array with this nested loop
                for (col = 0; col < 8; col++) // traversing through column
                {
                    // First setting up all four corners within this if-else chain
                    if ((row == 7 && col == 7) || (row == 7 && col == 0) || (row == 0 && col == 7) || (row == 0 && col == 0))
                        cells[row, col].Text = "2"; // Get the 4 corners with access level 2
                    else if ((row == 0 && col == 1) || (row == 1 && col == 0) || // Top-Left  
                        (row == 0 && col == 6) || (row == 1 && col == 7) ||   // Top-Right
                        (row == 6 && col == 0) || (row == 7 && col == 1) || // Bottom-Left 
                        (row == 6 && col == 7) || (row == 7 && col == 6))   // Bottom-Right
                        cells[row, col].Text = "3"; // Get the 8 2nd-last corners with access level 3
                    else if ((row == 0 || row == 7 || row > 1 && row < 6) &&
                            (col == 0 || col == 7 || col > 1 && col < 6) ||
                            (row == 6 && col == 6) || (row == 6 && col == 1) || (row == 1 && col == 6) || (row == 1 && col == 1))
                        cells[row, col].Text = "4"; // Get the end sides and 3rd-last corners with access level 4
                    else if ((row >= 1 && row <= 6) && (col > 1 && col < 6) || (col == 1 || col == 6))
                        cells[row, col].Text = "6"; // Get the 2nd-last 16 end sides with access level 6

                    // Finally, fill out all the middle cells with the highest access
                    if ((row > 1 && row < 6) && (col > 1 && col < 6))
                        cells[row, col].Text = "8"; // Get the middle cells with access level 8
                } // end of for loop for columns
                row++;
            } while (row < 8);
        } // end of access()

    } // end of Knight class
}
