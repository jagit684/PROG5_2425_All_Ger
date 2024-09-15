using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDomain
{
    public class Person
    {
        // fields
        private string firstName;
        private string lastName;

        // constructor
        public Person(string first, string? middle, string last, string birthdate, string telnr)
        {
            firstName = first ?? "onbekend";
            lastName = last ?? "onbekend";
            MiddleName = middle;
            BirthDate = DateTime.ParseExact(birthdate, "dd-MM-yyyy", CultureInfo.InvariantCulture); ;
            TelNr = telnr;
        }

        // properties
        public string FirstName
        {
            get { return MyFormatWord(firstName); }
            set { firstName = value ?? "onbekend"; }
        }

        public string LastName
        {
            get { return MyFormatWord(lastName); }
            set { lastName = value ?? "onbekend"; }
        }

        public string? MiddleName { get; set; }
        public string FullName
        {
            get
            {
                return TrimExtraSpaces($"{FirstName} {MiddleName} {LastName}");
            }
        }
        public DateTime BirthDate { get; set; }
        public string TelNr { get; set; }

        // calculated properties
        public int Age
        {
            get
            {
                int age = DateTime.Now.Year - BirthDate.Year;
                if (DateTime.Now.DayOfYear < BirthDate.DayOfYear)
                {
                    age--;
                }
                return age;
            }
        }

        // methods for formatting
        private string TrimExtraSpaces(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            var words = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return string.Join(" ", words);
        }

        private string MyFormatWord(string word)
        {
            if (string.IsNullOrEmpty(word))
                return string.Empty;

            return char.ToUpper(word[0]) + word.Substring(1).ToLower();
        }
    }
}