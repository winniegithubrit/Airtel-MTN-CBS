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
        private string? _accessToken;

        public AirtelApiClient(HttpClient httpClient, IOptions<AirtelApiOptions> options)
        {
            _httpClient = httpClient;
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task<string> GetAccessTokenAsync()
        {
            var tokenUrl = new Uri(new Uri(_options.BaseUrl), "auth/oauth2/token");
            var content = new StringContent(
                $"grant_type=client_credentials&client_id={Uri.EscapeDataString(_options.ClientId)}&client_secret={Uri.EscapeDataString(_options.ClientSecret)}",
                Encoding.UTF8,
                "application/x-www-form-urlencoded");

            HttpResponseMessage response = await _httpClient.PostAsync(tokenUrl, content);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Failed to get access token. Status code: {response.StatusCode}, Reason: {response.ReasonPhrase}");
            }

            var responseString = await response.Content.ReadAsStringAsync();
            dynamic? responseObject = JsonConvert.DeserializeObject(responseString);

            if (responseObject == null || responseObject.access_token == null)
            {
                throw new InvalidOperationException("Failed to obtain access token from Airtel API.");
            }

            _accessToken = responseObject.access_token;
            return _accessToken;
        }

        private async Task EnsureAccessTokenAsync()
        {
            if (string.IsNullOrEmpty(_accessToken))
            {
                _accessToken = await GetAccessTokenAsync();
            }
        }

        public async Task<string> MakeCashInRequest(string endpoint, object requestBody)
        {
            await EnsureAccessTokenAsync();

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_accessToken}");
            _httpClient.DefaultRequestHeaders.Add("X-Country", "UG");
            _httpClient.DefaultRequestHeaders.Add("X-Currency", "UGX");

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
