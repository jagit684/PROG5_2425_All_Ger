using MyDomain;
using wk1_HelloWorld;

namespace wk1_Telefoonboekje
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Person> mylist = new() {
                            new("Jan", null, "Jansen", "15-07-1996", "06-33323312"),
                            new("Piet", null, "Pietersen", "28-05-1997", "06-33367812"),
                            new("Klaas", "de", "Klomp", "10-09-1995", "06-33362372"),
                            new("Henk", "van de", "Meiden", "22-03-1998", "06-33321332"),
                            new("Piet", "van", "Vliet", "03-11-1994", "06-73102932"),
                            new("Koos", null, "Tomeloos", "17-08-1996", "06-73174291"),
                            new("Kees", "de", "Koning", "30-06-1997", "06-73174391"),
                            new("John", null, "Doe", "13-10-1995", "06-12345678"),
                            new("Hendrik", null, "Hendriks", "25-04-1998", "06-98765432"),
                            new("Marieke", null, "Janssen", "06-12-1994", "06-87654321"),
                            new("Eva", null, "Bakker", "19-09-1996", "06-76543210"),
                            new("Pieter", null, "Smit", "02-07-1997", "06-65432109"),
                            new("Lotte", null, "Mulder", "15-11-1995", "06-54321098"),
                            new("Daan", null, "Visser", "28-05-1998", "06-43210987"),
                            new("Sophie", "de", "Boer", "10-12-1994", "06-32109876"),
                            new("Ruben", null, "Bos", "23-08-1996", "06-21098765"),
                            new("Emma", null, "Vos", "05-06-1997", "06-10987654"),
                            new("Lucas", "de", "Graaf", "18-10-1995", "06-09876543"),
                            new("Mila", null, "Jacobs", "30-04-1998", "06-98765432"),
                            new("Finn", null, "Huisman", "13-12-1994", "06-87654321")
                        };

            TelephoneBook mybook = new(mylist);
            MyIO myio = new();
            string? startletter = myio.GetString("Welke letter?: ");
            List<Person> resultlist = mybook.GetPeople(startletter);
            myio.PrintTelephoneBook(resultlist, resultlist.Count );

            Console.OutputEncoding = System.Text.Encoding.UTF8;
            char horizontalBar = '\u2015';
            Console.WriteLine($"{new string(horizontalBar, 35)}");

        }
    }
}
