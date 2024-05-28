using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
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
    }
// method to create an api user
    public async Task<string> CreateApiUserAsync()
    {
      var requestUri = $"{_settings.BaseUrl}/v1_0/apiuser";

      // Clear existing headers and add the subscription key
      _httpClient.DefaultRequestHeaders.Clear();
      _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _settings.ApiKey);
// request wilh the callback host and replacing the value
      var content = new StringContent("{\"providerCallbackHost\": \"https://webhook.site/69a88354-f230-49b1-8026-aa9d7889c0ed\"}", Encoding.UTF8, "application/json");
// sending a POST request to the MTN MoMo Api
      var response = await _httpClient.PostAsync(requestUri, content);
      response.EnsureSuccessStatusCode();

      return await response.Content.ReadAsStringAsync();
    }
// method to request for payment
    public async Task<string> RequestToPayAsync(string externalId, string payerId, decimal amount, string currency, string payerMessage, string payeeNote)
    {
      var requestUri = $"{_settings.BaseUrl}/collection/v1_0/requesttopay";
      // Generate a new GUID for the request ID
      var requestId = Guid.NewGuid().ToString();
      var jsonContent = new
      // data for the body
      {
        amount = amount.ToString(),
        currency = currency,
        externalId = externalId,
        payer = new { partyIdType = "MSISDN", partyId = payerId },
        payerMessage = payerMessage,
        payeeNote = payeeNote,
        callbackUrl = "https://webhook.site/69a88354-f230-49b1-8026-aa9d7889c0ed"
      };
// serializing the body into JSON format
      var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(jsonContent), Encoding.UTF8, "application/json");

      //  adding necessary headers
      _httpClient.DefaultRequestHeaders.Clear();
      _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _settings.ApiKey);
      _httpClient.DefaultRequestHeaders.Add("X-Reference-Id", requestId);
      _httpClient.DefaultRequestHeaders.Add("X-Target-Environment", "sandbox");
      _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _settings.AccessToken);
// sending the POST request
      var response = await _httpClient.PostAsync(requestUri, content);
      response.EnsureSuccessStatusCode();
// returning response as a string type
      return await response.Content.ReadAsStringAsync();
    }
// method to get the payment status
    public async Task<string> GetRequestToPayStatusAsync(string requestId)
    {
      var requestUri = $"{_settings.BaseUrl}/collection/v1_0/requesttopay/{requestId}";

      // Clear existing headers and add necessary headers
      _httpClient.DefaultRequestHeaders.Clear();
      _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _settings.ApiKey);
      _httpClient.DefaultRequestHeaders.Add("X-Target-Environment", "sandbox");
      _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _settings.AccessToken);
// sending a GET request
      var response = await _httpClient.GetAsync(requestUri);
      response.EnsureSuccessStatusCode();
// resonse in string format
      return await response.Content.ReadAsStringAsync();
    }
  }
}
