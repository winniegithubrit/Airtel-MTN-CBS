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
    }
}
