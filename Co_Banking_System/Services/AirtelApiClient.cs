using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using Co_Banking_System.Options;

namespace Co_Banking_System.Services
{
    public class AirtelApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly AirtelApiOptions _options;

        public AirtelApiClient(IOptions<AirtelApiOptions> options)
        {
            _options = options.Value;
            _httpClient = new HttpClient();
        }

        public async Task<string> MakeCashInRequest(string endpoint, object requestBody)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_options.BearerToken}");
            _httpClient.DefaultRequestHeaders.Add("X-Country", "UG");
            _httpClient.DefaultRequestHeaders.Add("X-Currency", "UGX");
            _httpClient.DefaultRequestHeaders.Add("apiKey", _options.ApiKey);

            var jsonContent = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            var url = new Uri(new Uri(_options.BaseUrl), endpoint);

            HttpResponseMessage response = await _httpClient.PostAsync(url, jsonContent);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new HttpRequestException($"Airtel API request failed with status code: {response.StatusCode}");
            }
        }
    }
}
