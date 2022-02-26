using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace FnsService
{
    public class InnFromFnsService
    {
        public async Task<string> GetInn(Person person)
        {
            if (string.IsNullOrWhiteSpace(person.FirstName))
            {
                throw new ArgumentNullException(nameof(person.FirstName), "Поле 'Имя' не может быть пустым или состоять только из пробелов");
            }

            string firstName = HttpUtility.UrlEncode(person.FirstName);

            if (string.IsNullOrWhiteSpace(person.Patronymic))
            {
                person.Patronymic = "1";
            }

            string patronymic = HttpUtility.UrlEncode(person.Patronymic);

            if (string.IsNullOrWhiteSpace(person.LastName))
            {
                throw new ArgumentNullException(nameof(person.LastName), "Поле 'Фамилия' не может быть пустым или состоять только из пробелов");
            }

            string lastName = HttpUtility.UrlEncode(person.LastName);

            if (person.DateOfBirth < DateTime.Parse("01.01.1900") || person.DateOfBirth >= DateTime.Now)
            {
                throw new ArgumentException("Некорректная дата рождения", nameof(person.DateOfBirth));
            }

            string dateOfBirth = person.DateOfBirth.ToString("dd.MM.yyyy");

            if (string.IsNullOrWhiteSpace(person.NumberOfPassport))
            {
                throw new ArgumentNullException(nameof(person.NumberOfPassport), "Поле 'Номер паспорта' не может быть пустым или состоять только из пробелов");
            }

            string correctNumbOfPas = person.NumberOfPassport.Replace(" ", "");

            if (correctNumbOfPas.Length != 10)
            {
                throw new ArgumentException("Значение 'Номер паспорта' должно состоять из 10 цифр", nameof(person.NumberOfPassport));
            }

            if (correctNumbOfPas.Any(c => !Char.IsDigit(c)))
            {
                throw new ArgumentException("Значение 'Номер паспорта' должно состоять только из цифр", nameof(person.NumberOfPassport));
            }

            string i = correctNumbOfPas;
            string numberOfPassport = $"{i.Substring(0, 2)}+{i.Substring(2, 2)}+{i.Substring(4, 6)}";

            string data = $"c=find&captcha=&captchaToken=&fam={lastName}&nam={firstName}&otch={patronymic}&bdate={dateOfBirth}&doctype=21&docno={numberOfPassport}&docdt=";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://service.nalog.ru/");

                StringContent queryString = new StringContent(data, Encoding.UTF8, "application/x-www-form-urlencoded");

                HttpResponseMessage response = await client.PostAsync("inn-new-proc.do", queryString);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = response.Content.ReadAsStringAsync().Result;

                    FnsResponseWithId responseWithId = JsonConvert.DeserializeObject<FnsResponseWithId>(responseContent);

                    queryString = new StringContent($"c=get&requestId={responseWithId.RequestId}", Encoding.UTF8, "application/x-www-form-urlencoded");

                    response = await client.PostAsync("inn-new-proc.json", queryString);

                    if (response.IsSuccessStatusCode)
                    {
                        responseContent = response.Content.ReadAsStringAsync().Result;

                        FnsResponseWithInn responseWithInn = JsonConvert.DeserializeObject<FnsResponseWithInn>(responseContent);

                        if (responseWithInn.State == 1.0M)
                        {
                            return responseWithInn.Inn;
                        }
                        else
                        {
                            throw new Exception("Данные не найдены");
                        }
                    }
                    else
                    {
                        throw new Exception($"Запрос № 2 потерпел неудачу! Статус ошибки: {response.StatusCode}");
                    }
                }
                else
                {
                    throw new Exception($"Запрос № 1 потерпел неудачу! Статус ошибки: {response.StatusCode}");
                }
            }
        }
    }

    public class FnsResponseWithId
    {
        public string RequestId { get; set; }

        public bool CaptchaRequired { get; set; }
    }

    public class FnsResponseWithInn
    {
        public string Inn { get; set; }

        public decimal State { get; set; }

        // personFalse {"id":"d340f798-3b78-4fdc-b2ed-29798970de29","state":0.0,"error_code":1.0}
        // person {"id":"180caeee-7df1-45a9-8621-4171d2c65530","state":1.0,"inn":"770474877718","entityId":1.00070278422E11}
}
}