using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Co_Banking_System.Services;

namespace Co_Banking_System.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class MtnMomoController : ControllerBase
  {
    private readonly MtnMomoService _mtnMomoService;

    public MtnMomoController(MtnMomoService mtnMomoService)
    {
      _mtnMomoService = mtnMomoService ?? throw new ArgumentNullException(nameof(mtnMomoService));
    }

    [HttpPost("create-api-user")]
    public async Task<IActionResult> CreateApiUser()
    {
      try
      {
        var result = await _mtnMomoService.CreateApiUserAsync();
        return Ok(result);
      }
      catch (Exception ex)
      {
        return StatusCode(500, $"An error occurred while creating the API user: {ex.Message}");
      }
    }

    [HttpPost("request-to-pay")]
    public async Task<IActionResult> RequestToPay(string externalId, string payerId, decimal amount, string currency, string payerMessage, string payeeNote)
    {
      if (string.IsNullOrEmpty(externalId) || string.IsNullOrEmpty(payerId) || amount <= 0 || string.IsNullOrEmpty(currency) || string.IsNullOrEmpty(payerMessage) || string.IsNullOrEmpty(payeeNote))
      {
        return BadRequest("Invalid request parameters.");
      }

      try
      {
        var result = await _mtnMomoService.RequestToPayAsync(externalId, payerId, amount, currency, payerMessage, payeeNote);
        return Ok(result);
      }
      catch (Exception ex)
      {
        return StatusCode(500, $"An error occurred while processing the payment request: {ex.Message}");
      }
    }

    [HttpGet("request-to-pay-status/{requestId}")]
    public async Task<IActionResult> GetRequestToPayStatus(string requestId)
    {
      if (string.IsNullOrEmpty(requestId))
      {
        return BadRequest("Request ID is required.");
      }

      try
      {
        var result = await _mtnMomoService.GetRequestToPayStatusAsync(requestId);
        return Ok(result);
      }
      catch (Exception ex)
      {
        return StatusCode(500, $"An error occurred while fetching payment status: {ex.Message}");
      }
    }
  }
}
