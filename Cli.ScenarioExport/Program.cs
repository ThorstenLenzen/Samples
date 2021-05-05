using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Cli.ScenarioExport
{


    class Program
    {
        private static string _baseUrl;

        private const string ScenarioEndpoint = "ldscenario";
        private const string ExportEndpoint = "ldexport";

        private static readonly Guid ScenarioGuid = Guid.Parse("66fb0844-cc2b-47d0-9443-00245e7792f2");

        public static async Task Main(string[] args)
        {
            //var targetSystem = "Local";
            var targetSystem = "Nightly";

            SelectBaseUrl(targetSystem);

            var token = await GetAccessToken();

            var startSuccess =  await StartConstruction(token);

            if (!startSuccess)
            {
                DisplayUnsuccessful();
                return;
            }

            var waitSuccess = await WaitForConstructionToBeFinished(token);

            if (!waitSuccess)
            {
                DisplayUnsuccessful();
                return;
            }

            var retrieveSuccess = await RetrieveConstructedScenario(token);

            if (!retrieveSuccess)
            {
                DisplayUnsuccessful();
                return;
            }

            Console.WriteLine("LD Export finished successful.");
            Console.WriteLine("Press <Enter> to terminate...");
            Console.ReadLine();
        }

        private static void SelectBaseUrl(string targetSystem)
        {
            var abbreviation = targetSystem
                .Substring(0, 1)
                .ToLower();

            var target = "Local";

            switch (abbreviation)
            {
                case "n":
                    _baseUrl = "https://db-analytics-api.bku-web.db.de/planitnightly/";
                    target = "Nightly";
                    break;
                case "s":
                    _baseUrl = "https://db-analytics-api.bku-web.db.de/planitsprintly/";
                    target = "Sprint";
                    break;
                case "a":
                    _baseUrl = "https://db-analytics-api.bku-web.db.de/planitapproval/";
                    target = "Approval";
                    break;
                case "p":
                    _baseUrl = "https://db-analytics-api.bku-web.db.de/planit/";
                    target = "Production";
                    break;
                default:
                    _baseUrl = "http://localhost/planit/v1/";
                    break;
            }


            Console.WriteLine($"Target System is '{target}'.");
    }

    private static void DisplayUnsuccessful()
        {
            Console.WriteLine("LD Export failed.");
            Console.WriteLine("Press <Enter> to terminate...");
            Console.ReadLine();
        }

        private static async Task<bool> RetrieveConstructedScenario(string token)
        {
            Console.WriteLine($"Retrieving constructed scenario for '{ScenarioGuid}'...");

            var client = CreateHttpClient(token);
            var response = await client.GetAsync(
                $"{_baseUrl}{ScenarioEndpoint}/{ScenarioGuid}", 
                HttpCompletionOption.ResponseHeadersRead);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                Console.WriteLine($"Retrieving constructed scenario for '{ScenarioGuid}' was not successful.");
                Console.WriteLine($"Status code: '{response.StatusCode}'\nResponse: '{await response.Content.ReadAsStringAsync()}'");
                return false;
            }

            await using var streamToReadFrom = await response.Content.ReadAsStreamAsync();
            
            var fileToWriteTo = $"{AppDomain.CurrentDomain.BaseDirectory}\\{Guid.NewGuid()}.json";

            Console.WriteLine($"Writing scenario to '{fileToWriteTo}'.");
                    
            await using Stream streamToWriteTo = File.Create(fileToWriteTo);
            await streamToReadFrom.CopyToAsync(streamToWriteTo);

            Console.WriteLine($"Constructed scenario for '{ScenarioGuid}' successfully retrieved.");
            return true;
        }

        private static async Task<bool> StartConstruction(string token)
        {
            Console.WriteLine($"Requesting Scenario construction start for '{ScenarioGuid}'...");

            var client = CreateHttpClient(token);
            var json = $"{{\"scenarioId\": \"{ScenarioGuid}\"}}";
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{_baseUrl}{ExportEndpoint}", content);

            if (response.StatusCode != HttpStatusCode.Accepted)
            {
                Console.WriteLine($"Requesting Scenario construction start for '{ScenarioGuid}' was not successful.");
                Console.WriteLine($"Status code: '{response.StatusCode}'\nResponse: '{await response.Content.ReadAsStringAsync()}'");
                return false;
            }
                
            Console.WriteLine($"Construction request for scenario '{ScenarioGuid}' was accepted.");
            return true;
        }

        private static async Task<bool> WaitForConstructionToBeFinished(string token)
        {
            Console.WriteLine("Waiting for construction to be finished...");

            var client = CreateHttpClient(token);
            var response = new HttpResponseMessage();
            var start = DateTime.UtcNow;

            while (response.StatusCode != HttpStatusCode.Found)
            {
                Thread.Sleep(3000);
                response = await client.GetAsync($"{_baseUrl}{ExportEndpoint}/{ScenarioGuid}");

                if (response.StatusCode != HttpStatusCode.Found && response.StatusCode != HttpStatusCode.Accepted)
                {
                    Console.WriteLine($"Error during wait occurred.\nStatus code: '{response.StatusCode}'\nResponse: '{await response.Content.ReadAsStringAsync()}'");
                    return false;
                }

                Console.WriteLine($"Waiting since {(DateTime.UtcNow - start).TotalSeconds} secs...");
            }

            Console.WriteLine("Construction is finished...");
            return true;
        }

        public static async Task<string> GetAccessToken()
        {
            Console.WriteLine("Querying access token...");

            const string apiKey = "N2F8AE25AD0F4476DAF070DD2D5CC07CA6625786EA90745DA93A2BA6B6E9D";

            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("apikey", apiKey);

            var result = await client.GetAsync($"{_baseUrl}token");
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
