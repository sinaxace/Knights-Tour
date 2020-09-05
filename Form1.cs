using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// Author: Sina Lyon

namespace KnightsTour
{
    public partial class Form1 : Form
    {
        public Label[,] cells; // This program's main data structure will be initialized within Form1 constructor below
        Knight k; // Our knight piece

        public Form1()
        {
            InitializeComponent();

            // Starting at default (0,0)
            k = new Knight(0, 0);

            // clear files on startup
            deleteFiles();

            // Setting up cells in 2D Array of Label objects to update the cells right away during button events
            cells = new Label[8, 8] {
                {this.row0column0, this.row0column1, this.row0column2, this.row0column3, this.row0column4, this.row0column5, this.row0column6, this.row0column7 },
                {this.row1column0, this.row1column1, this.row1column2, this.row1column3, this.row1column4, this.row1column5, this.row1column6, this.row1column7 },
                {this.row2column0, this.row2column1, this.row2column2, this.row2column3, this.row2column4, this.row2column5, this.row2column6, this.row2column7 },
                {this.row3column0, this.row3column1, this.row3column2, this.row3column3, this.row3column4, this.row3column5, this.row3column6, this.row3column7 },
                {this.row4column0, this.row4column1, this.row4column2, this.row4column3, this.row4column4, this.row4column5, this.row4column6, this.row4column7 },
                {this.row5column0, this.row5column1, this.row5column2, this.row5column3, this.row5column4, this.row5column5, this.row5column6, this.row5column7 },
                {this.row6column0, this.row6column1, this.row6column2, this.row6column3, this.row6column4, this.row6column5, this.row6column6, this.row6column7 },
                {this.row7column0, this.row7column1, this.row7column2, this.row7column3, this.row7column4, this.row7column5, this.row7column6, this.row7column7 }
            };
        }

        /**
         * @method outputFile simply appends the trial string  to a file depending on 
         *      the relative url string parameter.
         */ 
        private void outputFile(string trial, string relative)
        {
            var file = File.AppendText(relative);
            file.WriteLine(trial);
            file.Close();
        }

        /**
         * @method getTrialsNumber is going through the process of getting the number of trials to 
         *              run either the non-intelligent or intelligent algorithm.
         */ 
        private int getTrialsNumber()
        {
            int trialsNumber;

            // First, record number of trials the user has chosen
            bool isMultiTrial = Int32.TryParse(DialogInput.promptDialog("How many trials: ", "Choose the number of tours."), out trialsNumber);

            // Double check to see if user entered a number or not
            if (!isMultiTrial)
            {
                MessageBox.Show("Error: Cannot read number of trials input. \nDefaulting to 1 trial", "Trial Choice Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                trialsNumber = 1; // default is one trial
            }

            return trialsNumber;
        }

        /**
         * @method setRowCol creates a dialog prompt to ask the user of the knight's
         *          starting position and returns a new Knight object to that choice.
         */ 
        private Knight setRowCol(int trials)
        {
            // Starting position will be asked for each trial
            var startingPos = DialogInput.promptDialog("Row, Column: ", "Choose your knight's starting position - Trial " + trials);

            // Next, splitting row & column numbers entered only if comma was delimited
            var rowcol = startingPos.Split(',');
            byte row, col;

            // Then we need to test the row and column inputs to see if they're an actual cell to set the knight at.
            if (Byte.TryParse(rowcol[0], out row) && Byte.TryParse(rowcol[1], out col) && row < 8 && col < 8 && row >= 0 && col >= 0)
                return new Knight(row, col);
            else
            {
                MessageBox.Show("Error: Cannot read row,column input. \nDefaulting to cell 0,0", "Row/Column Choice Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new Knight(0, 0);
            }

        } // end of setRowCol()

        /**
         * @method nonIntelButton_Click is an event handler for the non-intelligent button which triggers
         *           the Knight object's random traversing through the chessboard matrix.   
         */
        private void nonIntelButton_Click(object sender, EventArgs e)
        {
            List<byte> possibilities;
            int trials = getTrialsNumber();
            for (int i = 1; i <= trials; i++)
            {
                k = setRowCol(trials); // getting new Knight position
                k.reset(ref cells); // save the last trial for display but resets the preceding trials
                k.update(ref cells);

                // then starting the touring...
                do
                {
                    var rand = new Random();
                    possibilities = k.bound(false, ref cells); // contains all possible positioning directions

                    if (possibilities.Count != 0)
                    {
                        k.move(possibilities[rand.Next(possibilities.Count)]); // Updates Row & Column perminently
                        k.update(ref cells);
                    }

                } while (possibilities.Count != 0); // if list is empty, then there's no more directions to go

                // Finally, record trial result to SinaLyonNonIntelligentMethod.txt
                outputFile("Trial " + i + ": The knight was successfully able to touch " + k.Occupied + " squares.", "SinaLyonNonIntelligentMethod.txt");

            } // end of trials for loop
        } // end of nonIntelButton_Click() method

        /**
         * @method heuristicButton_Click focuses on the intelligent method of traversing through the chessboard
         *          by calling the Knight object's accessability heuristic algorithm after setting up the chessboard
         *          access levels.
         */
        private void heuristicButton_Click(object sender, EventArgs e)
        {
            int trials = getTrialsNumber();
            for (int i = 1; i <= trials; i++)
            {
                k = setRowCol(trials); // getting new Knight position
                k.reset(ref cells); // save the last trial for display but resets the preceding trials
                k.access(ref cells); // setup access levels for accessability heuristic
                k.update(ref cells);

                int direction; // to hold the final direction of the knight
                do
                {
                    var pointers = k.bound(true, ref cells);
                    direction = pointers.Count != 0 ? k.heuristic(pointers, ref cells) : -1; // contains all possible positioning directions, -1 if pointers are empty

                    if (direction >= 0)
                    {
                        k.move((byte)direction); // Updates Row & Column perminently
                        k.update(ref cells);
                    }

                } while (direction >= 0); // if negative, then there's no more directions to move to
            } // end of trials for loop
        } // end of heuristic button event

        /**
         * @method clearbtn_Click is an event handler that resets the entire chessboard, resets the Knight
         *              and deletes any trial files through deleteFiles() method.
         */
        private void clearbtn_Click(object sender, EventArgs e)
        {
            k.reset(ref cells); // reset chessboard
            k = new Knight(0, 0); // reset Knight object

            // clear existing files
            deleteFiles();
        }

        /**
         * @method deleteFiles makes sure that no existing files from past trials are around to 
         *              get a new file of trials.
         */
        private void deleteFiles()
        {
            // clear files
            if (File.Exists("SinaLyonNonIntelligentMethod.txt"))
                File.Delete("SinaLyonNonIntelligentMethod.txt");

            if (File.Exists("SinaLyonHeuristicsMethod.txt"))
                File.Delete("SinaLyonHeuristicsMethod.txt");
        }
    }
}
