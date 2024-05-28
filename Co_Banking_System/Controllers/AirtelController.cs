using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Co_Banking_System.Services;

namespace Co_Banking_System.Controllers
{
    // specifying that this class is an API contoller and it will handle HTTP requests
    [ApiController]
    // defining the base root for all actions in this controller
    [Route("api/[controller]")]
    public class AirtelController : ControllerBase
    {
// it holds the injected AirtelApiClient Service hence private
        private readonly AirtelApiClient _airtelApiClient;
// constructor that will accept an AirtelApiClient instance through dependency injection
        public AirtelController(AirtelApiClient airtelApiClient)
        {
            // assigning the injected AirtelApiClient to a private field
            _airtelApiClient = airtelApiClient;
        }
// handling the POST request to the api/airtel/cashin route
        [HttpPost("cashin")]
        public async Task<IActionResult> CashIn([FromBody] object cashInRequest)
        {
            try
            {
                // cashin endpoint
                var endpoint = "standard/v2/cashin/";
                /// Uses the AirtelApiClient to make the cash-in request, passing the endpoint and request data
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
