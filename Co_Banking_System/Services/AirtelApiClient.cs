using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using Co_Banking_System.Options;

namespace Co_Banking_System.Services
{
    public class AirtelApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly AirtelApiOptions _settings;
        private readonly ILogger<AirtelApiClient> _logger;
        private string? _accessToken;

        public AirtelApiClient(HttpClient httpClient, IOptions<AirtelApiOptions> settings, ILogger<AirtelApiClient> logger)
        {
            _httpClient = httpClient;
            _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Log the configuration values
            _logger.LogInformation($"ClientId: {_settings.ClientId ?? "null"}");
            _logger.LogInformation($"ClientSecret: {new string('*', _settings.ClientSecret?.Length ?? 0)}");
            _logger.LogInformation($"TokenUrl: {_settings.TokenUrl ?? "null"}");
        }

        public async Task<string> GetAccessToken()
        {
            if (_settings.ClientId == null || _settings.ClientSecret == null || _settings.TokenUrl == null)
            {
                throw new InvalidOperationException("API settings are not configured properly.");
            }

            var tokenRequestBody = new
            {
                client_id = _settings.ClientId,
                client_secret = _settings.ClientSecret,
                grant_type = "client_credentials"
            };

            var jsonContent = new StringContent(JsonConvert.SerializeObject(tokenRequestBody), Encoding.UTF8, "application/json");
            var tokenUrl = _settings.TokenUrl;

            _logger.LogInformation($"Sending token request to URL: {tokenUrl} with body: {JsonConvert.SerializeObject(tokenRequestBody)}");

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

            var baseUrl = _settings.BaseUrl ?? throw new InvalidOperationException("Base URL is not configured.");
            if (string.IsNullOrEmpty(endpoint))
            {
                throw new ArgumentException("Endpoint cannot be null or empty", nameof(endpoint));
            }

            var jsonContent = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            var url = new Uri(new Uri(baseUrl), endpoint);

            try
            {
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
            catch (HttpRequestException ex)
            {
                _logger.LogError($"HttpRequestException: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception: {ex.Message}");
                throw;
            }
        }

        // New method for cashout functionality
        public async Task<string> MakeCashOutRequest(string endpoint, object requestBody)
        {
            if (_accessToken == null)
            {
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

            var baseUrl = _settings.BaseUrl ?? throw new InvalidOperationException("Base URL is not configured.");
            if (string.IsNullOrEmpty(endpoint))
            {
                throw new ArgumentException("Endpoint cannot be null or empty", nameof(endpoint));
            }

            var jsonContent = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            var url = new Uri(new Uri(baseUrl), endpoint);

            try
            {
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
            catch (HttpRequestException ex)
            {
                _logger.LogError($"HttpRequestException: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception: {ex.Message}");
                throw;
            }
        }

        private class TokenResponse
        {
            public string? access_token { get; set; }
        }
    }
}
