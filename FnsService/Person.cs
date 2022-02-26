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
            if (string.IsNullOrWhiteSpace(firstName))
            {
                throw new ArgumentNullException(nameof(firstName),"Поле 'Имя' не может быть пустым или состоять только из пробелов");
            }
            
            FirstName = firstName;

            if (string.IsNullOrWhiteSpace(patronymic))
            {
                throw new ArgumentNullException(nameof(patronymic), "Поле 'Отчество' не может быть пустым или состоять только из пробелов");
            }

            Patronymic = patronymic;

            if (string.IsNullOrWhiteSpace(lastName))
            {
                throw new ArgumentNullException(nameof(lastName), "Поле 'Фамилия' не может быть пустым или состоять только из пробелов");
            }
            
            LastName = lastName;

            if (dateOfBirth < DateTime.Parse("01.01.1900") || dateOfBirth >= DateTime.Now)
            {
                throw new ArgumentException("Некорректная дата рождения", nameof(dateOfBirth));
            }

            DateOfBirth = dateOfBirth;

            if (string.IsNullOrWhiteSpace(numberOfPassport))
            {
                throw new ArgumentNullException(nameof(numberOfPassport), "Поле 'Номер паспорта' не может быть пустым или состоять только из пробелов");
            }

            string correctNumbOfPas = numberOfPassport.Replace(" ", "");

            if (correctNumbOfPas.Length != 10)
            {
                throw new ArgumentException("Значение 'Номер паспорта' должно состоять из 10 цифр", nameof(numberOfPassport));
            }

            if (correctNumbOfPas.Any(c => !Char.IsDigit(c)))
            {
                throw new ArgumentException("Значение 'Номер паспорта' должно состоять только из цифр", nameof(numberOfPassport));
            }

            NumberOfPassport = correctNumbOfPas;
        }
    }
}