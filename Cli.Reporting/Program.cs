using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Cli.Reporting
{
    class Program
    {
        private static string _baseUrl;
        private static readonly Guid ScenarioGuid = Guid.Parse("CB925166-0531-422E-BEEA-912A7CE76555");

        public static async Task Main(string[] args)
        {
            //var targetSystem = "Local";
            var targetSystem = "Nightly";

            SelectBaseUrl(targetSystem);

            var token = await GetAccessToken();

            _ = await QueryFullData(token);

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

        private static async Task<bool> QueryFullData(string token)
        {
            Console.WriteLine($"Querying full data starts for '{ScenarioGuid}'...");

            var client = CreateHttpClient(token);
            var response = await client.GetAsync($"{_baseUrl}staging/{ScenarioGuid}/full", HttpCompletionOption.ResponseHeadersRead);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                Console.WriteLine($"Querying full data for '{ScenarioGuid}' was not successful.");
                Console.WriteLine($"Status code: '{response.StatusCode}'\nResponse: '{await response.Content.ReadAsStringAsync()}'");
                return false;
            }
                
            Console.WriteLine($"Querying full data for scenario '{ScenarioGuid}' was successful.");

            await using var streamToReadFrom = await response.Content.ReadAsStreamAsync();

            var fileToWriteTo = $"{AppDomain.CurrentDomain.BaseDirectory}\\{Guid.NewGuid()}.json";

            Console.WriteLine($"Writing data to '{fileToWriteTo}'.");

            await using Stream streamToWriteTo = File.Create(fileToWriteTo);
            await streamToReadFrom.CopyToAsync(streamToWriteTo);

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
            client.Timeout = TimeSpan.FromMinutes(30);
            return client;
        }
    }
}
