using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Co_Banking_System.Services
{
    public class AirtelApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AirtelApiClient> _logger;
        private readonly IConfiguration _configuration;
        private string? _accessToken;

        public AirtelApiClient(IHttpClientFactory httpClientFactory, ILogger<AirtelApiClient> logger, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient();
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<string> GetAccessToken()
        {
            var tokenRequestBody = new
            {
                client_id = _configuration["AirtelApi:ClientId"],
                client_secret = _configuration["AirtelApi:ClientSecret"],
                grant_type = "client_credentials"
            };

            var jsonContent = new StringContent(JsonConvert.SerializeObject(tokenRequestBody), Encoding.UTF8, "application/json");

            var tokenUrl = _configuration["AirtelApi:TokenUrl"];
            if (string.IsNullOrEmpty(tokenUrl))
            {
                throw new InvalidOperationException("Token URL is not configured.");
            }

            try
            {
                var response = await _httpClient.PostAsync(tokenUrl, jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseContent);
                    if (tokenResponse?.access_token == null)
                    {
                        throw new InvalidOperationException("Token response is invalid.");
                    }
                    _accessToken = tokenResponse.access_token;
                    _logger.LogInformation($"Access Token: {_accessToken}");
                    return _accessToken;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Token request failed with status code: {response.StatusCode}, content: {errorContent}");
                    throw new HttpRequestException($"Token request failed with status code: {response.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"HttpRequestException: {ex.Message}");
                throw;
            }
            catch (JsonException ex)
            {
                _logger.LogError($"JsonException: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception: {ex.Message}");
                throw;
            }
        }

        public async Task<string> MakeCashInRequest(string endpoint, object requestBody)
        {
            if (_accessToken == null)
            {
                // Obtain access token if not already available
                _accessToken = await GetAccessToken();
            }

            if (_accessToken != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
            }
            else
            {
                throw new InvalidOperationException("Access token is null.");
            }

            var baseUrl = _configuration["AirtelApi:BaseUrl"];
            if (string.IsNullOrEmpty(baseUrl))
            {
                throw new InvalidOperationException("Base URL is not configured.");
            }

            var jsonContent = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            var url = new Uri(new Uri(baseUrl), endpoint);

            var response = await _httpClient.PostAsync(url, jsonContent);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError($"Airtel API request failed with status code: {response.StatusCode}, content: {errorContent}");
                throw new HttpRequestException($"Airtel API request failed with status code: {response.StatusCode}");
            }
        }

        private class TokenResponse
        {
            public string? access_token { get; set; }
        }
    }
}
