using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Co_Banking_System.Options;
using Co_Banking_System.Models;

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

      // Configure default request headers
      _httpClient.DefaultRequestHeaders.Clear();
      _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _settings.ApiKey);
      _httpClient.DefaultRequestHeaders.Add("X-Target-Environment", "sandbox");
      _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

  
    public async Task<string> RequestToPayAsync(string externalId, string payerId, decimal amount, string currency, string payerMessage, string payeeNote)
    {
      var requestUri = $"{_settings.BaseUrl}/collection/v1_0/requesttopay".Trim();
      var requestId = Guid.NewGuid().ToString();

      var jsonContent = JsonSerializer.Serialize(new
      {
        amount = amount.ToString("F2"), // Ensure proper decimal format
        currency,
        externalId,
        payer = new { partyIdType = "MSISDN", partyId = payerId },
        payerMessage,
        payeeNote,
        callbackUrl = "https://webhook.site/69a88354-f230-49b1-8026-aa9d7889c0ed"
      });

      var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

      // Clear previous headers to avoid duplication
      _httpClient.DefaultRequestHeaders.Remove("X-Reference-Id");
      _httpClient.DefaultRequestHeaders.Remove("Authorization");

      _httpClient.DefaultRequestHeaders.Add("X-Reference-Id", requestId);
      _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _settings.AccessToken);
      // adding timeout functionality

      _httpClient.Timeout = TimeSpan.FromSeconds(300);

      // Log the request for debugging
      Console.WriteLine($"Request URL: {requestUri}");
      Console.WriteLine($"Request Headers: {JsonSerializer.Serialize(_httpClient.DefaultRequestHeaders)}");
      Console.WriteLine($"Request Body: {jsonContent}");

      var response = await _httpClient.PostAsync(requestUri, content);

      if (!response.IsSuccessStatusCode)
      {
        var responseBody = await response.Content.ReadAsStringAsync();
        throw new Exception($"Request failed with status code {response.StatusCode} and response: {responseBody}");
      }

      return await response.Content.ReadAsStringAsync();
    }

  

    // creating a payment route 
    public async Task<string> CreatePaymentAsync(CreatePaymentModel model)
    {
      var requestUri = $"{_settings.BaseUrl}/collection/v2_0/payment".Trim();
      var requestId = Guid.NewGuid().ToString();

      var jsonContent = JsonSerializer.Serialize(new
      {
        externalTransactionId = model.ExternalTransactionId,
        money = new
        {
          amount = model.Amount.ToString("F2"), // Ensure proper decimal format
          currency = model.Currency
        },
        customerReference = model.CustomerReference,
        serviceProviderUserName = model.ServiceProviderUserName,
        receiverMessage = model.ReceiverMessage,
        senderNote = model.SenderNote
      });

      var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

      // Clear previous headers to avoid duplication
      _httpClient.DefaultRequestHeaders.Remove("X-Reference-Id");
      _httpClient.DefaultRequestHeaders.Remove("Authorization");

      _httpClient.DefaultRequestHeaders.Add("X-Reference-Id", requestId);
      _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _settings.AccessToken);

      // Log the request for debugging
      Console.WriteLine($"Request URL: {requestUri}");
      Console.WriteLine($"Request Headers: {JsonSerializer.Serialize(_httpClient.DefaultRequestHeaders)}");
      Console.WriteLine($"Request Body: {jsonContent}");

      var response = await _httpClient.PostAsync(requestUri, content);

      if (!response.IsSuccessStatusCode)
      {
        var responseBody = await response.Content.ReadAsStringAsync();
        throw new Exception($"Request failed with status code {response.StatusCode} and response: {responseBody}");
      }

      return await response.Content.ReadAsStringAsync();
    }


  }
}

