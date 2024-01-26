using System;
using myClasses;

namespace escobacli
{
    public class Program
    {
        public static void Main(string[] args)
        {


            Print("Welcome to Escoba CLI!");

            //Add a menu
            // 1. Start a new game
            // 2. Learn how to play
            // 3. Exit
            
            do
            {
                // NOTE: still deciding if i should put them outside the loop and keep the same names for all the games or change them each game
                // ask for player names
                Print("Enter player 1 name: ");
                Player p1 = new(inputName());

                Print("Enter player 2 name: ");
                Player p2 = new(inputName());

                // make new game w given names
                var game = new Game(p1, p2);
                game.Start();
            } while (true); // TODO: add an exit condition
        }

        // //////////////////////////////////////////////////////////
        //                                                          /
        //                      METHODS                             /
        //                                                          /
        // //////////////////////////////////////////////////////////

        // self describing
        public static string inputName()
        {

            string inputName = ReadLineNonNull();
            return inputName;
        }

        // i hate writing Console.WriteLine
        public static void Print(string text)
        {
            Console.WriteLine(text);
        }

        // self describing
        public static void PrintSeparator()
        {
            Console.WriteLine("--------------------------------------------------");
        }

        // easy ready method for input handling. else i'd have to check for null strings and empty strings everytime
        public static string ReadLineNonNull()
        {
            string? input;
            while (true)
            {
                input = Console.ReadLine();

                // is not null
                if (input != null)
                {
                    // format
                    input.Trim().ToUpper();

                    // is not empty nor is too long
                    if (input != "" && input.Length < 20)
                    {
                        return input;
                    }

                    Program.Print("The name is empty or too long, try again.");
                }
                else
                {
                    Program.Print("o.o");
                }
            }
        }

        // Uses ReadLineNonNull() to get a string, parses it as an int and returns it if its a single digit number
        public static int ReadDigit()
        {
            int input;
            while (true)
            {
                try
                {
                    input = int.Parse(Program.ReadLineNonNull());

                    if (input < 10 && input > -1)
                        return input;
                    else
                        throw new System.Exception();
                    
                }
                catch (System.Exception)
                {
                    Program.Print("Not a digit, try again.");
                }
            }
        }
    }
}