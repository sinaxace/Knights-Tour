using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// Author: Sina Lyon

namespace KnightsTour
{
    /**
     * @abstract ChessPiece contains the bare-bones of what the Knight class will need
     *              from it's base class. It's meant to be completely inherited by Knight.
     */ 
    public abstract class ChessPiece
    {
        public byte Row { get; set; } // records current row position
        public byte Column { get; set; } // records current column position
        public int Occupied { get; set; } // keeping track of number of spots occupied 

        public ChessPiece(byte row, byte column)
        {
            this.Row = row;
            this.Column = column;
        }
        public abstract void move(byte pointer);
        public abstract List<byte> bound(bool isAccess, ref Label[,] cells);
        public abstract int heuristic(List<byte> directions, ref Label[,] cells);
        public abstract bool detection(bool isAccess, ref Label[,] cells);
    } // end of ChessPiece abstract

    /**
     * @class Knight contains the main logic of this program. It has methods for both the non-intelligent 
     *          and intelligent versions of traversing through the chessboard using Knight piece movements.
     *  
     *  Note for partial class: This section focuses on the chesspiece part of the knight class.
     */
    public partial class Knight : ChessPiece, Chessboard
    {

        public Knight(byte row, byte column) : base(row, column) { } // initialized from the ChessPiece base class

        /**
         * @method detection returns conditions for occupied spots based on whether the chessboard
         *          was setup as an accessability matrix or just with all 0s.
         */
        public override bool detection(bool isAccess, ref Label[,] cells)
        {
            if (!isAccess)
                return cells[Row, Column].Text != "0"; // for Non-Intelligent detection
            else
                return cells[Row, Column].Font.Style == System.Drawing.FontStyle.Bold; // for Intelligent detection
        }

        /**
         * @method heuristic checks for the lease accessable cell within the list of direction numbers
         *      and returns a final integer direction to move the knight to.
         * @param directions contains a list of possible directions returned from the bound() method as an argument.
         * @param cells is referenced for manipulation of cell label objects
         */
        public override int heuristic(List<byte> directions, ref Label[,] cells)
        {

            if (directions.Count > 0) // skip code section if it's only one direction
            {

                byte[] accesses = new byte[directions.Count]; // a parellel array that will hold accessability numbers to compare
                byte row = Row, col = Column; // recording previous location

                // filling up access
                for (var i = 0; i < directions.Count; i++)
                {
                    move(directions[i]); // Temporarily move to a direction

                    accesses[i] = Byte.Parse(cells[Row, Column].Text); // Recording accessability number, parellel to directions's ordering

                    // Reset to undo move
                    Row = row;
                    Column = col;
                }

                // Finally, sort out the accesses array in parellel to directions array
                byte num; // temp variable for swap 
                for (var i = 0; i < accesses.Length - 1; i++)
                    if (accesses[i] > accesses[i + 1])
                    {
                        // store current index
                        num = accesses[i];

                        // then swap to sort
                        accesses[i] = accesses[i + 1];
                        accesses[i + 1] = num;

                        // now do the same for the directions list
                        num = directions[i];
                        directions[i] = directions[i + 1];
                        directions[i + 1] = num;
                    }
            }

            // Now just choose the first element of directions to get lowest accessability
            return directions[0];
        } // end of heuristic()

        /**
         * @method move() updates the Knight object's current Row & Column properties,
         *          and reverts back to it's old position after checking to see if
         * 
         * @param pointer contains the direction that the Knight object moves at.
         */
        public override void move(byte pointer)
        {
            switch (pointer)
            {
                case 0:
                    Row -= 1; // moving up 1
                    Column += 2; // moving right 2
                    break;
                case 1:
                    Row -= 2; // moving up 2
                    Column += 1; // moving right 1
                    break;
                case 2:
                    Row -= 2; // moving up 2
                    Column -= 1; // moving left 1
                    break;
                case 3:
                    Row -= 1; // moving up 1
                    Column -= 2; // moving left 2
                    break;
                case 4:
                    Row += 1; // moving down 1
                    Column -= 2; // moving left 2
                    break;
                case 5:
                    Row += 2; // moving down 2
                    Column -= 1; // moving left 1
                    break;
                case 6:
                    Row += 2; // moving down 2                        
                    Column += 1; // moving right 1
                    break;
                case 7:
                    Row += 1; // moving down 1
                    Column += 2; // moving right 2
                    break;
            } // end of switch
        } // end of move()


        /**
         * @author Sina Lyon
         * @method bound checks boundaries of the Knight's movement to the next cell.
         *          These checks include the chessboard edges & previously occupied
         *          cells.
         * @param isAccess when true, means that bound() is being used within the heuristic button event. 
         *          When false, bound() is being called within the non-intelligent button event. 
         * @param cells contains a reference of the Labels 2D array containing all cell labels to manipulate.
         * 
         * @return is sending a byte list of possible directions that the knight could move to.
         */
        public override List<byte> bound(bool isAccess, ref Label[,] cells)
        {
            List<byte> directions = new List<byte>() { 0, 1, 2, 3, 4, 5, 6, 7 };  // by default, it's initialized with all number directions

            

            // Top Corners
            if (Row == 0)
            {
                // Top-Left Corner
                if (Column == 0)
                    directions = new List<byte>() { 6, 7 };

                // Top-Left 2nd Last Corner
                if (Column == 1)
                    directions = new List<byte>() { 5, 6, 7 };

                // Top-Right 2nd Last Corner
                if (Column == 6)
                    directions = new List<byte>() { 4, 5, 6 };

                // Top-Right Corner
                if (Column == 7)
                    directions = new List<byte>() { 4, 5 };
            }

            // Bottom Corners
            if (Row == 7)
            {
                // Bottom-Left Corner
                if (Column == 0)
                    directions = new List<byte>() { 0, 1 };

                // Bottom-Left 2nd Last Corner
                if (Column == 1)
                    directions = new List<byte>() { 0, 1, 2 };

                // Bottom-Right 2nd Last Corner
                if (Column == 6)
                    directions = new List<byte>() { 1, 2, 3 };

                // Bottom-Right Corner
                if (Column == 7)
                    directions = new List<byte>() { 2, 3 };

            }

            // Top 2nd Last
            if (Row == 1)
            {
                // Top-Left Last
                if (Column == 0)
                    directions = new List<byte>() { 0, 6, 7 };

                // Top-Left 2nd Last 
                if (Column == 1)
                    directions = new List<byte>() { 0, 5, 6, 7 };

                // Top-Right 2nd Last 
                if (Column == 6)
                    directions = new List<byte>() { 3, 4, 5, 6 };

                // Top-Right Last
                if (Column == 7)
                    directions = new List<byte>() { 3, 4, 5 };
            }

            // Bottom 2nd Last
            if (Row == 6)
            {
                // Bottom-Left Last
                if (Column == 0)
                    directions = new List<byte>() { 0, 1, 7 };

                // Bottom-Left 2nd Last 
                if (Column == 1)
                    directions = new List<byte>() { 0, 1, 2, 7 };

                // Bottom-Right 2nd Last 
                if (Column == 6)
                    directions = new List<byte>() { 1, 2, 3, 4 };

                // Bottom-Right Last
                if (Column == 7)
                    directions = new List<byte>() { 2, 3, 4 };

            }

            // Middle Top & Bottom Sides
            if (Column > 1 && Column < 6)
            {
                // Top Side
                if (Row == 0)
                    directions = new List<byte>() { 4, 5, 6, 7 };

                // Top Side 2nd Last
                if (Row == 1)
                    directions = new List<byte>() { 0, 3, 4, 5, 6, 7 };

                // Bottom Side 2nd Last
                if (Row == 6)
                    directions = new List<byte>() { 0, 1, 2, 3, 4, 7 };

                // Bottom Side
                if (Row == 7)
                    directions = new List<byte>() { 0, 1, 2, 3 };
            }
            // Middle Left & Right Sides
            if (Row > 1 && Row < 6)
            {
                // Left Side
                if (Column == 0)
                    directions = new List<byte>() { 0, 1, 6, 7 };

                // Left Side 2nd Last
                if (Column == 1)
                    directions = new List<byte>() { 0, 1, 2, 5, 6, 7 };

                // Right Side 2nd Last
                if (Column == 6)
                    directions = new List<byte>() { 1, 2, 3, 4, 5, 6 };

                // Right Side
                if (Column == 7)
                    directions = new List<byte>() { 2, 3, 4, 5 };
            }

            // After checking all wall boundaries, next we need to test for already occupied cells below
            byte row = Row, column = Column; // storing original Row & Column positions in case we need to reset it

            // In this reverse loop, we're testing our directions list items to see if that future cell's already been occupied
            for (var index = directions.Count - 1; index >= 0; index--)
            {
                move(directions[index]); // Temporarily move to a direction
                if (detection(isAccess, ref cells)) // check for occupied cell through detection() method
                    directions.RemoveAt(index); // if already occupied the cell, remove that direction

                // Finally, restoring Row & Column back to original position after testing movement boundaries
                Row = row;
                Column = column;
            }

            return directions; // returning all possible movements as a byte list
        } // end of bound()
    } // end of partial Knight class (overriding chesspiece methods)

} // end of namespaces
