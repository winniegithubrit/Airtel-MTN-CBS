using Microsoft.AspNetCore.Mvc;
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
      _mtnMomoService = mtnMomoService;
    }

    [HttpPost("create-api-user")]
    public async Task<IActionResult> CreateApiUser()
    {
      var result = await _mtnMomoService.CreateApiUserAsync();
      return Ok(result);
    }

    [HttpPost("request-to-pay")]
    public async Task<IActionResult> RequestToPay(string externalId, string payerId, decimal amount, string currency, string payerMessage, string payeeNote)
    {
      var result = await _mtnMomoService.RequestToPayAsync(externalId, payerId, amount, currency, payerMessage, payeeNote);
      return Ok(result);
    }

    [HttpGet("request-to-pay-status/{requestId}")]
    public async Task<IActionResult> GetRequestToPayStatus(string requestId)
    {
      var result = await _mtnMomoService.GetRequestToPayStatusAsync(requestId);
      return Ok(result);
    }
  }
}
