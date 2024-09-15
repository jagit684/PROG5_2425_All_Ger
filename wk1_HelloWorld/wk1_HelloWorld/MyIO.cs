using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wk1_HelloWorld
{
    /*
     * This class is a simple input/output class that can be used to get input from the user.
     */
    internal class MyIO
    {
        public string? GetString(string prompt)
        {
            string? input;
            Console.Write("> " + prompt);
            input = Console.ReadLine();
            return input;
        }

        public string GetNotEmptyString(string prompt)
        {
            string? input = null;
            while (String.IsNullOrWhiteSpace(input))
            {
                Console.Write("> " + prompt);
                input = Console.ReadLine();
                if (String.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("> incorrect");
                }
                else
                {
                    break;
                }
            }
            return input;
        }

        public DateTime GetNotEmptyDate(string prompt)
        {
            string input = GetNotEmptyString(prompt);
            while (!DateTime.TryParseExact(input, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                Console.WriteLine("> incorrect");
                input = GetNotEmptyString(prompt);
            }
            return DateTime.ParseExact(input, "dd-MM-yyyy", CultureInfo.InvariantCulture);
        }

    }
}

