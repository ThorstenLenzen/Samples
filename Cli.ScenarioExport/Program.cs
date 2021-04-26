using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace TestClient.LdExport
{
    class Program
    {
        //private const string BaseUrl = "http://localhost/planit/v1/";
        //private const string BaseUrl = "https://db-analytics-api.bku-web.db.de/planit/";
        //private const string BaseUrl = "https://db-analytics-api.bku-web.db.de/planitnightly/";
        //private const string BaseUrl = "https://db-analytics-api.bku-web.db.de/planitsprintly/";
        private const string BaseUrl = "https://db-analytics-api.bku-web.db.de/planitapproval/";


        private const string ScenarioEndpoint = "ldscenario";
        private const string ExportEndpoint = "ldexport";

        private static readonly Guid ScenarioGuid = Guid.Parse("7e4f01ae-402b-4d39-b780-e49c874cc814");

        public static async Task Main(string[] args)
        {
            var token = await GetAccessToken();

            await StartConstruction(token);

            await WaitForConstructionToBeFinished(token);

            await RetrieveConstructedScenario(token);

            Console.WriteLine("LD Export finished.");
            Console.WriteLine("Press <Enter> to terminate...");
            Console.ReadLine();
        }

        private static async Task RetrieveConstructedScenario(string token)
        {
            Console.WriteLine($"Retrieving constructed scenario for '{ScenarioGuid}'...");

            var client = CreateHttpClient(token);
            var response = await client.GetAsync(
                $"{BaseUrl}{ScenarioEndpoint}/{ScenarioGuid}", 
                HttpCompletionOption.ResponseHeadersRead);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                Console.WriteLine($"Retrieving constructed scenario for '{ScenarioGuid}' was not successful.");
            }

            await using (var streamToReadFrom = await response.Content.ReadAsStreamAsync())
            {
                var fileToWriteTo = $"{AppDomain.CurrentDomain.BaseDirectory}\\{Guid.NewGuid()}.json";

                Console.WriteLine($"Writing scenario to '{fileToWriteTo}'.");

                await using Stream streamToWriteTo = File.Create(fileToWriteTo);
                await streamToReadFrom.CopyToAsync(streamToWriteTo);
            }

            Console.WriteLine($"Constructed scenario for '{ScenarioGuid}' successfully retrieved.");
        }

        private static async Task StartConstruction(string token)
        {
            Console.WriteLine($"Requesting Scenario construction start for '{ScenarioGuid}'...");

            var client = CreateHttpClient(token);
            var json = $"{{\"scenarioId\": \"{ScenarioGuid}\"}}";
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{BaseUrl}{ExportEndpoint}", content);

            if (response.StatusCode != HttpStatusCode.Accepted)
            {
                Console.WriteLine($"Requesting Scenario construction start for '{ScenarioGuid}' was not successful.");
            }
                
            Console.WriteLine($"Construction request for scenario '{ScenarioGuid}' was accepted.");
        }

        private static async Task WaitForConstructionToBeFinished(string token)
        {
            Console.WriteLine("Waiting for construction to be finished...");

            var client = CreateHttpClient(token);
            var response = new HttpResponseMessage();
            var start = DateTime.UtcNow;

            while (response.StatusCode != HttpStatusCode.Found)
            {
                Thread.Sleep(3000);
                response = await client.GetAsync($"{BaseUrl}{ExportEndpoint}/{ScenarioGuid}");

                Console.WriteLine($"Waiting since {(DateTime.UtcNow - start).TotalSeconds} secs...");
            }

            Console.WriteLine("Construction is finished...");
        }

        public static async Task<string> GetAccessToken()
        {
            Console.WriteLine("Querying access token...");

            const string apiKey = "N2F8AE25AD0F4476DAF070DD2D5CC07CA6625786EA90745DA93A2BA6B6E9D";

            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("apikey", apiKey);

            var result = await client.GetAsync($"{BaseUrl}token");
            var json = await result.Content.ReadAsStringAsync();
            var token = (string)(JObject.Parse(json)["data"]!["authenticationToken"]);

            Console.WriteLine("Access token successfully retrieved.");

            return token;
        }

        public static HttpClient CreateHttpClient(string accessToken)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(accessToken);
            return client;
        }
    }
}
