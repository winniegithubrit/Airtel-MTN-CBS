using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Co_Banking_System.Models;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace Co_Banking_System.Services
{
  public class MtnDisbursementService
  {
    private readonly HttpClient _httpClient;
    private readonly DisbursementSettings _settings;
    private readonly ILogger<MtnDisbursementService> _logger;

    // Constructor to initialize HttpClient, MtnMomoSettings, and Logger
    public MtnDisbursementService(HttpClient httpClient, IOptions<DisbursementSettings> settings, ILogger<MtnDisbursementService> logger)
    {
      _httpClient = httpClient;
      _settings = settings.Value;
      _logger = logger;
      // Set default headers for HttpClient
      _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
      _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _settings.ApiKey);
    }


    // GETACCOUNTBALANCE
    public async Task<AccountBalanceResponse?> GetAccountBalanceAsync(string accessToken, string targetEnvironment)
    {
      _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
      _httpClient.DefaultRequestHeaders.Add("X-Target-Environment", targetEnvironment);

      try
      {
        var response = await _httpClient.GetAsync($"{_settings.BaseUrl}/disbursement/v1_0/account/balance");

        if (response.IsSuccessStatusCode)
        {
          var jsonResponse = await response.Content.ReadAsStringAsync();
          _logger.LogInformation($"Received successful response from MTN API: {jsonResponse}");
          return JsonConvert.DeserializeObject<AccountBalanceResponse>(jsonResponse);
        }
        else
        {
          var errorResponse = await response.Content.ReadAsStringAsync();
          _logger.LogError($"Failed to get account balance with status code {response.StatusCode}: {errorResponse}");
          throw new Exception($"Failed to get account balance: {errorResponse} (Status Code: {response.StatusCode})");
        }
      }
      catch (Exception ex)
      {
        _logger.LogError($"An error occurred while getting account balance: {ex.Message}");
        throw;
      }
    }
    // DEPOSIT FUNCTIONALITY
    public async Task<DepositResponse?> DepositAsync(string accessToken, string targetEnvironment, DepositRequest depositRequest)
    {
      // Set authorization and target environment headers
      _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
      _httpClient.DefaultRequestHeaders.Remove("X-Target-Environment");
      _httpClient.DefaultRequestHeaders.Add("X-Target-Environment", targetEnvironment);
      _httpClient.DefaultRequestHeaders.Add("X-Reference-Id", Guid.NewGuid().ToString());

      // Serialize the deposit request to JSON
      var jsonRequest = JsonConvert.SerializeObject(depositRequest);
      var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

      // Log the outgoing request
      _logger.LogInformation($"Sending request to MTN API: {jsonRequest}");

      // Send the POST request to the MTN API
      var response = await _httpClient.PostAsync($"{_settings.BaseUrl}/disbursement/v2_0/deposit", content);

      // Check if the response is successful
      if (response.IsSuccessStatusCode)
      {
        // Read and deserialize the successful response
        var jsonResponse = await response.Content.ReadAsStringAsync();
        _logger.LogInformation($"Received successful response from MTN API: {jsonResponse}");
        return JsonConvert.DeserializeObject<DepositResponse>(jsonResponse);
      }
      else
      {
        // Read and log the error response
        var errorResponse = await response.Content.ReadAsStringAsync();
        _logger.LogError($"Deposit failed with status code {response.StatusCode}: {errorResponse}");
        // Provide more details in the exception message
        throw new Exception($"Deposit failed: {errorResponse} (Status Code: {response.StatusCode})");
      }
    }
  }
}
