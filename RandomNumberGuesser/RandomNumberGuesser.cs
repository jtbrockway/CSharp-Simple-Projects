/*
  C# program to generate a random number and recieve user inputted guessing which number
  Based on youtube video by Frencesco Magliocco, "Creating A Number Guessing Game In C# With Visual Studio 2017", youtube link: https://www.youtube.com/watch?v=IhqdPDPV_g8
*/

using System;

class RandomNumberGuesser {

  ///<summary> Function to automatically guess the random number using a simple binary search method </summary>
  ///<param name=randomNumber> The random number to guess </param>
  ///<returns> Returns when correctly guesses number </returns> 
  static void AutoGuesser(int randomNumber) {
    bool correct = false;
    //High,guess,low set up for binary search
    int high = 100;
    int guess = 50;
    int low = 0;
    int placeholder;

    while(!correct) {
      placeholder = guess;
      //If high, next guess is between your low and previous guess
      if (guess > randomNumber) {
        guess = guess - ((guess - low) / 2);
        high = placeholder;
        continue;
      //If low, next guess is between your high and previous guess
      } else if (guess < randomNumber) {
        guess = guess + ((high - guess) / 2);
        low = placeholder;
        continue;
      }
      Console.WriteLine("The number was " + guess.ToString());
      correct = true;
    }
    return;
  }

  //Program starting point
  static void Main() {
    //Random object used to generate random numbers
    Random generator = new Random();
    int randomNumber = generator.Next(0,100);
    
    //User decides if they want to autoguess or not
    Console.WriteLine("Would you like to auto guess? Input y for yes, or n for no");
    bool inputtedCorrectly = false;
    //Verify user provided a valid input
    do {
      string userAutoGuess = Console.ReadLine();
      if(userAutoGuess == "y") {
        AutoGuesser(randomNumber);
        return;
      } else if (userAutoGuess == "n") {
        inputtedCorrectly = true;
      } else {
        Console.WriteLine("Your input was invalid, please only input y or n");
      }
    } while(!inputtedCorrectly);

    //Predeclare int parsed user input (101 is not a value that can exit the program)
    int inputtedNumber = 101;
    //Read user input, provide feedback, loop until user guesses correctly
    do {
      Console.WriteLine("Enter a number");
      string userInput = Console.ReadLine();

      //Try to convert user inputted string to an int
      //Throws error if unable to parse int, continues the loop
      try {
        inputtedNumber = Int32.Parse(userInput);
      } catch (FormatException e) {
        Console.WriteLine(e.Message + " Please Try Again");
        continue;
      }

      //Provide feedback of high or low
      if(inputtedNumber > randomNumber) {
        Console.WriteLine("You are to high");
      } else if(inputtedNumber < randomNumber) {
        Console.WriteLine("You are to low");
      }
    } while (inputtedNumber != randomNumber);

    Console.WriteLine("You guessed correctly!");
    //Wait for user input to exit program
    Console.ReadLine();
  }
}