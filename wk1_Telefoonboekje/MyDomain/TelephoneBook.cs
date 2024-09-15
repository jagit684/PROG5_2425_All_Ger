using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDomain
{
    public class TelephoneBook(List<Person> inlist)
    {

        private List<Person> Book = inlist;
        public int GetNumberOfPersons => Book.Count();

        public List<Person> GetPeople(string? startletter)
        {
            if (String.IsNullOrWhiteSpace(startletter))
            {
                return Book
                    .OrderBy(p => p.FullName)
                    .ToList();
            }
            else
            {
                return Book
                    .Where(p => p.FirstName.StartsWith(startletter.ToUpper()))
                    .OrderBy(p => p.FullName)
                    .ToList();
            }
        }
    }
}
