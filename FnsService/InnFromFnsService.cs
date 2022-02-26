using System;
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
            string firstName = HttpUtility.UrlEncode(person.FirstName);
            string patronymic = HttpUtility.UrlEncode(person.Patronymic);
            string lastName = HttpUtility.UrlEncode(person.LastName);
            string dateOfBirth = person.DateOfBirth.ToString("dd.MM.yyyy");
            string i = person.NumberOfPassport;
            string numberOfPassport = $"{i.Substring(0, 2)}+{i.Substring(2, 2)}+{i.Substring(4,6)}";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://service.nalog.ru/");

                string data = $"c=find&captcha=&captchaToken=&fam={lastName}&nam={firstName}&otch={patronymic}&bdate={dateOfBirth}&doctype=21&docno={numberOfPassport}&docdt=";

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

                        return responseWithInn.Inn;
                    }
                    else
                    {
                        throw new Exception("Запрос № 2 потерпел неудачу");
                    }
                }
                else
                {
                    throw new Exception("Запрос № 1 потерпел неудачу");
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
    }
}