using System;
using System.Net.Http;
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
                System.Console.WriteLine(response.ToString());
            }
        }
    }
}
