using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace astro
{
    class Program
    {
        private static readonly string ISS_API_URL = "http://api.open-notify.org/astros.json";
        static async Task Main(string[] args)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetStringAsync(ISS_API_URL);
                var deserializedResponse = JsonSerializer.Deserialize<ISSResponseData>(response);
                System.Console.WriteLine(deserializedResponse.number.ToString());
            }
        }
    }
    internal class ISSResponseData
    {
        public IEnumerable<Person> people { get; set; }
        public int number { get; set; }
        public string message { get; set; }
    }

    internal class Person
    {
        public string craft { get; set; }
        public string name { get; set; }
    }
}

