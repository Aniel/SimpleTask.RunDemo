using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTask.RunDemo
{
    /// <summary>
    /// A simple Task.Run() console demo
    /// </summary>
    class Program
    {
        static bool IsRunning = false;

        /// <summary>
        /// The entry point of the console program
        /// </summary>
        static void Main()
        {
            var exit = false;
            while (!exit)
            {
                if (!IsRunning)
                    Console.WriteLine("Gebe ein Nummer groesser 0 ein\nZum Beenden schreibe \"exit\":");
                var userInput = Console.ReadLine();
                var number = 0;
                var defaultNumber = 10000000;
                if (userInput.ToLower().Equals("exit"))
                {
                    exit = true;
                }
                else
                {
                    if (!IsRunning)
                    {
                        if (!int.TryParse(userInput, out number))
                        {
                            number = defaultNumber;
                            ConsoleWriteLineColor($"Eingabe Fehlerhaft: es wird die default Numemr verwendet genommen", ConsoleColor.Red);
                        }
                        AsyncRunner(number);
                        ConsoleWriteLineColor($"Berechne {number} :) Chatte doch etwas mit mir, solange ich im Hintergrund rechne: ", ConsoleColor.Green);
                    }
                    else
                    {
                        ConsoleWriteColor("Du hast \"", ConsoleColor.DarkYellow);
                        ConsoleWriteColor(userInput, ConsoleColor.DarkRed);
                        ConsoleWriteColor("\" geschrieben!\nWillst du noch etwas sagen?\n", ConsoleColor.DarkYellow);
                    }
                }
            }

            Console.WriteLine("Zum beenden drücke eine beliebige Taste");
            Console.ReadKey();
        }
        /// <summary>
        /// Async Wrapper Methode because the Main methode cannot be async
        /// </summary>
        /// <param name="number">The max number to be checked as prime</param>
        static async void AsyncRunner(int number)
        {
            //Start the Task to Calculate the prime numbers. This will spawn a new thread
            var task = Task.Run(() => Calculate(number));
            //Signal flag that the calculation is running
            IsRunning = true;
            //Execute code while the task is running
            ConsoleWriteLineColor($"Calculator here: I started calculating", ConsoleColor.Red);
            //Wait for the task completion
            var primes = await task;
            //Signal flag that the task has finished
            IsRunning = false;
            ConsoleWriteLineColor($"Ich habe {primes.Count} zahlen gefunden", ConsoleColor.Red);
        }

        /// <summary>
        /// Calculates the primes
        /// </summary>
        /// <param name="number">The max number to be checked as prime</param>
        /// <returns>The List of all found primes</returns>
        static List<int> Calculate(int number)
        {
            var primes = new List<int>();
            for (int i = 0; i <= number; i++)
            {
                if (IsPrime(i))
                {
                    primes.Add(i);
                }
            }
            return primes;
        }

        /// <summary>
        /// Checks if teh givin number is a prime
        /// </summary>
        /// <param name="number">The number to check</param>
        /// <returns>true if the given number is a prime</returns>
        static bool IsPrime(int number)
        {
            if ((number & 1) == 0)
            {
                if (number == 2)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            for (int i = 3; (i * i) <= number; i += 2)
            {
                if ((number % i) == 0)
                {
                    return false;
                }
            }
            return number != 1;
        }

        static void ConsoleWriteLineColor(string text, ConsoleColor color)
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = oldColor;
        }

        static void ConsoleWriteColor(string text, ConsoleColor color)
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ForegroundColor = oldColor;
        }
    }
}
