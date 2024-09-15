using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDomain
{
    public class Person
    {
        // constants
        private const int RetirementAge = 67;
        // fields
        private string firstName;
        private string lastName;

        // constructor
        public Person(string first, string? middle, string last, DateTime birthdate)
        {
            firstName = first ?? "onbekend";
            lastName = last ?? "onbekend";
            MiddleName = middle;
            BirthDate = birthdate;
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
        public bool IsRetired
        {
            get
            {
                return Age >= RetirementAge;
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