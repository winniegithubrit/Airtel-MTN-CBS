using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Co_Banking_System.Options;

namespace Co_Banking_System.Services
{
  public class MtnMomoService
  {
    private readonly HttpClient _httpClient;
    private readonly MoMoApiOptions _settings;

    public MtnMomoService(HttpClient httpClient, IOptions<MoMoApiOptions> settings)
    {
      _httpClient = httpClient;
      _settings = settings.Value;
    }

    public async Task<string> CreateApiUserAsync()
    {
      var requestUri = $"{_settings.BaseUrl}/v1_0/apiuser";

      // Clear existing headers and add the subscription key
      _httpClient.DefaultRequestHeaders.Clear();
      _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _settings.ApiKey);

      var content = new StringContent("{\"providerCallbackHost\": \"69a88354-f230-49b1-8026-aa9d7889c0ed\"}", Encoding.UTF8, "application/json");

      var response = await _httpClient.PostAsync(requestUri, content);
      response.EnsureSuccessStatusCode();

      return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> RequestToPayAsync(string externalId, string payerId, decimal amount, string currency, string payerMessage, string payeeNote)
    {
      var requestUri = $"{_settings.BaseUrl}/collection/v1_0/requesttopay";
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

      var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(jsonContent), Encoding.UTF8, "application/json");

      //  adding necessary headers
      _httpClient.DefaultRequestHeaders.Clear();
      _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _settings.ApiKey);
      _httpClient.DefaultRequestHeaders.Add("X-Reference-Id", requestId);
      _httpClient.DefaultRequestHeaders.Add("X-Target-Environment", "sandbox");
      _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _settings.AccessToken);

      var response = await _httpClient.PostAsync(requestUri, content);
      response.EnsureSuccessStatusCode();

      return await response.Content.ReadAsStringAsync();
    }

    public async Task<string> GetRequestToPayStatusAsync(string requestId)
    {
      var requestUri = $"{_settings.BaseUrl}/collection/v1_0/requesttopay/{requestId}";

      // Clear existing headers and add necessary headers
      _httpClient.DefaultRequestHeaders.Clear();
      _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _settings.ApiKey);
      _httpClient.DefaultRequestHeaders.Add("X-Target-Environment", "sandbox");
      _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _settings.AccessToken);

      var response = await _httpClient.GetAsync(requestUri);
      response.EnsureSuccessStatusCode();

      return await response.Content.ReadAsStringAsync();
    }
  }
}
