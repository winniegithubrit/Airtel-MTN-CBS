using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging; 
using System.Net.Http;
using System.Threading.Tasks;
using Co_Banking_System.Services;

namespace Co_Banking_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirtelController : ControllerBase
    {
        private readonly AirtelApiClient _airtelApiClient;
        private readonly ILogger<AirtelController> _logger; 

        public AirtelController(AirtelApiClient airtelApiClient, ILogger<AirtelController> logger) // Modify constructor
        {
            _airtelApiClient = airtelApiClient;
            _logger = logger; // Initialize logger
        }

        [HttpPost("cashin")]
        public async Task<IActionResult> CashIn([FromBody] object requestBody)
        {
            _logger.LogInformation("Received cash-in request.");
            try
            {
                var response = await _airtelApiClient.MakeCashInRequest("your-endpoint", requestBody);
                _logger.LogInformation("Cash-in request successful.");
                return Ok(response);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Cash-in request failed.");
                return StatusCode((int)(ex.StatusCode ?? System.Net.HttpStatusCode.InternalServerError), ex.Message);
            }
        }

        [HttpGet("token")]
        public async Task<IActionResult> GenerateToken()
        {
            _logger.LogInformation("Received request to generate token.");
            try
            {
                var token = await _airtelApiClient.GetAccessTokenAsync();
                _logger.LogInformation("Token generation successful.");
                return Ok(new { access_token = token });
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Token generation failed.");
                return StatusCode((int)(ex.StatusCode ?? System.Net.HttpStatusCode.InternalServerError), ex.Message);
            }
        }
    }
}
