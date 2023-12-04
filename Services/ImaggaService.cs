using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.DataProtection;
using RestSharp;
using System.Net;
using Azure;
using Newtonsoft.Json;

namespace ICream.Services
{
    public class ImaggaService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "acc_7146f69cbae6523";
        private readonly string _apiSecret = "4bf2a1926df4a5cf2f5ff2f2aae56626";

        public ImaggaService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }


        public async Task<bool> IsIceCream(string imageUrl)
        {
            string imgUrl = imageUrl;

            string basicAuthValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_apiKey}:{_apiSecret}"));

            // Create a RestClient for the Imagga API endpoint
            var client = new RestClient("https://api.imagga.com/v2/tags");

            // Create a request with the image URL as a query parameter and the Authorization header
            var request = new RestRequest();
            request.AddParameter("image_url", imageUrl);
            request.AddHeader("Authorization", $"Basic {basicAuthValue}");

            // Execute the request
            var response = await client.ExecuteAsync(request);

            // Check if the response is successful
            if (response.StatusCode == HttpStatusCode.OK)
            {
                // Parse the response JSON to get the tags
                var jsonResponse = JsonConvert.DeserializeObject<dynamic>(response.Content);
                var tags = jsonResponse["result"]["tags"];

                // Check if "ice cream" is one of the tags
                foreach (var tag in tags)
                {
                    if (tag["tag"].ToString().Equals("{\r\n  \"en\": \"ice cream\"\r\n}", StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
            }

            return false; // If "ice cream" tag was not found or there was an error
        }









        // Define the data structures needed to deserialize the Imagga API response.
        public class ImaggaApiResponse
        {
            public ImaggaResult Result { get; set; }
        }

        public class ImaggaResult
        {
            public List<ImaggaTag> Tags { get; set; }
        }

        public class ImaggaTag
        {
            public string Tag { get; set; }
        }
    }
}


