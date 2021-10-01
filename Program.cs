using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace astro
{
    class Program
    {
        private static readonly string ISS_API_URL = "http://api.open-notify.org/astros.json";
        private static readonly string _nameHeader = "Name";
        private static readonly string _craftHeader = "Craft";
        private const int _minNameHeader = 5;
        private const int _minCraftHeader = 6;
        private static ISSResponseData _deserializedResponse;

        static async Task Main(string[] args)
        {
            _deserializedResponse = await GetISSData();
            if (_deserializedResponse != null)
            {
                PrintResults(_deserializedResponse);
            }
        }

        private static async Task<ISSResponseData> GetISSData()
        {
            ISSResponseData deserializedResponse = null;

            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetStringAsync(ISS_API_URL);
                    deserializedResponse = JsonSerializer.Deserialize<ISSResponseData>(response);
                }
            }
            catch (Exception e)
            {
                System.Console.WriteLine("There was a problem with your request: " + e.Message);
            }
            return deserializedResponse;
        }

        private static void PrintResults(ISSResponseData deserializedResponse)
        {
            int nameHeaderLength = Math.Max(deserializedResponse.people.Max(p => p.name).Length, _minNameHeader) + 3;
            int craftHeaderLength = Math.Max(deserializedResponse.people.Max(p => p.craft).Length + 1, _minCraftHeader);

            PrintHeader(nameHeaderLength, craftHeaderLength);
            PrintHeaderTableData(deserializedResponse, nameHeaderLength, craftHeaderLength);
        }

        private static void PrintHeader(int maxNameLength, int maxCraftLength)
        {
            Console.WriteLine($"{_nameHeader.PadRight(maxNameLength)}| {_craftHeader.PadRight(maxCraftLength)}");
            Console.WriteLine(new string('-', maxNameLength) + '|' + new string('-', maxCraftLength));
        }

        private static void PrintHeaderTableData(ISSResponseData deserializedResponse, int maxNameLength, int maxCraftLength)
        {
            foreach (Person p in deserializedResponse.people)
            {
                Console.WriteLine("{0}| {1}", p.name.PadRight(maxNameLength), p.craft.PadRight(maxCraftLength));
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

