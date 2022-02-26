using System;
using System.Threading.Tasks;

namespace FnsService
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Person person = new Person("Владислав", "Валерьевич", "Захаров",
                new DateTime(1991, 11, 22), "4511505609");

            Person personWithoutPatron = new Person("Владислав", null, "Захаров",
                new DateTime(1991, 11, 22), "4511505609");

            Person personFalse = new Person("Иван", "Иванович", "Иванов",
                new DateTime(1986, 05, 16), "1234569874");

            InnFromFnsService innFromFnsService = new InnFromFnsService();

            string result = await innFromFnsService.GetInn(personFalse);

            Console.WriteLine(result);
        }
    }
}
