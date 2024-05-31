using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Co_Banking_System.Services;

namespace Co_Banking_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AirtelController : ControllerBase
    {
        private readonly AirtelApiClient _airtelApiClient;
        private readonly ILogger<AirtelController> _logger;

        public AirtelController(AirtelApiClient airtelApiClient, ILogger<AirtelController> logger)
        {
            _airtelApiClient = airtelApiClient;
            _logger = logger;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateToken()
        {
            _logger.LogInformation("Token generation request received.");
            try
            {
                var accessToken = await _airtelApiClient.GetAccessToken();
                _logger.LogInformation("Token generated successfully.");
                return Ok(new { access_token = accessToken });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to generate access token.");
                return StatusCode(500, new { message = "Failed to generate access token" });
            }
        }

        [HttpPost("cashin")]
        public async Task<IActionResult> CashIn([FromBody] object cashInRequest)
        {
            if (cashInRequest == null)
            {
                _logger.LogError("CashIn request body is null.");
                return BadRequest(new { message = "Request body cannot be null" });
            }

            try
            {
                var endpoint = "standard/v2/cashin/";
                var response = await _airtelApiClient.MakeCashInRequest(endpoint, cashInRequest);
                return Ok(response);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP request to Airtel API failed.");
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred.");
                return StatusCode(500, new { message = "An unexpected error occurred" });
            }
        }

        // New CashOut endpoint
        [HttpPost("cashout")]
        public async Task<IActionResult> CashOut([FromBody] object cashOutRequest)
        {
            if (cashOutRequest == null)
            {
                _logger.LogError("CashOut request body is null.");
                return BadRequest(new { message = "Request body cannot be null" });
            }

            try
            {
                var endpoint = "standard/v2/cashout/";
                var response = await _airtelApiClient.MakeCashOutRequest(endpoint, cashOutRequest);
                return Ok(response);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP request to Airtel API failed.");
                return StatusCode(500, new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred.");
                return StatusCode(500, new { message = "An unexpected error occurred" });
            }
        }
    }
}
