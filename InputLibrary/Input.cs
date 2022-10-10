using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InputLibrary
{
    public static class Input
    {
        public static bool getYesInput(string message)
        {
            do
            {
                Console.Write("\n" + message + ": ");
                char input = Console.ReadLine()[0];
                if (Char.ToLower(input) ==  'y')
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("Error: Please enter 'Y' to take a number");
                }
            } while (true);
        }

        public static int getIntegerInput(string message, int min, int max)
        {
            bool validInput = false;
            int num = 0;

            do
            {
                Console.Write("\n" + message + ": ");
                try
                {
                    var input = Console.ReadLine();
                    num = Int32.Parse(input);

                    if (num >= min && num <= max)
                    {
                        validInput = true;
                    }
                    else
                    {
                        Console.WriteLine(
                            String.Format("Error: Number must be between {0} and {1}", min, max));
                    }

                }
                catch (FormatException e)
                {
                    Console.WriteLine("Error: " + e.Message);
                }
            } while (!validInput);

            return num;

        }
    }
}
