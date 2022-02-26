using System;
using System.Threading.Tasks;

namespace FnsService
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Person person = new Person("Сергей", "Владимирович", "Нажесткин",
                new DateTime(1986, 03, 28), "4607091630");

            InnFromFnsService innFromFnsService = new InnFromFnsService();

            string result = await innFromFnsService.GetInn(person);

            Console.WriteLine(result);
        }
    }
}
