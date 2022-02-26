using System;
using System.Linq;

namespace FnsService
{
    public class Person
    {
        public int PersonId { get; set; }

        public string FirstName { get; set; }

        public string Patronymic { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string NumberOfPassport { get; set; }

        public Person(string firstName, string patronymic, string lastName, DateTime dateOfBirth, string numberOfPassport)
        {
            FirstName = firstName;
            Patronymic = patronymic;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            NumberOfPassport = numberOfPassport;
        }
    }
}