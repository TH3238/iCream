using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ICream.Services
{

    public class AddressValidationGateway
    {
        private readonly HttpClient _httpClient;

        public AddressValidationGateway()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://localhost:5036/");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<bool> CheckAddress(string city, string street)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"api/address/valid?city={city}&street={street}");

                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    var responseJson = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseString);

                    if (responseJson.ContainsKey("exists") && responseJson["exists"] is bool exists)
                    {
                        // Here 'exists' will contain the value (true or false)
                        bool isValid = exists; // Converting 'exists' to the Boolean you need
                        return isValid;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    // Handle non-success status code
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }
    }
}