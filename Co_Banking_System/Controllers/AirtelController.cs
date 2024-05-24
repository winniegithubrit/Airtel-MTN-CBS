using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Co_Banking_System.Services;

namespace Co_Banking_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AirtelController : ControllerBase
    {
        private readonly AirtelApiClient _airtelApiClient;

        public AirtelController(AirtelApiClient airtelApiClient)
        {
            _airtelApiClient = airtelApiClient;
        }

        [HttpPost("cashin")]
        public async Task<IActionResult> CashIn([FromBody] object cashInRequest)
        {
            try
            {
                var endpoint = "standard/v2/cashin/";
                var response = await _airtelApiClient.MakeCashInRequest(endpoint, cashInRequest);
                return Ok(response);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
