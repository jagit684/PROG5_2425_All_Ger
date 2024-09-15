using MyDomain;
using System.Globalization;

namespace wk1_HelloWorld
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MyIO io = new MyIO();
            Console.WriteLine("Hello, please enter a person.");

            string first = io.GetNotEmptyString("voornaam: ");
            string? middle = io.GetString("tussenvoegsel: ");
            string last = io.GetNotEmptyString("achternaam: ");
            DateTime birthdate = io.GetNotEmptyDate("geboortedatum (dd-mm-yyyy): ");


            Person p = new Person(first, middle, last, birthdate);

            Console.OutputEncoding = System.Text.Encoding.UTF8;
            char horizontalBar = '\u2015';
            Console.WriteLine($"{new string(horizontalBar, 35)}");
            Console.Write("Hallo " + p.FullName + ", ");
            Console.WriteLine("je bent " + p.Age + " jaar en ");
            Console.WriteLine((p.IsRetired ? "" : "niet ") + "met pensioen.");
            Console.WriteLine($"{new string(horizontalBar, 35)}");
        }
    }
}
