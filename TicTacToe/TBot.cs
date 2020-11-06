using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace TicTacToe
{
    /// <summary>
    /// Class to hold Tic Tac Toe bot logic. Based on Tic Tac Toe AI from Gred Surma - https://towardsdatascience.com/tic-tac-toe-creating-unbeatable-ai-with-minimax-algorithm-8af9e52c1e7d
    /// </summary>
    class TBot
    {
        private int moveHere;
        /// <summary>
        /// Helper function to find the winner of a current board state
        /// </summary>
        /// <param name="winnerBoard">The current board state to check winner against</param>
        /// <returns>-1 if human won, 0 if tie, 1 if computer wins</returns>
        private int FindWinner(MarkType[] winnerBoard)
        {
            //1 if computer, 0 if tie, -1 if human
            int winner = 0;

            //Find winner in board
            for (int i = 0; i < 3; i++)
            {
                int rowIndex = i * 3;
                //Horizontal winner
                if ((winnerBoard[rowIndex] != MarkType.Free) && (winnerBoard[rowIndex] == winnerBoard[rowIndex + 1] && winnerBoard[rowIndex] == winnerBoard[rowIndex + 2]))
                {
                    //If human won, set to -1, else 1
                    winner = winnerBoard[rowIndex] == MarkType.Cross ? -1 : 1;
                }
                //Vertical winner
                if ((winnerBoard[i] != MarkType.Free) && ((winnerBoard[i] == winnerBoard[i + 3]) && (winnerBoard[i] == winnerBoard[i + 6])))
                {
                    winner = winnerBoard[rowIndex] == MarkType.Cross ? -1 : 1;
                }
                //Diagonal L-R winner
                if (i == 0)
                {
                    if ((winnerBoard[0] != MarkType.Free) && ((winnerBoard[0] == winnerBoard[4]) && (winnerBoard[0] == winnerBoard[8])))
                    {
                        winner = winnerBoard[rowIndex] == MarkType.Cross ? -1 : 1;
                    }
                }
                //Diagonal R-L winner
                if (i == 2)
                {
                    if ((winnerBoard[2] != MarkType.Free) && ((winnerBoard[2] == winnerBoard[4]) && (winnerBoard[2] == winnerBoard[6])))
                    {
                        winner = winnerBoard[rowIndex] == MarkType.Cross ? -1 : 1;
                    }
                }
            }
            return winner;
        }

        /// <summary>
        /// Utility function to manage creating copy of the original boards
        /// </summary>
        /// <param name="copyArray">Clone array to copy into</param>
        /// <param name="originalArray">Original array to copy from</param>
        private void MakeCopy(MarkType[] copyArray, MarkType[] originalArray)
        {
            for(int i = 0; i < originalArray.Length; i++)
            {
                copyArray[i] = originalArray[i];
            }
        }

        /// <summary>
        /// Algorithm that decides best move using via minimax
        /// </summary>
        /// <param name="board">Board state</param>
        /// <param name="currentPlayer"> tracks the current player, -1 human, 1 if computer</param>
        /// <returns>The score associated with the current move</returns>
        private int DecideMove(MarkType[] decisionBoard, int currentPlayer)
        {
            //Check if there are any winners
            int won = FindWinner(decisionBoard);
            if (won != 0)
            {
                return won;
            }

            int score = -2; //Tracks the score
            int moveIndex = -1; //Tracks which index will be the move

            for(int i = 0; i < 9; i++)
            {
                //Only move on free spaces
                if(decisionBoard[i] == MarkType.Free)
                {
                    MarkType[] newMoveBoard = new MarkType[decisionBoard.Length];
                    MakeCopy(newMoveBoard, decisionBoard); // Duplicate board for testing different moves
                    newMoveBoard[i] = currentPlayer == 1 ? MarkType.Nought : MarkType.Cross; // Fill board with new move dependent on whose turn it is
                    int moveScore = -DecideMove(newMoveBoard, -1 * currentPlayer); // Check value of making the move
                    if(moveScore > score)
                    {
                        score = moveScore;
                        moveIndex = i;
                    }
                }
            }
            //If tie, return 0
            if(moveIndex == -1)
            {
                return 0;
            }

            moveHere = moveIndex;
            return score;
        }

        /// <summary>
        /// Public function to handle bots move
        /// </summary>
        /// <param name="board">Current board state</param>
        public int Move(MarkType[] board)
        {
            DecideMove(board, 1);
            return moveHere;
        }
    }
}
