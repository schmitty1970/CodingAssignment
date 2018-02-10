using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Example.Console.TestHarness
{
    /// <summary>
    /// Console app to: 
    /// 1. system test harness either locally, or once the REST Api is deployed there,  
    /// 2. demonstrate how to call the API
    /// 3. demonstrate how to call the API using normal GUID string for retrieval
    /// 4. demonstrate how to call the API using an encoded and shortened GUID string for retrieval
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            string baseAddress = "http://localhost:51211/api/";

            string plainText1 = "Test Data Number 1 - test 1";

            System.Console.WriteLine("========================================");
            System.Console.WriteLine("TESTING 1");
            System.Console.WriteLine($"ORIGINAL: {plainText1}");
            Harness t1 = new Harness(baseAddress, plainText1);
            t1.Test().Wait();

            System.Console.WriteLine("========================================");
            System.Console.WriteLine("Finished testing 1...");

            System.Console.WriteLine(" ");

            string plainText2 = "Data to test 2 - test 2";

            System.Console.WriteLine("========================================");
            System.Console.WriteLine("TESTING 2");
            System.Console.WriteLine($"ORIGINAL: {plainText2}");
            Harness t2 = new Harness(baseAddress, plainText2);
            t2.Test().Wait();

            System.Console.WriteLine("========================================");
            System.Console.WriteLine("Finished testing 2...");
            System.Console.ReadKey();
        }  
                
    }

    public class Harness
    {
        private const string AcceptMediaType = "application/json";
        private const string ClientAuthToken = "TestHarness";
        private readonly string _baseAddress;
        private readonly string _plainText;

        public Harness(string baseAddress, string plainText)
        {
            _baseAddress = baseAddress;
            _plainText = plainText;
        }

        /// <summary>
        /// Test a NON-ENCODED Guid when retrieving
        /// </summary>
        public async Task Test()
        {
            var token = await Store();
            System.Console.WriteLine($"TOKEN: {token}");

            var unecrypted = await Retrieve(token);
            System.Console.WriteLine($"UNENCRYPTED: {unecrypted}");
            System.Console.WriteLine($"ASSERTION (EQUALS): {_plainText.Equals(unecrypted)}");
        }

        /// <summary>
        /// Call the Store endpoint
        /// </summary>        
        public async Task<string> Store()
        {
            var client = new HttpClient {BaseAddress = new Uri(_baseAddress)};
            client.DefaultRequestHeaders.Add("Authorization", $"WeakCustom {ClientAuthToken}"); 

            var response = await client.PostAsync("store/",
                new StringContent(JsonConvert.SerializeObject(_plainText), Encoding.UTF8, AcceptMediaType));

            var token = string.Empty;
            if (response.StatusCode == HttpStatusCode.Created)
            {
                var content = await response.Content.ReadAsStringAsync();
                token = JsonConvert.DeserializeObject<string>(content);
            }

            return token;
        }

        /// <summary>
        /// Call the Retrieve endpoint
        /// </summary>  
        public async Task<string> Retrieve(string token)
        {
            var client = new HttpClient { BaseAddress = new Uri(_baseAddress) };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(AcceptMediaType));
            client.DefaultRequestHeaders.Add("Authorization", $"WeakCustom {ClientAuthToken}");

            var response = await client.GetAsync($"Retrieve/{token}");

            var decryptedValue = "DID NOT WORK...............";
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                decryptedValue = JsonConvert.DeserializeObject<string>(content);
            }

            return decryptedValue;
        }
    }
}
