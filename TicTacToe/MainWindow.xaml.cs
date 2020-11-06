using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

//Based on youtube tutorial by AngelSix, "C# Programming Tutorials: Beginners 09 Creating Game Tic Tac Toe with WPF", youtube link: https://www.youtube.com/watch?v=mnTyiUAHuVk

namespace TicTacToe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        #region Private Members
        /// <summary>
        /// Reference to the Tic Tac Toe Bot
        /// </summary>
        private TBot bot;
        /// <summary>
        /// Holds current results of cells in active game
        /// </summary>
        private MarkType[] currVals;
        /// <summary>
        /// True if player 1(X) turn, or player 2(O) turn
        /// </summary>
        private bool player1Turn;
        /// <summary>
        /// True if player has toggled bot as oponent
        /// </summary>
        private bool botAlive;
        /// <summary>
        /// True if game is done, false if not
        /// </summary>
        private bool gameEnded;
        #endregion
        #region Constructor
        /// <summary>
        /// Default Constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            NewGame();
        }
        #endregion

        /// <summary>
        /// Handles resetting the field to default when a new game happens
        /// </summary>
        private void NewGame()
        {
            //Instantiate the bot
            bot = new TBot();

            //Create new array of free cells
            currVals = new MarkType[9];
            
            //Explicitly set all cells to Free
            for(int i = 0; i < 9; i++){
                currVals[i] = MarkType.Free;
            }

            //Set to player 1's turn
            player1Turn = true;

            //Reset field to default
            BoardContainer.Children.Cast<Button>().ToList().ForEach(btn => {
                btn.Content = string.Empty;
                btn.Background = Brushes.White;
                btn.Foreground = Brushes.Blue;
            });

            gameEnded = false;
        }

        /// <summary>
        /// Handles resetting the game 
        /// </summary>
        /// <param name="sender">Button that was clicked</param>
        /// <param name="e">The click events</param>
        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            NewGame();
        }

        /// <summary>
        /// Handles changing the content of the buttons when they are clicked
        /// </summary>
        /// <param name="sender">Button that was clicked</param>
        /// <param name="e">The click events</param>
        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            //If the game is completed, reset the game when a button is clicked
            if(gameEnded)
            {
                NewGame();
                return;
            }

            Button button = (Button)sender;

            //Identify which button was clicked
            int col = Grid.GetColumn(button);
            int row = Grid.GetRow(button);
            int index = col + (row * 3);
            
            if(currVals[index] != MarkType.Free)
            {
                return;
            }

            if(player1Turn)
            {
                currVals[index] = MarkType.Cross;
                button.Content = "X";
                button.Foreground = Brushes.Blue;
                
                //Bot takes turn
                if(botAlive)
                {
                    int moveIndex = bot.Move(currVals); //Get the bots move
                    currVals[moveIndex] = MarkType.Nought; //Update vals list with bots move
                    //Update button with bot's move
                    Button moveButton = BoardContainer.Children.Cast<Button>().ToList()[moveIndex];
                    moveButton.Content = "O";
                    moveButton.Foreground = Brushes.Yellow;

                    player1Turn ^= true;
                }
            }
            else
            {
                currVals[index] = MarkType.Nought;
                button.Content = "O";
                button.Foreground = Brushes.Red;
            }
            //Swap player turns
            player1Turn ^= true;

            //Check for winner
            if(CheckWinner())
            {
                gameEnded = true;
            }
        }

        /// <summary>
        /// Handles toggling of the tic tac toe bot as the oponent
        /// </summary>
        /// <param name="sender">Button that was clicked</param>
        /// <param name="e">The click events</param>
        private void BtnBot_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            if (botAlive) {
                button.Background = Brushes.White;
                button.Content = "Bot: Off";
                botAlive = false;
            } 
            else
            {
                button.Background = Brushes.Gray;
                button.Content = "Bot: On";
                botAlive = true;
            }

        }

        /// <summary>
        /// Helper method to check for a winner each time a button is pressed
        /// </summary>
        /// <returns>True if winner, false if not</returns>
        private bool CheckWinner()
        {
            for (int i = 0; i < 3; i++)
            {
                int rowIndex = i * 3;
                //Horizontal winner
                if ((currVals[rowIndex] != MarkType.Free) && (currVals[rowIndex] == currVals[rowIndex + 1] && currVals[rowIndex] == currVals[rowIndex + 2]))
                {
                    ColorWinners(rowIndex, rowIndex + 1, rowIndex + 2);
                    return true;
                }
                //Vertical winner
                if ((currVals[i] != MarkType.Free) && ((currVals[i] == currVals[i + 3]) && (currVals[i] == currVals[i + 6])))
                {
                    ColorWinners(i, i + 3, i + 6);
                    return true;
                }
                //Diagonal L-R winner
                if (i == 0)
                {
                    if((currVals[0] != MarkType.Free) && ((currVals[0] == currVals[4]) && (currVals[0] == currVals[8])))
                    {
                        ColorWinners(0, 4, 8);
                        return true;
                    }
                }
                //Diagonal R-L winner
                if (i == 2)
                {
                    if ((currVals[2] != MarkType.Free) && ((currVals[2] == currVals[4]) && (currVals[2] == currVals[6])))
                    {
                        ColorWinners(2, 4, 6);
                        return true;
                    }
                }
            }

            //No more cells available
            if (!currVals.Any(btn => btn == MarkType.Free))
            {
                gameEnded = true;
                BoardContainer.Children.Cast<Button>().ToList().ForEach(btn => { btn.Background = Brushes.Orange; });
            }
            return false;
        }

        /// <summary>
        /// Helper method used to color buttons that won
        /// </summary>
        private void ColorWinners(int btn1, int btn2, int btn3)
        {
            int counter = 0;
            BoardContainer.Children.Cast<Button>().ToList().ForEach(btn => {
                if(counter == btn1 || counter == btn2 || counter == btn3)
                {
                    btn.Background = Brushes.Green;
                }
                counter++;
            });
        }
    }
}
