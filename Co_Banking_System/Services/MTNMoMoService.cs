using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Co_Banking_System.Options;

namespace Co_Banking_System.Services
{
  // Service class to interact with MTN MoMo API
  public class MtnMomoService
  {
    // HTTP client for sending requests
    private readonly HttpClient _httpClient;
    // Options object to hold MTN MoMo API settings
    private readonly MoMoApiOptions _settings;

    // Constructor to initialize the service with an HttpClient and MoMoApiOptions
    public MtnMomoService(HttpClient httpClient, IOptions<MoMoApiOptions> settings)
    {
      _httpClient = httpClient;
      _settings = settings.Value;

      // Configure default request headers
      _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _settings.ApiKey);
      _httpClient.DefaultRequestHeaders.Add("X-Target-Environment", "sandbox");
      _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    // Method to create an API user
    public async Task<string> CreateApiUserAsync()
    {
      var requestUri = $"{_settings.BaseUrl}/v1_0/apiuser";
      var callbackHost = "https://webhook.site/69a88354-f230-49b1-8026-aa9d7889c0ed";
      var jsonContent = JsonSerializer.Serialize(new { providerCallbackHost = callbackHost });
      var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

      var response = await _httpClient.PostAsync(requestUri, content);
      response.EnsureSuccessStatusCode();

      return await response.Content.ReadAsStringAsync();
    }

    // Method to request for payment
    public async Task<string> RequestToPayAsync(string externalId, string payerId, decimal amount, string currency, string payerMessage, string payeeNote)
    {
      var requestUri = $"{_settings.BaseUrl}/collection/v1_0/requesttopay";
      var requestId = Guid.NewGuid().ToString();

      var jsonContent = JsonSerializer.Serialize(new
      {
        amount = amount.ToString(),
        currency,
        externalId,
        payer = new { partyIdType = "MSISDN", partyId = payerId },
        payerMessage,
        payeeNote,
        callbackUrl = "https://webhook.site/69a88354-f230-49b1-8026-aa9d7889c0ed"
      });

      var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
      _httpClient.DefaultRequestHeaders.Add("X-Reference-Id", requestId);
      _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _settings.AccessToken);

      var response = await _httpClient.PostAsync(requestUri, content);
      response.EnsureSuccessStatusCode();

      return await response.Content.ReadAsStringAsync();
    }

    // Method to get the payment status
    public async Task<string> GetRequestToPayStatusAsync(string requestId)
    {
      var requestUri = $"{_settings.BaseUrl}/collection/v1_0/requesttopay/{requestId}";
      var response = await _httpClient.GetAsync(requestUri);
      response.EnsureSuccessStatusCode();
      return await response.Content.ReadAsStringAsync();
    }
  }
}
